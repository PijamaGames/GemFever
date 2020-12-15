using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    Vector2 joystick = Vector2.zero;

    [SerializeField] float horizontalSpeed = 3f;
    [SerializeField] float maxHorizontalSpeed = 30f;

    [SerializeField] float verticalSpeed = 2f;
    [SerializeField] float maxVerticalSpeed = 30f;

    public bool climbingLadder = false;

    // Start is called before the first frame update
    void Start()
    {
        
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void MovementInput(InputAction.CallbackContext context)
    {
        joystick = context.ReadValue<Vector2>();
        RotatePlayer();
    }

    void Update()
    {
        Debug.Log("X: " + joystick.x + "Y:" + joystick.y);
    }

    //Movement Update
    private void FixedUpdate()
    {
        Movement();
        ClampVelocity();
    }

    private void Movement()
    {
        float horizontalMovement = 0f;
        float verticalMovement = 0f;
        Vector3 finalMovement = Vector3.zero;

        horizontalMovement = Vector3.right.magnitude * joystick.x * horizontalSpeed * Time.deltaTime;

        finalMovement.x = horizontalMovement;

        //Desasctivar gravedad
        if (climbingLadder)
        {
            Debug.Log("Trepando");

            rb.useGravity = false;

            verticalMovement = Vector3.up.magnitude * joystick.y * verticalSpeed * Time.deltaTime;

            finalMovement.y = verticalMovement;
        }
        else
            rb.useGravity = true;

        rb.AddForce(finalMovement, ForceMode.VelocityChange);
    }

    private void RotatePlayer()
    {
        //Izquierda
        if(joystick.x < 0f)
        {
            transform.forward = -Vector3.right;
        }
        //Derecha
        else if (joystick.x > 0f)
        {
            transform.forward = Vector3.right;
        }
    }

    private void ClampVelocity()
    {
        Vector3 clampedVelocity = rb.velocity;

        if (joystick.x != 0)
            clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -maxHorizontalSpeed, maxHorizontalSpeed);
        else
            clampedVelocity.x = 0f;

        if (climbingLadder)
        {
            if (joystick.y != 0)
                clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, -maxVerticalSpeed, maxVerticalSpeed);
            else
                clampedVelocity.y = 0f;
        }
        else if(clampedVelocity.y > 0f) 
        {
            clampedVelocity.y = 0f;
        }

        clampedVelocity.z = 0f;

        rb.velocity = clampedVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ladder")
        {
            climbingLadder = true;
            //Vector3 aux = rb.velocity;
            //rb.velocity = new Vector3(0f, aux.y, 0f);
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ladder")
            climbingLadder = false;
    }
}
