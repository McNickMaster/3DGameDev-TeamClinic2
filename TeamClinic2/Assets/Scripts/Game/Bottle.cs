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
            GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in Enemies)
            {
                if (Vector3.Distance(this.transform.position, enemy.transform.position) <= 20)
                {
                    Owens_TestEnemy brain = enemy.GetComponent<Owens_TestEnemy>();
                    brain.FollowBottle(this.transform.position);
                }
            }

            Instantiate(sound, transform.position, transform.rotation, null);
            Instantiate(shards, transform.position, transform.rotation, null);
            Destroy(this.gameObject);
        }
        
    }
}
