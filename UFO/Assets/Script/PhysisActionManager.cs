using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PhysisActionManager : MonoBehaviour, IActionManager
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Fly(GameObject ufo, float angle, float v)
    {
        if (ufo.GetComponent<Rigidbody>() == null)
        {
            ufo.AddComponent<Rigidbody>();
        }

        float v_x = v * 20f / (float)(Math.Sqrt((400f + angle * angle)));
        float v_y = v * angle / (float)(Math.Sqrt((400f + angle * angle)));
        if (ufo.GetComponent<UfoProperty>().direction.x == -1)
        {
            ufo.GetComponent<Rigidbody>().velocity = new Vector3(v_x*-1, v_y, 0);
        }
        else
        {
            ufo.GetComponent<Rigidbody>().velocity = new Vector3(v_x, v_y, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
