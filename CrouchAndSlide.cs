using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrouchAndSlide : MonoBehaviour
{
    public GameObject playerCam;
    public float camStandHeight;
    public float camCrouchHeight;
    public float stanceSmoothTime;
    public CapsuleCollider colliderStand;
    public CapsuleCollider colliderCrouch;
    private float heightError = 0.2f;
    private float maxSpeedError = 3f;
    public CharacterController characterController;
    public GameObject stanceChecker;
    private PlayerMove playerMove;
    private Jump jumpScript;
    public float crouchSpeed;
    public float slideSpeed;
    private float slideThreshold;
    private bool slidePerformed = false;
    private float standSpeed;
    //player mask is to ignore collision with the layer "Player". The layer "Player" is meant for the player model.
    public LayerMask playerMask;
    private float currentCamHeight;
    private float stanceVelocity = 0;
    private Vector3 stancePositionVelocity = new Vector3(0,0,0);

    public enum stances
    {
        stand,
        crouch
    }
    public stances playerStances;
    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.GetComponent<PlayerMove>().enabled)
        {
            Debug.Log("PlayerMove Script found for CrouchAndSlide!");
            playerMove = GetComponent<PlayerMove>();
        } else
        {
            Debug.Log("PlayerMove Script not found for CrouchAndSlide.");
        }
        if(gameObject.GetComponent<Jump>().enabled)
        {
            Debug.Log("Jump script found for CrouchAndSlide!");
            jumpScript = GetComponent<Jump>();
        } else
        {
            Debug.Log("Jump script not found for CrouchAndSlide.");
        }
        
        
        playerStances = stances.stand;
        currentCamHeight = camStandHeight;
        characterController.height = colliderStand.height;
        standSpeed = playerMove.minSpeed;
        slideThreshold = playerMove.maxSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerStances == stances.stand)
        {
            if(!(jumpScript == null))
            {
                //slows the rate which speed changes when player is in air from jumping. Resets to normal rate when on the ground.
                if (jumpScript.getJumped())
                {
                    playerMove.setAcceleration(0.05f);
                }
                else
                {
                    playerMove.setAcceleration(playerMove.getOriginalAcceleration());
                }
            }
           
            //checks if possible for player to stand up. Player height freezes the moment canChangeStance() is false. Player height continues when true.
            if (canChangeStance())
            {
                currentCamHeight = Mathf.SmoothDamp(currentCamHeight, camStandHeight, ref stanceVelocity, stanceSmoothTime);
                playerCam.transform.localPosition = new Vector3(playerCam.transform.localPosition.x, currentCamHeight, playerCam.transform.localPosition.z);
                characterController.height = Mathf.SmoothDamp(characterController.height, colliderStand.height, ref stanceVelocity, stanceSmoothTime);
                characterController.center = Vector3.SmoothDamp(characterController.center, new Vector3(0, 0, 0), ref stancePositionVelocity, stanceSmoothTime);
                //Checks if player is at proper height to move at standSpeed.
                if (characterController.height >= (colliderStand.height - heightError))
                {
                    playerMove.minSpeed = standSpeed;
                }
            }
            //if player speed is smaller/equal to the slideThreshold, or the player is sprinting, player minSpeed is set to stand Speed and maxSpeed is set to slideThreshold
            if(playerMove.speed <= (slideThreshold - maxSpeedError) || playerMove.getIsSprinting())
            {
                playerMove.minSpeed = standSpeed;
                playerMove.maxSpeed = slideThreshold;
                slidePerformed = false;
                
            }
            
            
            
        } else if(playerStances == stances.crouch)
        {
            
            currentCamHeight = Mathf.SmoothDamp(currentCamHeight, camCrouchHeight, ref stanceVelocity, stanceSmoothTime);
            playerCam.transform.localPosition = new Vector3(playerCam.transform.localPosition.x, currentCamHeight, playerCam.transform.localPosition.z);
            characterController.height = Mathf.SmoothDamp(characterController.height, colliderCrouch.height, ref stanceVelocity, stanceSmoothTime);
            characterController.center = Vector3.SmoothDamp(characterController.center, new Vector3(0, colliderCrouch.center.y, 0), ref stancePositionVelocity, stanceSmoothTime);
            if(!(jumpScript == null))
            {
                //slows the rate which speed changes when player is in air from jumping. Resets to normal rate when on the ground.
                if (jumpScript.getJumped())
                {
                    playerMove.setAcceleration(0.05f);
                }
                else
                {
                    playerMove.setAcceleration(playerMove.getOriginalAcceleration());
                }
            }
            //code block for sliding. Slide will be performed if
            //1. Speed is greater than required speed to slide (slideThreshold)
            //2. Slide is currently not being performed
            //3. Player is moving forward
            if (playerMove.speed >= (slideThreshold - maxSpeedError) && !(slidePerformed) && (playerMove.getMoveValue().y > 0))
            {
                
                playerMove.maxSpeed = slideSpeed;
                playerMove.speed = slideSpeed;
                slidePerformed = true;
                

            } else if(playerMove.speed <= (slideThreshold - maxSpeedError))
            {
                playerMove.minSpeed = crouchSpeed;
                playerMove.maxSpeed = slideThreshold;
                slidePerformed = false;
                
            }
            
            
        }

    }
    public void crouch(InputAction.CallbackContext context)
    {
        //Changes stance state
        if (context.performed)
        {
            playerStances = stances.crouch;
            
        }
        if(context.canceled)
        {
            playerStances = stances.stand;      
        }
    }

    private bool canChangeStance()
    {
        return !(Physics.CheckSphere(stanceChecker.transform.position, characterController.radius - 0.1f, playerMask));
    }
    public bool isSlidePerformed()
    {
        return slidePerformed;
    }
}
