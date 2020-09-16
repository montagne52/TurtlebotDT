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
        transform.position.Set(10.0f, 0.25f, -0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = turtlebot.transform.position + posOffset;
        transform.position.Set(transform.position.x + 0.1f, transform.position.y, transform.position.z);
        //Vector3 temp = turtlebot.transform.position;
        //temp.x = temp.x;
        //temp.y = temp.y + 0.25f;
        //temp.z = temp.z - 0.5f;
        //transform.position = temp;
        //Debug.Log(transform.position.x + ", " + transform.position.y + ", " + transform.position.z);
        //transform.rotation = turtlebot.transform.rotation + rotOffset;
    }
}
