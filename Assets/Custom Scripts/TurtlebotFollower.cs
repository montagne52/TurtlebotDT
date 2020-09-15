using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtlebotFollower : MonoBehaviour
{
    public GameObject turtlebot;
    private Vector3 posOffset;
    //private Quaternion rotOffset;

    // Start is called before the first frame update
    void Start()
    {
        posOffset = new Vector3(0, 0.25f, -0.5f);
        //rotOffset = new Quaternion(22.56f, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = turtlebot.transform.position + posOffset;
        //transform.rotation = turtlebot.transform.rotation + rotOffset;
    }
}
