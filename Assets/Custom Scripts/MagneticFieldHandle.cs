/*
 * MagneticFieldHandle base class. all behavour relevant to drawing the arrow in Unity.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticFieldHandle : MonoBehaviour
{
    public Vector3 currentReading; // latest sensor reading
    public Vector3 testReading; // for debugging, e.g. when robot not running
    public GameObject vectorPrefab; // gameobject for visualising magnetic field vector

    //public GameObject xComponent;
    //public GameObject yComponent;
    //public GameObject zComponent;

    [SerializeField]  protected float xScale = 0.1f;  // choose preferred scaling of vector gameobject
    [SerializeField]  protected float zScale = 0.1f;  // choose preferred scaling of vector gameobject
    protected float yScale = 0.05f; // just for initialising

    protected GameObject vector; // the actual vector gameobject based on prefab
    protected bool isMessageReceived = false;
    protected bool debugMode = false; // if enabled, DebugMethod can be used

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Write is called by the RosSubscriber each time new data is received
    public void Write(Vector3 fieldVector)
    {
        currentReading = fieldVector;
        isMessageReceived = true;
    }

    // DrawVector scales, rotates and transforms a (vector) gameobject according to the magnetic field reading
    protected void DrawVector(float scalar)
    {
        // Scale the current sensor reading to allow for appropriate 3D object sizes
        Vector3 scaledReading;
        scaledReading = (debugMode) ? testReading * scalar : currentReading * scalar;

        // Determine magnitude
        float newMagnitude = scaledReading.magnitude;

        // Determine rotation
        float newRotX;
        float newRotY;
        float newRotZ;

        // Determine cylinder object rotation based on spherical coordinate system
        newRotX = Mathf.Acos(scaledReading.y / newMagnitude) * Mathf.Rad2Deg;
        newRotY = Mathf.Atan(scaledReading.x / scaledReading.z) * Mathf.Rad2Deg;
        newRotZ = 0.0f;
        if (scaledReading.z < 0) { newRotX = -newRotX; }
        if (float.IsNaN(newRotX)) { newRotX = 0.0f; }
        if (float.IsNaN(newRotY)) { newRotY = 0.0f; }
        if (float.IsNaN(newRotZ)) { newRotZ = 0.0f; }

        // Set new scale and rotation
        vector.transform.localScale = new Vector3(xScale, newMagnitude, zScale);
        vector.transform.localEulerAngles = new Vector3(newRotX, newRotY, newRotZ);
    }

    // [TODO] DrawVectorComponents scales, rotates and transfroms the XYZ components of the magnetic field reading
    protected void DrawVectorComponents(float scalar)
    {

    }
    
    // InstantiateVector creates the vector gameobject from a given prefab
    protected void InstantiateVector(Transform parent)
    {
        vector = Instantiate(vectorPrefab, parent);
        vector.transform.localScale = new Vector3(xScale, yScale, zScale);
        vector.transform.position = parent.position;
    }

    protected virtual void DebugMethod()
    {

    }
}
