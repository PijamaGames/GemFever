using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float startingHorizontalSpeed = 120f;
    [SerializeField] float maxHorizontalSpeed = 30f;

    [SerializeField] float startingVerticalSpeed = 120f;
    [SerializeField] float maxVerticalSpeed = 30f;

    [SerializeField] float stunTime = 0.5f;
    [SerializeField] float invulnerabiltyTime = 1f;

    [SerializeField] int maxGemsInPouch = 5;
    [SerializeField] float knockBackReductionPerGemInPouch = 0.1f;
    [SerializeField] float horizontalMovementReductionPerGemInPouch = 0.1f;
    [SerializeField] float verticalMovementReductionPerGemInPouch = 0.1f;

    Rigidbody rb;
    Vector2 joystick = Vector2.zero;
    Vector3 velocity, knockback;

    [HideInInspector] public bool climbingLadder = false;
    [HideInInspector] public bool isStunned = false;
    [HideInInspector] public bool isInvulnerable = false;

    float horizontalSpeed;
    float verticalSpeed;

    public Queue<Gem> gemPouch = new Queue<Gem>();

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        horizontalSpeed = startingHorizontalSpeed;
        verticalSpeed = startingVerticalSpeed;
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
        }
        else
        {
            isInvulnerable = true;
            StartCoroutine(StunTime());
            StartCoroutine(InvulnerabilityTime());
        }

        //No conservar movimiento hacia arriba al salir de una escalera
        if (!climbingLadder && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }

        //Frenar al intentar subir una escalera (más cómodo para escalar)
        if(climbingLadder && joystick.x == 0 && !isStunned)
            rb.velocity = new Vector3(0f, rb.velocity.y, rb.velocity.z);

        if (isStunned && !climbingLadder)
            rb.useGravity = true;

        ClampVelocity();
        
        rb.AddForce(knockback, ForceMode.Impulse);
        
        knockback = Vector3.zero;
    }

    public void MovementInput(InputAction.CallbackContext context)
    {
        joystick = context.ReadValue<Vector2>();
        RotatePlayer();
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

    void ClampVelocity()
    {
        Vector3 clampedVelocity = Vector3.zero;

        //Clamp horizontal
        if (rb.velocity.x > maxHorizontalSpeed)
            clampedVelocity.x = maxHorizontalSpeed;

        else if (rb.velocity.x < -maxHorizontalSpeed)
            clampedVelocity.x = -maxHorizontalSpeed;

        else
            clampedVelocity.x = rb.velocity.x;

        if(climbingLadder)
        {
            if (rb.velocity.y > maxVerticalSpeed)
                clampedVelocity.y = maxVerticalSpeed;

            else if (rb.velocity.y < -maxVerticalSpeed)
                clampedVelocity.y = -maxVerticalSpeed;

            else if (joystick.y == 0)
                clampedVelocity.y = 0;
        }
        else
            clampedVelocity.y = rb.velocity.y;

        rb.velocity = clampedVelocity;
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
        isStunned = true;
        knockbackForce = knockbackForce - (knockbackForce * knockBackReductionPerGemInPouch * gemPouch.Count);
        knockback = knobackDirection * knockbackForce;
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

    public bool TryAddGemToPouch(Gem gem)
    {
        if (gemPouch.Count < maxGemsInPouch)
        {
            gemPouch.Enqueue(gem);

            UpdateSpeed();
            CheckPouchFull();

            return true;
        }
        else
            return false;
    }

    public Gem TryRemoveGemFromPouch()
    {
        if (gemPouch.Count <= 0) return null;

        //Sacar primera gema de la bolsa y devolverla
        Gem gem = gemPouch.Dequeue();

        UpdateSpeed();
        CheckPouchFull();

        return gem;
    }

    private void UpdateSpeed()
    {
        horizontalSpeed = startingHorizontalSpeed - (horizontalSpeed * horizontalMovementReductionPerGemInPouch * gemPouch.Count);
        verticalSpeed = startingVerticalSpeed - (verticalSpeed * verticalMovementReductionPerGemInPouch * gemPouch.Count);
    }

    private void CheckPouchFull()
    {
        if (gemPouch.Count == maxGemsInPouch)
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Gem"), true);
        else
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Gem"), false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ladder")
            climbingLadder = true;    
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ladder")
            climbingLadder = false;
    }
}
