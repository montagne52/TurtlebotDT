/*
 * Custom joint state subscriber
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace RosSharp.RosBridgeClient
{
    public class MyJointStateSubscriber : UnitySubscriber<MessageTypes.Sensor.JointState>
    {
        public List<string> JointNames;
        public List<MyJointMotorWriter> JointMotorWriters;

        protected override void ReceiveMessage(MessageTypes.Sensor.JointState message)
        {
            int index;
            for (int i = 0; i < message.name.Length; i++)
            {
                index = JointNames.IndexOf(message.name[i]);
                //Debug.Log(index);
                if (index != -1)
                {
                    JointMotorWriters[index].Write((float)message.velocity[i]);
                    //pos = message.position[i];
                }
            }
        }
    }
}