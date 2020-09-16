/*
 * Test script for subscribing to /geomagnetic_data topic
 */

using RosSharp.RosBridgeClient.MessageTypes.Sensor;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class MyMagneticFieldSubscriber : UnitySubscriber<MessageTypes.Sensor.MagneticField>
    {
        public Vector3 magneticFieldVector;

        protected override void Start()
        {
            base.Start();
        }

        private void Update()
        {
            
        }

        private void FixedUpdate()
        {
            
        }

        protected override void ReceiveMessage(MagneticField message)
        {
            magneticFieldVector = getMagneticFieldVector(message);
        }

        private Vector3 getMagneticFieldVector(MagneticField msg)
        {
            return new Vector3(
                (float)msg.magnetic_field.x,
                (float)msg.magnetic_field.y,
                (float)msg.magnetic_field.z);
        }
    }
}