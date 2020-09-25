/* 
 * Script for publishing the initial position of the turtlebot (which can be chosen beforehand)
 * relative of the room or driving area. This data is needed in MATLAB to
 * do the computations of e.g. interpolating magnetic field strength
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RosSharp.RosBridgeClient
{
    public class MyPositionsPublisher : UnityPublisher<MessageTypes.Std.Float64MultiArray>
    {
        public GameObject turtlebot;
        public GameObject wallNorth;
        public GameObject wallWest;
        public GameObject wallSouth;
        public GameObject wallEast;

        private MessageTypes.Std.Float64MultiArray message;
        private double[] initialPositions;  // initial configuration, sent to MATLAB

        private double distanceToNorth;     // distance between Turtlebot and north boundary (Z direction)
        private double distanceToWest;      // distance between Turtlebot and west boundary (-X direction)
        private double distanceToSouth;     // distance between Turtlebot and south boundary (-Z direction)
        private double distanceToEast;      // distance between Turtlebot and north boundary (X direction)

        protected override void Start()
        {
            base.Start();
            InitializeMessage();
            InvokeRepeating("UpdateMessage", 0, 2);
        }

        private void FixedUpdate()
        {

        }

        private void InitializeMessage()
        {
            message = new MessageTypes.Std.Float64MultiArray();
            distanceToNorth = wallNorth.transform.position.z - turtlebot.transform.position.z;
            distanceToWest = wallWest.transform.position.x - turtlebot.transform.position.x;
            distanceToSouth = wallSouth.transform.position.z - turtlebot.transform.position.z;
            distanceToEast = wallEast.transform.position.x - turtlebot.transform.position.x;

            initialPositions = new double[]
            {
                distanceToNorth, distanceToWest, distanceToSouth, distanceToEast
            };

            message.data = initialPositions;
        }

        private void UpdateMessage()
        {
            Publish(message);
        }
    }
}

