using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RM3100 : MagneticFieldHandle
{
    public float magnitudeScalar = 0.00001f;

    // Start is called before the first frame update
    void Start()
    {
        InstantiateVector(transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMessageReceived)
        {
            DrawVector(magnitudeScalar);
            isMessageReceived = false;
        }
    }
}
