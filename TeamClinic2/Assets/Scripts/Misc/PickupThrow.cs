using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PickupThrow : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] Transform holdArea;
    private GameObject heldThing;
    private Rigidbody heldRB;


    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float throwForce = 150.0f;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldThing == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    PickupThing(hit.transform.gameObject);
                }
            }
            else
            {
                Throw();
            }
        }
        else
        {
            MoveThing();
        }
    }

    void PickupThing(GameObject Thing)
    {
        if (Thing.GetComponent<Rigidbody>() && Thing.CompareTag("Throwable"))
        {
            heldRB = Thing.GetComponent<Rigidbody>();
            heldRB.useGravity = false;
            heldRB.drag = 10;
            heldRB.constraints = RigidbodyConstraints.FreezeRotation;
            heldRB.isKinematic = true;

            heldRB.transform.parent = holdArea;

            heldThing = Thing;
        }
    }

    void MoveThing()
    {
        if (heldThing != null && Vector3.Distance(heldThing.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDir = (holdArea.position - heldThing.transform.position);
            heldRB.AddForce(moveDir * throwForce);
        }
    }

    void Throw()
    {
        heldRB.useGravity = true;
        heldRB.drag = 1;
        heldRB.constraints = RigidbodyConstraints.None;
        heldRB.transform.parent = null;
        heldRB.isKinematic = false;

        heldThing.GetComponent<Bottle>().thrown = true;

        heldThing = null;

        heldRB.AddForce(transform.TransformDirection(Vector3.forward) * (throwForce / 10), ForceMode.Impulse);
    }
}
