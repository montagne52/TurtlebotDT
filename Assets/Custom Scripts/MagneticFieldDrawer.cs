/*
 * Process the data coming from MATLAB and draw the magnetic field heat map
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MagneticFieldDrawer : MonoBehaviour
{
    public GameObject turtlebot;   // the MagneticFieldDrawer must be located at initial position of turtlebot
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
    private float[] zFieldValuesStacked;  // remember: this can be anything (scalar potential, norm, whatever you like...)
    private float[] normalisedData;

    private int xSize;  // number of vertices in x direction
    private int zSize;  // [PLACEHOLDER] number of vertices in z direction

    // Start is called once at the start
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        NaNColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);  // transparent for NaNs
        colorGradient = new Gradient();
        defaultGradientColors = new Color[]
        {
            Color.blue, Color.cyan, Color.green, Color.yellow, Color.red
        };
        gradientColors = defaultGradientColors;
        SetColorGradient();

        transform.position = turtlebot.transform.position + new Vector3(0.0f, 0.01f, 0.0f);
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

        
        // Determine gridsize
        xSize = 0;
        float xCompare = vertices[xSize].x;
        do
        {
            xSize++;
        }
        while (vertices[xSize].x != xCompare);

        zSize = 1;
        for (int i = 1; i < vertices.Length; i++)
        {
            if (vertices[i].z != vertices[i - 1].z)
                zSize++;
        }

        // Define triangles of the mesh with a nasty algorithm, the general idea is
        // that the triangles array holds the indeces of vertices that form each triangle
        // please see unity API or google on procedural mesh generation, e.g.:
        // https://catlikecoding.com/unity/tutorials/procedural-grid/
        int nTriangles = (this.xSize - 1) * (zSize - 1) * 2;
        triangles = new int[nTriangles * 3];

        for (int z = 0; z < zSize - 1; z++)
        {
            for (int i = 0, x = 0; x < this.xSize - 1; i += 6, x++)
            {
                // first triangle of a 'square' (area bounded by four vertices)
                triangles[(z * (this.xSize - 1) * 6) + i] = x + (z * this.xSize);
                triangles[(z * (this.xSize - 1) * 6) + i + 1] = x + 1 + (z * this.xSize);
                triangles[(z * (this.xSize - 1) * 6) + i + 2] = this.xSize + x + (z * this.xSize);

                // second triangle of the square
                triangles[(z * (this.xSize - 1) * 6) + i + 3] = x + 1 + (z * this.xSize);
                triangles[(z * (this.xSize - 1) * 6) + i + 4] = this.xSize + x + 1 + (z * this.xSize);
                triangles[(z * (this.xSize - 1) * 6) + i + 5] = this.xSize + x + (z * this.xSize);
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

