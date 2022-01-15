using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    
    private Vector3 jumpHeight = new Vector3(0,0,0);
    public CharacterController characterController;
    public GameObject groundChecker;
    private bool isGround;
    public LayerMask ground;
    private Vector3 yVelocity = new Vector3 (0, 0, 0);
    private float groundPull = -55;
    
    
    // Start is called before the first frame update
    void Start()
    {
        

       
    }

    // Update is called once per frame
    void Update()
    {
        
        isGround = Physics.CheckSphere(groundChecker.transform.position, 0.4f, ground);
        //if else statement determines actions based on whether the CheckSphere is touching the "ground" layermask
        if (!isGround)
        {
            //We use Time.deltaTime so the gravity will be independent of player's framerate.
            yVelocity.y += Physics.gravity.y * Time.deltaTime * 2;
            characterController.stepOffset = 0;
        }
        else // Inside this else statement, all code applies when falling or jumping
        {
            
            
            if(yVelocity.y < 0) // Inside this if statement, the codes only work for falling
            {
                yVelocity.y = groundPull;
                characterController.stepOffset = 0.5f;

            }
            
        }
        //Calculates verticaly velocity and applies it to gameobject.
        //yVelocity = gravity + jumpHeight;
        characterController.Move((yVelocity) * Time.deltaTime);
        //print(isGround);



    }

   public void jump(InputAction.CallbackContext context)
   {
        
        if (context.performed)
        {
            if (isGround)
            {
                jumpHeight = new Vector3(0, 8f, 0);
                yVelocity = jumpHeight;
            }
        }
        

        
        
    
   }
}

