using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController)), DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{
    public float speed = 7.5f;
    public float gravity = 20.0f;
    [Space]
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    Transform cam;
    CharacterController charController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;


    void Awake()
    {
        cam = FindObjectOfType<Camera>().transform;
        charController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnEnable()
    {
        // Reset rotation to cameras current after being disabled
        rotationX = cam.rotation.eulerAngles.x;
        // Negitive rotations loop to 360
        if (rotationX > 180)
        { rotationX -= 360; }
    }


    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        moveDirection.y = movementDirectionY;

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!charController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        charController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove && !PauseMenuScript.paused)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            cam.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.localRotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }



        //used to change speed for testing
        if (Input.GetKeyDown(KeyCode.Equals))
        { speed += 5; }
        if (Input.GetKeyDown(KeyCode.Minus))
        { speed -= 5; }
    }
}