/*
 * Make the camera follow the turtlebot when it moves
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtlebotFollower : MonoBehaviour
{
    private Transform turtlebotTransform;

    [SerializeField]
    private Vector3 offsetPosition;

    [SerializeField]
    private Space offsetPositionSpace = Space.Self;

    [SerializeField]
    private bool lookAt = true;

    private void Start()
    {
        offsetPosition.x = 0;
        offsetPosition.y = 0.25f;
        offsetPosition.z = -0.5f;
        turtlebotTransform = GameObject.Find("base_link").GetComponent<Transform>();
    }

    private void Update()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (turtlebotTransform == null)
        {
            Debug.LogWarning("Missing target ref !", this);

            return;
        }

        // compute position
        if (offsetPositionSpace == Space.Self)
        {
            transform.position = turtlebotTransform.TransformPoint(offsetPosition);
        }
        else
        {
            transform.position = turtlebotTransform.position + offsetPosition;
        }

        // compute rotation
        if (lookAt)
        {
            transform.LookAt(turtlebotTransform);
        }
        else
        {
            transform.rotation = turtlebotTransform.rotation;
        }
    }
}

