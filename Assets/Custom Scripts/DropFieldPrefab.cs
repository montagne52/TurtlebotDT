using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;

// Couple to MATLAB, e.g. in background do interpolation and when interpolation finished send to Unity and draw map

public class DropFieldPrefab : MagneticFieldHandle
{
    public GameObject fieldPrefab;
    public GameObject prefabParent;
    public Vector3 posOffset;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DropPrefab", 3, 1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DropPrefab()
    {
        Instantiate(fieldPrefab, gameObject.transform.position + posOffset, gameObject.transform.rotation, prefabParent.transform);
    }
}
