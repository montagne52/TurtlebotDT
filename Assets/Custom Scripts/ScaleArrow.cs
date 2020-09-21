using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleArrow : MonoBehaviour
{
    float[] scaleMap = new float[10];
    public GameObject xArrow;
    public GameObject yArrow;
    public GameObject zArrow;

    public GameObject totalArrow;
    private Vector3 directionTotalArrow;
    private float timeToSetDifferentArrow = 0f;

    private Vector3 defaultDirectionTotalArrow = new Vector3(0, 1, 0);
    
    // om even iets te hebben voor de scaling
    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            scaleMap[i] = i * 0.1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int scale = Mathf.FloorToInt(Mathf.Clamp(Time.time, 0, 9));

        //xArrow.transform.localScale = Vector3.one * scaleMap[scale];
        yArrow.transform.localScale = Vector3.one * scaleMap[scale];
        zArrow.transform.localScale = Vector3.one * scaleMap[scale];

        scaleXArrow(scaleMap[scale]);

        if(Time.time > timeToSetDifferentArrow) // every second
        {
            Debug.Log("scale: " + scale + ", MAp : " + scaleMap[scale]);

            float x = Random.Range(0.0f, 1.0f);
            float y = Random.Range(0.0f, 1.0f);
            float z = Random.Range(0.0f, 1.0f);
            directionTotalArrow = new Vector3(x, y, z);

            Quaternion rotationTotalArrow = Quaternion.FromToRotation(defaultDirectionTotalArrow, directionTotalArrow); 
            // Quaternions moet je niet willen proberen te begrijpen trouwens, gewoon gebruiken.
            
            totalArrow.transform.localScale = Vector3.one * directionTotalArrow.magnitude;
            totalArrow.transform.rotation = rotationTotalArrow;

            Debug.Log("Total Arrow: " + directionTotalArrow);

            timeToSetDifferentArrow += 1f;
        }
    }

    // zo zou je de X van buiteaf kunnen scalen, of dit allemaal gewoon in je mapdrawer class gooien uiteraard, keuze is aan jou
    public void scaleXArrow(float scale)
    {
        // misschien uit zetten wanneer geen signaal
        if(float.IsNaN(scale) || scale == 0)
        {
            xArrow.SetActive(false);
        }
        else
        {
            xArrow.SetActive(true);
            xArrow.transform.localScale = Vector3.one * scale;
        }
    }

    // wel oppassen je past dus de local scale aan, dus als je nog objecten in/onder de arrow objecten gaat zetten klopt er qua vectoren niks meer van (alles daaronder zit in de local wereld van hun parent die dan dus gescaled is), maar hier zal je wel niks mee doen ;)
}
