using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    public Transform mainCamera;
    private Vector2 mouseDelta;
    private float rotX;
    private float rotY;
    
    [Range(0.01f, 10.00f)]
    public float sensitivity;
    // Start is called before the first frame update
    void Start()
    {
        
        rotX = transform.rotation.y;
        rotY = transform.rotation.x;
        

    }

    

    //Rotates camera based on mouse movement from "Camera" action
    public void look(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            mouseDelta = context.ReadValue<Vector2>();
            rotX += mouseDelta.x * sensitivity * Time.deltaTime * 10;
            rotY -= mouseDelta.y * sensitivity * Time.deltaTime * 10;
            rotY = Mathf.Clamp(rotY, -90, 90);
            //Replacing gameObject and mainCamera's angle values with new angle values
            gameObject.transform.localEulerAngles = new Vector3(transform.rotation.x, rotX, transform.rotation.z);
            mainCamera.transform.localEulerAngles = new Vector3(rotY, mainCamera.transform.rotation.y, mainCamera.transform.rotation.z);
            
        }

    }
    
}
