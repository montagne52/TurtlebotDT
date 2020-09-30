/*
 * Simple script allowing to delete environment objects whenever they are not relevant
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{ 
    public bool enableEnvironment;

    private GameObject[] walls;

    // Start is called before the first frame update
    void Start()
    {
        //walls = new GameObject[]
        //{
        //    GameObject.Find("WallNorth"),
        //    GameObject.Find("WallWest"),
        //    GameObject.Find("WallSouth"),
        //    GameObject.Find("WallEast"),
        //    GameObject.Find("Tree_1"),
        //    GameObject.Find("Tree_2"),
        //    GameObject.Find("Tree_3"),
        //    GameObject.Find("SceneryCube"),
        //    GameObject.Find("Car White"),
        //    GameObject.Find("BoundedArea"),
        //};

        //if (!enableEnvironment)
        //{
        //    for (int i = 0; i < walls.Length; i++)
        //    {
        //        Destroy(walls[i]);
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
