using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//TODO
/*
 * Hacer que stun solo bloquee movimiento horizontal (si te stunean en una escalera se empujan, pero no caes)
 * Hacer que la fuerza del knockback sea un stun y no un teleport
 */

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    Vector2 joystick = Vector2.zero;

    [SerializeField] float horizontalSpeed = 3f;
    [SerializeField] float maxHorizontalSpeed = 30f;

    [SerializeField] float verticalSpeed = 2f;
    [SerializeField] float maxVerticalSpeed = 30f;

    [SerializeField] float stunTime = 0.5f;
    [SerializeField] float invulnerabiltyTime = 1f;

    public bool climbingLadder = false;

    public bool isStunned { get; set; }
    public bool isInvulnerable { get; set; }

    Vector3 velocity, knockback;

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
        //Debug.Log("X: " + joystick.x + "Y:" + joystick.y);
        //Debug.Log(gameObject.name + " Is Stunned: " + isStunned);
        //Debug.Log(gameObject.name + " Is Invulnerable: " + isInvulnerable);
        //Debug.Log(rb.velocity);
    }

    //Movement Update
    void FixedUpdate()
    {
        velocity = new Vector3(0f, rb.velocity.y, 0f);

        if(!isStunned)
        {
            velocity = Movement();
            rb.AddForce(velocity, ForceMode.VelocityChange);

            velocity = ClampVelocity(velocity);
        }
        else
        {
            isInvulnerable = true;
            StartCoroutine(StunTime());
            StartCoroutine(InvulnerabilityTime());
        }

        if(climbingLadder)
        {
            rb.velocity = velocity;
        }
        else if (!climbingLadder && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector3(velocity.x, 0f, velocity.z);
        }
        else
        {
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }
        
        rb.AddForce(knockback, ForceMode.Impulse);

        knockback = Vector3.zero;
    }

    Vector3 Movement()
    {
        float horizontalMovement = 0f;
        float verticalMovement = 0f;
        Vector3 finalMovement = Vector3.zero;

        horizontalMovement = Vector3.right.magnitude * joystick.x * horizontalSpeed * Time.deltaTime;

        finalMovement.x = horizontalMovement;

        //Desasctivar gravedad
        if (climbingLadder)
        {
            rb.useGravity = false;

            verticalMovement = Vector3.up.magnitude * joystick.y * verticalSpeed * Time.deltaTime;

            finalMovement.y = verticalMovement;
        }
        else
            rb.useGravity = true;

        return finalMovement;
    }

    Vector3 ClampVelocity(Vector3 velocityToClamp)
    {
        if (joystick.x != 0)
            velocityToClamp.x = Mathf.Clamp(velocityToClamp.x, -maxHorizontalSpeed, maxHorizontalSpeed);
        else
            velocityToClamp.x = 0f;

        if (climbingLadder)
        {
            if (joystick.y != 0)
                velocityToClamp.y = Mathf.Clamp(velocityToClamp.y, -maxVerticalSpeed, maxVerticalSpeed);
            else
                velocityToClamp.y = 0f;
        }
        else if(velocityToClamp.y > 0f) 
        {
            velocityToClamp.y = 0f;
        }

        velocityToClamp.z = 0f;

        return velocityToClamp;
    }

    void RotatePlayer()
    {
        //Izquierda
        if (joystick.x < 0f)
        {
            transform.forward = -Vector3.right;
        }
        //Derecha
        else if (joystick.x > 0f)
        {
            transform.forward = Vector3.right;
        }
    }

    public void Knockback(Vector3 knobackDirection, float knockbackForce)
    {
        Debug.Log("Knockback");
        isStunned = true;
        knockback = knobackDirection * knockbackForce;
        //rb.AddForce(knobackDirection * knockbackForce, ForceMode.Acceleration);
    }

    IEnumerator StunTime()
    {
        yield return new WaitForSecondsRealtime(stunTime);
        isStunned = false;
    }

    IEnumerator InvulnerabilityTime()
    {
        yield return new WaitForSecondsRealtime(invulnerabiltyTime);
        isInvulnerable = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ladder")
        {
            climbingLadder = true;
            //Vector3 aux = rb.velocity;
            //rb.velocity = new Vector3(0f, aux.y, 0f);
        }          
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ladder")
            climbingLadder = false;
    }
}
