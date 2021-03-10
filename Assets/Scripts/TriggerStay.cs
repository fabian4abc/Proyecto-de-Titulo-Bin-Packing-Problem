using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerStay : MonoBehaviour
{
    // Start is called before the first frame update
    private int c;
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!GameObject.Find("Box").GetComponent<A_2>().bol)  
        {
            c++;
            if (string.Compare(collision.transform.parent.gameObject.name,this.transform.parent.gameObject.name)<0)
            {
                if (collision.transform.parent.gameObject.name != "Box" && c >= 100)
                {
                    Destroy(collision.transform.parent.gameObject);
                }
            }
        }
    }
}
