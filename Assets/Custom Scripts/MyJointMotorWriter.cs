/*
 * Custom joint motor writer intended to set the hinge joint motor velocity
 * in [deg/s] that comes from ROS topic joint_states/velocity [rad/s]
 */

using System.Collections;
using System.Collections.Generic;
using RosSharp.Urdf;
using UnityEngine;
using Joint = UnityEngine.Joint;


namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(HingeJoint))]
    public class MyJointMotorWriter : MonoBehaviour
    {
        public float velocityScalar;

        private HingeJoint _hingeJoint;
        private JointMotor jointMotor;
        private float targetVelocity; // in [deg/s]
        private bool isMessageReceived;

        private void Start()
        {
            _hingeJoint = GetComponent<HingeJoint>();
            _hingeJoint.useMotor = true;
            velocityScalar = 1.0f;
        }

        private void Update()
        {
            if (isMessageReceived)
                ProcessMessage();
        }

        private void ProcessMessage()
        {
            jointMotor = _hingeJoint.motor;
            jointMotor.targetVelocity = targetVelocity;
            _hingeJoint.motor = jointMotor;
            isMessageReceived = false;
        }

        public void Write(float value)
        {
            targetVelocity = -(value * Mathf.Rad2Deg * velocityScalar);
            isMessageReceived = true;
        }
    }

}