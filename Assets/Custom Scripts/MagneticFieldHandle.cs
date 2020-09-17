using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticFieldHandle : MonoBehaviour
{
    /*
    public float[][] xMap;
    public float[][] yMap;
    public float[][] zMap;
    */
    public Vector3 currentReading;

    protected bool isMessageReceived = false;

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


    /*
    private void OnDrawGizmos()
    {
        Debug.Log("Draw Vector");
        Vector3 pos = transform.position;
        Vector3 dir = new Vector3(10, 10, 10);
        DrawArrow.ForGizmo(pos, dir);
    }
    */
}
