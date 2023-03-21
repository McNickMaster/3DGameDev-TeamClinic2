using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public static PlayerMovement instance;

    [Header("Movement Settings")]
    public float movespeed = 1.3f, gravForce;
    public float crouchHeight = 0.6f, crouchSpeedMod = 0.75f;


    private float moveMod = 1;
    private float yMod = 1;

    [Header("Camera Settings")]
    
    public float sensMultiplier = 1f;
    private float xRotation;
    private float sensitivity = 50f;
    public float leanAngle = 5f, leanShift = 0.25f;

    [Header("Modules")]
    public Transform body, playerCam;
    public Rigidbody rb;

    private bool crouched = false, justCrouched = false;
    private bool grounded = false;
    private Camera myCamera;

    
    private float desiredX, desiredZ, desiredShift;



    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        instance = this;
        myCamera = playerCam.GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckCrouch();
        Look();

   
        

        
    }



    private void Jump()
    {
        
    }

    private void Move()
    {
        Vector3 velocity;

        velocity = GetInput_Translation() * movespeed * Time.fixedDeltaTime;

        velocity = (velocity.x * body.transform.right) + (velocity.z * body.transform.forward);

        //charController.Move(velocity.x * body.transform.right + velocity.z * body.transform.forward + (gravForce * Vector3.down));
        float y = grounded ? 0 : gravForce * yMod  * Time.fixedDeltaTime;
        rb.velocity = (moveMod * (velocity.x * body.transform.right + velocity.z * body.transform.forward) + (y * Vector3.down));
        if(crouched)
        {
            rb.velocity *= crouchSpeedMod;
        }
    }

    private void CheckCrouch()
    {
        justCrouched = false;
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(crouched)
            {
                crouched = false;
            } else if(!crouched)
            {
                crouched = true;
                justCrouched = true;
            }
        }

        
        transform.localScale = 0.6f * new Vector3(1, crouched ? crouchHeight:1
        , 1);

        if(justCrouched)
        {
            transform.Translate(Vector3.down * crouchHeight * 1.25f);
        }
    }

    private void Look() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

    
        int leanDir = 0;
        if(Input.GetKey(KeyCode.Q))
        {
            leanDir = 1;
        }
        if(Input.GetKey(KeyCode.E))
        {
            leanDir = -1;
        }

        leanDir = GetInput_Translation().magnitude > 0.1f ? 0 : leanDir;

        float target_camera_tilt = leanAngle * leanDir;
        desiredZ = Mathf.Lerp(desiredZ,target_camera_tilt,0.1f);
        
        float target_camera_shift = leanShift * -leanDir;
        desiredShift = Mathf.Lerp(desiredShift,target_camera_shift,0.1f);

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, desiredZ);
        playerCam.transform.localPosition = desiredShift * body.transform.right;
        body.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    private Vector3 GetInput_Translation()
    {
        Vector3 input_dir = Vector3.zero;

        input_dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        Vector3.ClampMagnitude(input_dir, 1);

        return input_dir;
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            GameManager.instance.Win();
        }

        if(other.gameObject.layer == 11)
        {
            GameManager.instance.PlayerDied();
        }
    }

    void OnTriggerExit(Collider other)
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        //ground
        if(collision.collider.gameObject.layer.Equals(6))
        {
            grounded = true;
        }
    }
    
    void OnCollisionStay(Collision collision)
    {
        //ground
        if(collision.collider.gameObject.layer.Equals(6))
        {
            grounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        //ground
        if(collision.collider.gameObject.layer.Equals(6))
        {
            grounded = false;
        }
    }

    
}
