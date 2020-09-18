using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticFieldHandle : MonoBehaviour
{
    public Vector3 currentReading;
    public GameObject vectorPrefab;
    public Vector3 testReading;

    protected GameObject vector;
    protected Transform vectorParentTf;
    protected float scaleX = 0.008f;
    protected float scaleY = 0.05f;
    protected float scaleZ = 0.008f;
    protected bool isMessageReceived = false;
    protected bool debugMode = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Write(Vector3 fieldVector)
    {
        currentReading = fieldVector;
        isMessageReceived = true;
    }

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
        vector.transform.localScale = new Vector3(scaleX, newMagnitude, scaleZ);
        vector.transform.localEulerAngles = new Vector3(newRotX, newRotY, newRotZ);
    }

    protected void InstantiateVector(Transform parent)
    {
        vector = Instantiate(vectorPrefab, parent);
        vectorParentTf = parent;
        vector.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        vector.transform.position = parent.position;
    }

    protected virtual void DebugMethod()
    {

    }
}
