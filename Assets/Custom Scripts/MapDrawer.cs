using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;

// Couple to MATLAB, e.g. in background do interpolation and when interpolation finished send to Unity and draw map

public class MapDrawer : MonoBehaviour
{
    public MyMagneticFieldSubscriber magneticFieldSubscriber;
    // var die aan Map linkt
    public GameObject map;
    // als je geen texture functie teken iets hebt, drop die prefab!
    public GameObject prefab;

    public float[][] xMap;
    public float[][] yMap;
    public float[][] zMap;
    //public Vector3[] posMap;

    // Start is called before the first frame update
    void Start()
    {
        magneticFieldSubscriber = GameObject.Find("RosConnector").GetComponent<MyMagneticFieldSubscriber>();
        //map = GameObject.Find("Map").GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 bufferValues = magneticFieldSubscriber.magneticFieldVector;
        Vector3 position = transform.position;
    }
}
