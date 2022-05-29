using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public CharacterController characterController;
    private CrouchAndSlide crouchSlideScript;
    public Vector3 vector3Move;
    private Vector2 moveValue = new Vector2(0, 0);
    private bool isSprinting = false;
    public float speed = 5f;
    private float acceleration;
    private float ORIGINALACCELERATION = 0.1f;
    public float maxSpeed;
    public float minSpeed;
    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.GetComponent<CrouchAndSlide>().enabled)
        {
            Debug.Log("Found CrouchAndSlide script for PlayerMove!");
            crouchSlideScript = gameObject.GetComponent<CrouchAndSlide>();
        } else
        {
            Debug.Log("CrouchAndSlide script not found");
        }
        acceleration = ORIGINALACCELERATION;
        
    }

    // Update is called once per frame
    void Update()
    {
        vector3Move = transform.right * moveValue.x + transform.forward * moveValue.y;

        //locks the player into a single direction when sliding
        if (crouchSlideScript != null)
        {
            if (crouchSlideScript.isSlidePerformed())
            {
                
                vector3Move = transform.forward * moveValue.y;
                characterController.Move(transform.forward * moveValue.y * Time.deltaTime * speed);
                
            } else
            {
                
                characterController.Move(vector3Move * Time.deltaTime * speed);
            }
        } else
        {
            characterController.Move(vector3Move * Time.deltaTime * speed);
        }
        
        //if player presses sprint key while moving forward, their character will accelerate up to the maxSpeed
        if (isSprinting && moveValue.y > 0 && !(crouchSlideScript.isSlidePerformed()))
        {
            
            speed += acceleration;
            if(speed >= maxSpeed)
            {
                speed = maxSpeed;
            }
            
        }
        //if player lets go of the sprint key or stops moving forward, character speed slows to minSpeed
        if(!(isSprinting) || moveValue.y <= 0 || crouchSlideScript.isSlidePerformed())
        {
            speed -= acceleration;
            isSprinting = false;
            if(speed <= minSpeed)
            {
                speed = minSpeed;
            }
        }
        
    }
    public void move(InputAction.CallbackContext context)
    {
        
        if (context.performed)
        {
            
            moveValue = context.ReadValue<Vector2>();
           // print(moveValue);
        }
        if(context.canceled)
        {
            moveValue = context.ReadValue<Vector2>();
            //print(moveValue);
        }
        
        
    }
    public void sprint(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            isSprinting = true;
        }
        if(context.canceled)
        {
            isSprinting = false;
        }
    }
    public bool getIsSprinting()
    {
        return isSprinting;
    }
    public void setAcceleration(float newAccel)
    {
        acceleration = newAccel;
    }
    public float getOriginalAcceleration()
    {
        return ORIGINALACCELERATION;
    }
    public Vector2 getMoveValue()
    {
        return moveValue;
    }
}
