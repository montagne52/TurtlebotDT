/*
 * Script for subscribing to /magnetic_field topic in order to receive sensor readings
 * of the integrated magnetometer
 */

using RosSharp.RosBridgeClient.MessageTypes.Sensor;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class MyMagnetometerSubscriber : UnitySubscriber<MessageTypes.Sensor.MagneticField>
    {
        public Vector3 magneticFieldVector;                 // In Unity coordinate system!
        private Magnetometer magnetometer;

        protected override void Start()
        {
            base.Start();
            magnetometer = GameObject.Find("Magnetometer").GetComponent<Magnetometer>();
            magneticFieldVector = new Vector3(0, 0, 0);
        }

        private void Update()
        {

        }

        private void FixedUpdate()
        {

        }

        protected override void ReceiveMessage(MagneticField message)
        {
            // The xyz components are reordered such that the directions in Unity correspond
            // with the NED orientation of the RM3100 measurements
            magneticFieldVector.Set(
                (float)message.magnetic_field.y,
                -(float)message.magnetic_field.z,
                (float)message.magnetic_field.x);
            magnetometer.Write(magneticFieldVector);
        }
    }
}