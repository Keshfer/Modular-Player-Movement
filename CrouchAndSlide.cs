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
    public CharacterController characterCollider;
    public GameObject stanceChecker;
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
        playerStances = stances.stand;
        currentCamHeight = camStandHeight;
        characterCollider.height = colliderStand.height;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerStances == stances.stand)
        {
            if (canChangeStance())
            {
                currentCamHeight = Mathf.SmoothDamp(currentCamHeight, camStandHeight, ref stanceVelocity, stanceSmoothTime);
                playerCam.transform.localPosition = new Vector3(playerCam.transform.localPosition.x, currentCamHeight, playerCam.transform.localPosition.z);
                characterCollider.height = Mathf.SmoothDamp(characterCollider.height, colliderStand.height, ref stanceVelocity, stanceSmoothTime);
                characterCollider.center = Vector3.SmoothDamp(characterCollider.center, new Vector3(0, 0, 0), ref stancePositionVelocity, stanceSmoothTime);
            }
            
        } else if(playerStances == stances.crouch)
        {
            
            currentCamHeight = Mathf.SmoothDamp(currentCamHeight, camCrouchHeight, ref stanceVelocity, stanceSmoothTime);
            playerCam.transform.localPosition = new Vector3(playerCam.transform.localPosition.x, currentCamHeight, playerCam.transform.localPosition.z);
            characterCollider.height = Mathf.SmoothDamp(characterCollider.height, colliderCrouch.height, ref stanceVelocity, stanceSmoothTime);
            characterCollider.center = Vector3.SmoothDamp(characterCollider.center, new Vector3(0, colliderCrouch.center.y, 0), ref stancePositionVelocity, stanceSmoothTime);
            
        }
        
    }
    public void crouch(InputAction.CallbackContext context)
    {
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
        return !(Physics.CheckSphere(stanceChecker.transform.position, characterCollider.radius - 0.1f, playerMask));
    }
}
