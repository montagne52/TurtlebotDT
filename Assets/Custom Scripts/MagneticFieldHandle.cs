using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticFieldHandle : MonoBehaviour
{
    public Vector3 currentReading;
    public GameObject vectorPrefab;
    public Vector3 testVector;
    public Vector3 posTestVector;

    protected GameObject vector;
    protected Transform vectorParent;
    protected float scaleX = 0.008f;
    protected float scaleY = 0.05f;
    protected float scaleZ = 0.008f;
    protected bool isMessageReceived = false;


    protected Transform vectorOrigin;

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
        Vector3 scaledReading = currentReading * scalar;
        //Vector3 scaledReading = testVector * scalar;

        // Determine scale
        float newMagnitude = scaledReading.magnitude;
        //Debug.Log(newMagnitude);

        // Determine rotation
        float newRotX;
        float newRotY;
        float newRotZ;
        /*
        if (scaledReading.y > 0)
        {
            newRotX = (Mathf.Rad2Deg * Mathf.Atan2(scaledReading.z, scaledReading.y));
            newRotZ = -(Mathf.Rad2Deg * Mathf.Atan2(scaledReading.x, scaledReading.y));
        } else if (scaledReading.y < 0)
        {
            newRotX = (Mathf.Rad2Deg * Mathf.Atan2(scaledReading.z, scaledReading.y)) + 180;
            newRotZ = -((Mathf.Rad2Deg * Mathf.Atan2(scaledReading.x, scaledReading.y)) + 180);
        } else
        {
            newRotX = 90;
            newRotZ = 90;
        }

        if (scaledReading.x > 0)
            newRotY = -(Mathf.Rad2Deg * Mathf.Atan2(scaledReading.z, scaledReading.x));
        else if (scaledReading.x < 0)
            newRotY = -(Mathf.Rad2Deg * Mathf.Atan2(scaledReading.z, scaledReading.x)) - 180;
        else
            newRotY = -90;
        */

        // THIS IS IT! See: spherical coordinate system
        newRotX = Mathf.Acos(scaledReading.y / newMagnitude) * Mathf.Rad2Deg;
        newRotY = Mathf.Atan(scaledReading.x / scaledReading.z) * Mathf.Rad2Deg;
        newRotZ = 0.0f;

        // Determine position
        float newPosX = Mathf.Cos(Mathf.Deg2Rad * newRotY) * newMagnitude;
        float newPosY = Mathf.Cos(Mathf.Deg2Rad * newRotZ) * newMagnitude;
        float newPosZ = -(Mathf.Sin(Mathf.Deg2Rad * newRotY)) * newMagnitude;

        //Debug.Log("RotX = " + newRotX);
        //Debug.Log("RotY = " + newRotY);
        //Debug.Log("RotZ = " + newRotZ);

        //Debug.Log(vectorOrigin.position);

        // Set new scale, rotation and position
        //vec.transform.localScale = newScale;
        //vec.transform.localPosition = new Vector3(0, newMagnitude, 0);
        //vec.transform.eulerAngles = new Vector3(90, 0, 0);
        //vec.transform.rotation = Quaternion.LookRotation(testVector);
        vector.transform.localScale = new Vector3(scaleX, newMagnitude, scaleZ);
        vector.transform.eulerAngles = new Vector3(newRotX, newRotY, newRotZ);
        //vector.transform.localPosition = new Vector3(newPosX, newPosY, newPosZ);
        vector.transform.localPosition = new Vector3(0, 0, 0);
        //vector.transform.Translate(vectorOrigin.transform.localPosition);
        //vector.transform.localPosition = new Vector3(0, 0, 0);
    }

    protected void InstantiateVector(Transform parent)
    {
        vector = Instantiate(vectorPrefab, parent);
        vectorParent = parent;
        vectorOrigin = vector.transform.Find("Origin");
        vector.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        vector.transform.position = parent.position + new Vector3(0, scaleY, 0);
    }
}
