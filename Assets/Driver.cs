using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{


    public bool isDriving;
    public Transform driverPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

     /*
        if(isDriving)
        {
            GetComponent<CharacterController>().enabled = false;
            GetComponent<Collider>().enabled = false;
            GetComponent<PlayerController>().enabled = false;

            GetComponentInChildren<Animator>().SetBool("isDriving", true);
            transform.SetParent(driverPos);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            GetComponent<PlayerController>().child.localRotation = Quaternion.identity; 
        }*/
    }
     
}
