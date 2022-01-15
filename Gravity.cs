using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private CharacterController characterController;
    public Vector3 gravity;
    private GameObject groundChecker;
    private bool isGround;
    public LayerMask ground;
    private int airTime = 0;
    //ALWAYS HAVE THE GROUNDCHECKER AS THE FIRST CHILD ************************************
    
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        groundChecker = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        isGround = Physics.CheckSphere(groundChecker.transform.position, 0.5f, ground);
        if(!isGround)
        {
            airTime++;
            gravity = new Vector3(0, Physics.gravity.y * (airTime / 60), 0);
            //print(gravity);

        } else
        {
            airTime = 0;
        }
        
        characterController.Move(gravity * Time.deltaTime);
    }
}
