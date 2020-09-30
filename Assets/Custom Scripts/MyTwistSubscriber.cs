/*
 * Class that subscribes to Twist message (e.g. cmd_vel or cmd_vel_rc100) in order to
 * translate and rotate the Turtlebot object in Unity based on the actual linear
 * and angular velocity of the Turtlebot in physical world.
 */

/*
© CentraleSupelec, 2017
Author: Dr. Jeremy Fix (jeremy.fix@centralesupelec.fr)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

// Adjustments to new Publication Timing and Execution Framework
// © Siemens AG, 2018, Dr. Martin Bischoff (martin.bischoff@siemens.com)

using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class MyTwistSubscriber : UnitySubscriber<MessageTypes.Geometry.Twist>
    {
        public Transform SubscribedTransform;
        public float linearVelocityScalar;
        public float angularVelocityScalar;

        private Vector3 linearVelocity;
        private Vector3 angularVelocity;

        protected override void Start()
        {
            base.Start();
        }

        protected override void ReceiveMessage(MessageTypes.Geometry.Twist message)
        {
            linearVelocity = ToVector3(message.linear).Ros2Unity();
            angularVelocity = -ToVector3(message.angular).Ros2Unity();
        }

        private static Vector3 ToVector3(MessageTypes.Geometry.Vector3 geometryVector3)
        {
            return new Vector3((float)geometryVector3.x, (float)geometryVector3.y, (float)geometryVector3.z);
        }

        private void Update()
        {
            //Debug.Log("LinearVel = " + linearVelocity.z + ", AngularVel = " + angularVelocity.y);
            SubscribedTransform.Translate(Vector3.forward * Time.deltaTime * linearVelocity.z * linearVelocityScalar);
            SubscribedTransform.Rotate(Vector3.up * Time.deltaTime * angularVelocity.y * Mathf.Rad2Deg * angularVelocityScalar);
        }
    }
}