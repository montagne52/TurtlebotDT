using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class MyMatlabSubscribers : MonoBehaviour
    {
        public string[] topicNames;
        public GameObject target;
        private MagneticFieldDrawer magneticFieldDrawer;
        private MyFloat32MultiArraySubscriber[] myFloat32MultiArraySubscribers;

        // Start is called before the first frame update
        void Start()
        {
            if (topicNames != null && target != null)
            {
                magneticFieldDrawer = target.GetComponent<MagneticFieldDrawer>();
                initSubscribers();
            }
            else
            {
                Debug.Log("Error: Topic Names or Target GameObject not defined");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void initSubscribers()
        {
            myFloat32MultiArraySubscribers = new MyFloat32MultiArraySubscriber[topicNames.Length];
            for (int i = 0; i < topicNames.Length; i++)
            {
                myFloat32MultiArraySubscribers[i] = gameObject.AddComponent<MyFloat32MultiArraySubscriber>();
                myFloat32MultiArraySubscribers[i].Topic = topicNames[i];
                myFloat32MultiArraySubscribers[i].setTarget(magneticFieldDrawer);
            }
        }
    }
}

