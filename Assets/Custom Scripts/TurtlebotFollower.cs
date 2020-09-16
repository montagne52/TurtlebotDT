/*
 * Make the camera follow the turtlebot when it moves
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtlebotFollower : MonoBehaviour
{
    private Transform target;

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
        target = GameObject.Find("base_link").GetComponent<Transform>();
    }

    private void Update()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (target == null)
        {
            Debug.LogWarning("Missing target ref !", this);

            return;
        }

        // compute position
        if (offsetPositionSpace == Space.Self)
        {
            transform.position = target.TransformPoint(offsetPosition);
        }
        else
        {
            transform.position = target.position + offsetPosition;
        }

        // compute rotation
        if (lookAt)
        {
            transform.LookAt(target);
        }
        else
        {
            transform.rotation = target.rotation;
        }
    }
}

