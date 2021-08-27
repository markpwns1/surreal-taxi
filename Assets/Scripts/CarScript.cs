using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarScript : MonoBehaviour
{

    public int publicint;

    // Start is called before the first frame update
    void Start()
    {

        print (publicint);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.name == "person")
        {
            Debug.Log("collision detected");

            Destroy(col.gameObject);
        }

    }



}

