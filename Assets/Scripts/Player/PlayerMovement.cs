using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    Vector2 joystick = Vector2.zero;
    [SerializeField] float speed = 3f;
    [SerializeField] float maxSpeedHorizontal = 30f;
    [SerializeField] float maxSpeedVertical = 60f;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void MovementInput(InputAction.CallbackContext context)
    {
        joystick = context.ReadValue<Vector2>();
        Debug.Log(joystick);
    }

    void Update()
    {
        
    }

    //Movement Update
    private void FixedUpdate()
    {
        HorizontalMovement();
        RotatePlayer();
        ClampVelocity();
    }

    private void HorizontalMovement()
    {
        float horizontalMovement = Vector3.right.magnitude * joystick.x * speed * Time.deltaTime;
        float verticalMovement = 0f;

        Vector3 finalMovement = new Vector3(horizontalMovement, verticalMovement, 0f);
        rb.AddForce(finalMovement, ForceMode.VelocityChange);
    }

    private void RotatePlayer()
    {
        //Eeeeeeew
        if(joystick.x < 0f)
        {
            transform.RotateAround(transform.position, Vector3.up, 180f);
        }
        else if (joystick.x > 0f)
        {
            transform.RotateAround(transform.position, Vector3.up, 180f);
        }
    }

    private void ClampVelocity()
    {
        Vector3 clampedVelocity = rb.velocity;

        if (joystick.x != 0)
            clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -maxSpeedHorizontal, maxSpeedHorizontal);
        else
            clampedVelocity.x = 0f;

        clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, -maxSpeedVertical, maxSpeedVertical);
        clampedVelocity.z = 0f;

        rb.velocity = clampedVelocity;

        //Debug.Log(rb.velocity);
    }
}
