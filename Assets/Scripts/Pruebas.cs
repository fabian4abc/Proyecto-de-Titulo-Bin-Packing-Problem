using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pruebas : MonoBehaviour
{

    public GameObject pol1;
    public GameObject pol2;
    public GameObject pol3;
    public GameObject pol4;
    public int c;

    // Start is called before the first frame update
    void Start()
    {
        // 3.06 y 0.28
        GameObject g;
        Renderer rend;
        Vector3 size;
        float sizex;
        float sizey;
        g = GameObject.Find("Up");
        rend = g.GetComponent<Renderer>();
        size = rend.bounds.size;
        sizex = rend.bounds.size.x;
        sizey = rend.bounds.size.y;
        g = GameObject.Find("Down");
        rend = g.GetComponent<Renderer>();
        size = rend.bounds.size;
        sizex = rend.bounds.size.x;
        sizey = rend.bounds.size.y;
        g = GameObject.Find("Left");
        rend = g.GetComponent<Renderer>();
        size = rend.bounds.size;
        sizex = rend.bounds.size.x;
        sizey = rend.bounds.size.y;
        g = GameObject.Find("Right");
        rend = g.GetComponent<Renderer>();
        size = rend.bounds.size;
        sizex = rend.bounds.size.x;
        sizey = rend.bounds.size.y;

        Instantiate(pol1);
        Instantiate(pol2);
        Instantiate(pol3);
        Instantiate(pol4);
        Instantiate(pol1);
        Instantiate(pol2);
        Instantiate(pol1);
        Instantiate(pol2);
        Instantiate(pol3);
        Instantiate(pol4);
        Instantiate(pol1);
        Instantiate(pol2);

        
    }




    // Update is called once per frame
    void FixedUpdate()
    {
        if (c++ == 200)
        {
            var a = GameObject.Find("P3(Clone)");
            var b = Instantiate(pol4, a.transform.GetChild(0).transform.position, Quaternion.identity);
            print(a.name);
            Destroy(a);
        }
    }
}
