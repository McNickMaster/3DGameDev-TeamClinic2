using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{

    public LayerMask layerMask;
    public GameObject sound;
    public GameObject shards;

    public bool thrown = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.layer + " " + layerMask.value);
        if(thrown)
        {
            Instantiate(sound, transform.position, transform.rotation, null);
            Instantiate(shards, transform.position, transform.rotation, null);
            Destroy(this.gameObject);
        }
        
    }
}
