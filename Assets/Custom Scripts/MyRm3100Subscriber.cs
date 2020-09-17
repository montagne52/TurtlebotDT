/*
 * Script for subscribing to /rm3100 topic in order to receive sensor readings
 * using the rosbridge
 */

using RosSharp.RosBridgeClient.MessageTypes.Sensor;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class MyRm3100Subscriber : UnitySubscriber<MessageTypes.Sensor.MagneticField>
    {
        public Vector3 magneticFieldVector;                 // In Unity coordinate system!
        private RM3100 rm3100;

        protected override void Start()
        {
            base.Start();
            rm3100 = GameObject.Find("RM3100").GetComponent<RM3100>();
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
            rm3100.Write(magneticFieldVector);
        }
    }
}