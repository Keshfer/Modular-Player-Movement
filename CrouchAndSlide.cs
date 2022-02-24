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
    public CharacterController characterController;
    public GameObject stanceChecker;
    private PlayerMove playerMove;
    public float crouchSpeed;
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
        playerMove = GetComponent<PlayerMove>();
        playerStances = stances.stand;
        currentCamHeight = camStandHeight;
        characterController.height = colliderStand.height;
        standSpeed = playerMove.minSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerStances == stances.stand)
        {
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
            
        } else if(playerStances == stances.crouch)
        {
            
            currentCamHeight = Mathf.SmoothDamp(currentCamHeight, camCrouchHeight, ref stanceVelocity, stanceSmoothTime);
            playerCam.transform.localPosition = new Vector3(playerCam.transform.localPosition.x, currentCamHeight, playerCam.transform.localPosition.z);
            characterController.height = Mathf.SmoothDamp(characterController.height, colliderCrouch.height, ref stanceVelocity, stanceSmoothTime);
            characterController.center = Vector3.SmoothDamp(characterController.center, new Vector3(0, colliderCrouch.center.y, 0), ref stancePositionVelocity, stanceSmoothTime);
            playerMove.minSpeed = crouchSpeed;
            
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
}
