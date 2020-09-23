/*
 * Script for subscribing to the MATLAB topic that updates on magnetic field data
 * whenever the interpretation and interpolation of sensor data has finished
 */

using RosSharp.RosBridgeClient.MessageTypes.Sensor;
using RosSharp.RosBridgeClient.MessageTypes.Std;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class MyFloat32MultiArraySubscriber : UnitySubscriber<MessageTypes.Std.Float32MultiArray>
    {
        private MagneticFieldDrawer magneticFieldDrawer;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected void Update()
        {

        }

        private void FixedUpdate()
        {
            
        }

        protected override void ReceiveMessage(Float32MultiArray message)
        {
            Debug.Log("Message received from topic: " + Topic + ", Array length = " + message.data.Length);
            magneticFieldDrawer.Write(Topic, message.data);
        }

        public void setTarget(MagneticFieldDrawer target)
        {
            magneticFieldDrawer = target;
        }
    }
}


