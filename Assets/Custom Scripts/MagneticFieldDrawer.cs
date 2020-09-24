/*
 * Process the data coming from MATLAB and draw the magnetic field heat map
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MagneticFieldDrawer : MonoBehaviour
{
    public bool recalculateField = false;
    public Color[] gradientColors;
    private Color[] defaultGradientColors;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    private Color[] colors;
    private Gradient colorGradient;
    private Color NaNColor;

    private bool xCoordinateReceived = false;
    private bool yCoordinateReceived = false;
    private bool zFieldReceived = false;

    private float[] xCoordinatesStacked;
    private float[] yCoordinatesStacked;
    [SerializeField] private float[] zFieldValuesStacked;  // remember: this can be anything (scalar potential, norm, whatever you like...)
    [SerializeField] private float[] normalisedData;

    private int xMeshLength = 201;  // [PLACEHOLDER] number of vertices in x direction
    private int zMeshLength = 301;  // [PLACEHOLDER] number of vertices in z direction

    // Start is called once at the start
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        NaNColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        colorGradient = new Gradient();
        defaultGradientColors = new Color[]
        {
            Color.blue, Color.cyan, Color.green, Color.yellow, Color.red
        };
        gradientColors = defaultGradientColors;
        SetColorGradient();
    }

    // Update is called once per frame
    void Update()
    {
        if (xCoordinateReceived && yCoordinateReceived && zFieldReceived)
        {
            UpdateMagneticField();
            xCoordinateReceived = false;
            yCoordinateReceived = false;
            zFieldReceived = false;
        }

        if (recalculateField)
        {
            SetColorGradient();
            UpdateMagneticField();
            recalculateField = false;
        }
    }

    // Write is called by the RosSubscriber each time new data is received
    public void Write(string topicName, float[] data)
    {
        if (string.Equals(topicName, "/x_coordinate"))
        {
            xCoordinateReceived = true;
            xCoordinatesStacked = data;
        }
        else if (string.Equals(topicName, "/y_coordinate"))
        {
            yCoordinateReceived = true;
            yCoordinatesStacked = data;
        }
        else if (string.Equals(topicName, "/z_field"))
        {
            zFieldReceived = true;
            zFieldValuesStacked = data;

            // Revert 0s back to NaNs (which were converted from 0 to NaN in MATLAB before)
            // Take care that legitimate '0' values are now lost, though chances are small.
            for (int i = 0; i < zFieldValuesStacked.Length; i++)
            {
                if (zFieldValuesStacked[i] == 0)
                    zFieldValuesStacked[i] = float.NaN;
            }
        }
        else
        {
            Debug.Log("topicName unknown");
        }
    }


    // Main method handling the visualisation of the magnetic field map
    private void UpdateMagneticField()
    {
        // Define vertices of the mesh which are the X and Y coordinates, note that
        // ROS coordinates are converted to Unity coordinates, i.e. X = -Y and Z = X
        vertices = new Vector3[xCoordinatesStacked.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(-yCoordinatesStacked[i], 0, xCoordinatesStacked[i]);
        }

        // Define triangles of the mesh with a nasty algorithm, the general idea is
        // that the triangles array holds the indeces of vertices that form each triangle
        // please see unity API or google on procedural mesh generation, e.g.:
        // https://catlikecoding.com/unity/tutorials/procedural-grid/
        int nTriangles = (xMeshLength - 1) * (zMeshLength - 1) * 2;
        triangles = new int[nTriangles * 3];

        for (int z = 0; z < zMeshLength - 1; z++)
        {
            for (int i = 0, x = 0; x < xMeshLength - 1; i += 6, x++)
            {
                // first triangle of a 'square' (area bounded by four vertices)
                triangles[(z * (xMeshLength - 1) * 6) + i] = x + (z * xMeshLength);
                triangles[(z * (xMeshLength - 1) * 6) + i + 1] = x + 1 + (z * xMeshLength);
                triangles[(z * (xMeshLength - 1) * 6) + i + 2] = xMeshLength + x + (z * xMeshLength);

                // second triangle of the square
                triangles[(z * (xMeshLength - 1) * 6) + i + 3] = x + 1 + (z * xMeshLength);
                triangles[(z * (xMeshLength - 1) * 6) + i + 4] = xMeshLength + x + 1 + (z * xMeshLength);
                triangles[(z * (xMeshLength - 1) * 6) + i + 5] = xMeshLength + x + (z * xMeshLength);
            }
        }


        // Define colours of the mesh (vertices) which defines the magnetic field strength at each coordinate
        // recall that each vertex is an (x,z) coordinate and has an associated field strength
        NormaliseData();
        colors = new Color[vertices.Length];


        for (int i = 0; i < colors.Length; i++)
        {
            float val = normalisedData[i];
            if (float.IsNaN(val))
            {
                colors[i] = NaNColor;
            }
            else
            {
                colors[i] = colorGradient.Evaluate(val);
            }
        }


        // Update the actual mesh
        UpdateMesh();
    }


    // normalise the data with range [0, 1]
    private void NormaliseData()
    {
        // Determine minimum and maximum of our dataset
        float min = float.NaN;

        int i = 0;
        while (float.IsNaN(min) || min == 0)
        {
            min = zFieldValuesStacked[i];
            i++;
        }
        float max = min;

        for (int j = 0; j < zFieldValuesStacked.Length; j++)
        {
            float val = zFieldValuesStacked[j];
            if (!float.IsNaN(val))
            {
                min = Mathf.Min(min, val);
                max = Mathf.Max(max, val);
            }
        }
        Debug.Log("Min = " + min + ", Max = " + max);

        // Normalise data and place into new array
        normalisedData = new float[zFieldValuesStacked.Length];
        for (int k = 0; k < zFieldValuesStacked.Length; k++)
        {
            float val = zFieldValuesStacked[k];
            if (!float.IsNaN(val))
            {
                normalisedData[k] = (val - min) / (max - min);
            }
            else
            {
                normalisedData[k] = float.NaN;
            }
        }
    }


    // Set the color gradient (may be changed from UI)
    private void SetColorGradient()
    {
        if (gradientColors.Length > 8)
        {
            Debug.Log("No more than 8 colors may be chosen!");
            gradientColors = defaultGradientColors;
        }

        GradientColorKey[] gradientColorKeys = new GradientColorKey[gradientColors.Length];
        GradientAlphaKey[] gradientAlphaKeys = new GradientAlphaKey[gradientColors.Length];

        float p = 1.0f / gradientColors.Length;
        float alpha = 1.0f;
        for (int i = 0; i < gradientColors.Length; i++)
        {
            gradientColorKeys[i] = new GradientColorKey(gradientColors[i], p*i);
            gradientAlphaKeys[i] = new GradientAlphaKey(alpha, p*i);
        }

        colorGradient.SetKeys(gradientColorKeys, gradientAlphaKeys);
    }


    // Update actual mesh object
    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }
}

