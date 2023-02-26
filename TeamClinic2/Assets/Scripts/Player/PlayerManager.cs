using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Header("Modules")]
    public PlayerMovement movement;


    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivatePlayer()
    {
        movement.enabled = true;
    }

    public void DeactivatePlayer()
    {
        movement.enabled = false;
    }
}
