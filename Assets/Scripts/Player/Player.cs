using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float startingHorizontalSpeed = 120f;
    [SerializeField] float startingMaxHorizontalSpeed = 10f;
    [Space]

    [SerializeField] float startingVerticalSpeed = 120f;
    [SerializeField] float startingMaxVerticalSpeed = 7f;
    [Space]

    [SerializeField] float stunTime = 0.5f;
    [SerializeField] float invulnerabiltyTime = 1f;
    [SerializeField] int droppedGemsPerHit = 1;
    [Space]

    [SerializeField] int maxGemsInPouch = 5;
    [SerializeField] float knockBackReductionPerGemInPouch = 0.1f;
    [SerializeField] float horizontalMovementReductionPerGemInPouch = 0.1f;
    [SerializeField] float verticalMovementReductionPerGemInPouch = 0.1f;
    [Space]

    public float gemThrowForce = 50f;
    [SerializeField] float gemThrowCooldown = 1f;
    [SerializeField] GameObject throwGemPosition;
    [Space]

    [SerializeField] float scoreIncrementPerGemStored = 0.1f;
    public int score = 0;

    //Physics
    Rigidbody rb;
    Vector3 velocity, knockback;

    float horizontalSpeed;
    float maxHorizontalSpeed;

    float verticalSpeed;
    float maxVerticalSpeed;

    //Inputs
    Vector2 joystick = Vector2.zero;
    float throwGemInput = 0f;

    //States
    [HideInInspector] public bool climbingLadder = false;
    [HideInInspector] public bool isStunned = false;
    [HideInInspector] public bool isInvulnerable = false;

    //GemPouch
    [SerializeField] MeshRenderer pouchMeshRenderer;
    [SerializeField] MeshFilter pouchMeshFilter;
    [SerializeField] int maxPouchSize;
    [SerializeField] List<GemPouchTier> gemPouchTiers = new List<GemPouchTier>();
    public int currentPouchSize = 0;
    public GemPouchTier currentTier;

    //Gems
    Queue<Gem> gemPouch = new Queue<Gem>();
    bool gemThrowOnCooldown = false;

    //UI
    [HideInInspector]public int playerNumber = 0;
    GameUIManager gameUIManager;

    // Start is called before the first frame update
    void Start()
    {
        gameUIManager = FindObjectOfType<GameUIManager>();
        gameUIManager.ActivatePlayerUI(playerNumber);

        rb = gameObject.GetComponent<Rigidbody>();

        horizontalSpeed = startingHorizontalSpeed;
        verticalSpeed = startingVerticalSpeed;

        maxHorizontalSpeed = startingMaxHorizontalSpeed;
        maxVerticalSpeed = startingMaxVerticalSpeed;

        currentTier = gemPouchTiers[0];
        ChangePouchSize();
    }

    void Update()
    {
        //Debug.Log("X: " + joystick.x + "Y:" + joystick.y);
        //Debug.Log(gameObject.name + " Is Stunned: " + isStunned);
        //Debug.Log(gameObject.name + " Is Invulnerable: " + isInvulnerable);
        //Debug.Log(rb.velocity);
        //Debug.Log(gemPouch.Count);
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

    #region Input Management Methods
    public void MovementInput(InputAction.CallbackContext context)
    {
        joystick = context.ReadValue<Vector2>();
        RotatePlayer();
    }

    public void ThrowGemInput(InputAction.CallbackContext context)
    {
        if (!context.performed || !gameObject.scene.IsValid()) return;

        throwGemInput = context.ReadValue<float>();
        Debug.Log("ThrowInput: " + throwGemInput);

        if (!gemThrowOnCooldown && throwGemInput == 1)
        {
            Gem thrownGem = TryRemoveGemFromPouch();

            if(thrownGem != null)
            {
                gemThrowOnCooldown = true;
                StartCoroutine(GemThrowCooldown());
                //Animación de lanzar
                thrownGem.ThrowGem(transform.forward, throwGemPosition.transform.position, gemThrowForce, this);
            }
        }
    }
    #endregion

    #region Movement Methods
    Vector3 Movement()
    {
        float horizontalMovement = 0f;
        float verticalMovement = 0f;
        Vector3 finalMovement = Vector3.zero;

        horizontalMovement = Vector3.right.magnitude * joystick.x * horizontalSpeed * Time.deltaTime;

        finalMovement.x = horizontalMovement;

        //Desactivar gravedad
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

    private void UpdateSpeed()
    {
        horizontalSpeed = startingHorizontalSpeed - (startingHorizontalSpeed * horizontalMovementReductionPerGemInPouch * gemPouch.Count);
        verticalSpeed = startingVerticalSpeed - (startingVerticalSpeed * verticalMovementReductionPerGemInPouch * gemPouch.Count);

        maxHorizontalSpeed = startingMaxHorizontalSpeed - (startingMaxHorizontalSpeed * horizontalMovementReductionPerGemInPouch * gemPouch.Count);
        maxVerticalSpeed = startingMaxVerticalSpeed - (startingMaxVerticalSpeed * verticalMovementReductionPerGemInPouch * gemPouch.Count);
    }
    #endregion

    #region Knockback, Stun and Cooldowns methods
    public void Knockback(Vector3 knobackDirection, float knockbackForce)
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerStunned");
        isStunned = true;
        knockbackForce = knockbackForce - (knockbackForce * knockBackReductionPerGemInPouch * gemPouch.Count);
        knockback = knobackDirection * knockbackForce;
    }

    IEnumerator StunTime()
    {
        yield return new WaitForSecondsRealtime(stunTime);
        isStunned = false;
        CheckPouchFull();
    }

    IEnumerator InvulnerabilityTime()
    {
        yield return new WaitForSecondsRealtime(invulnerabiltyTime);
        isInvulnerable = false;
    }

    IEnumerator GemThrowCooldown()
    {
        yield return new WaitForSecondsRealtime(gemThrowCooldown);
        gemThrowOnCooldown = false;
    }
    #endregion

    #region GemPouch Methods
    public bool TryAddGemToPouch(Gem gem)
    {
        if (gemPouch.Count < maxGemsInPouch)
        {
            gemPouch.Enqueue(gem);

            currentPouchSize++;

            UpdateSpeed();
            CheckPouchFull();
            ChangePouchSize();

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
        gem.gameObject.SetActive(true);

        currentPouchSize--;
        if (currentPouchSize < 0) currentPouchSize = 0;

        UpdateSpeed();
        CheckPouchFull();
        ChangePouchSize();

        return gem;
    }    

    public void DropGem(Vector3 dropDirection)
    {
        for(int i = 0; i < droppedGemsPerHit; i++)
        {
            Gem droppedGem = TryRemoveGemFromPouch();
            if(droppedGem != null)
            {
                Debug.Log("Dropped gem");
                StartCoroutine(droppedGem.IgnoreCollisionsForSomeTime(gameObject.GetComponent<Collider>(), stunTime));
                droppedGem.transform.position = transform.position;
                droppedGem.DropForce(dropDirection, 3f);
            }
        }
    }

    private void CheckPouchFull()
    {
        if (gemPouch.Count == maxGemsInPouch)
            //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Gem"), true);
            gameObject.layer = LayerMask.NameToLayer("PlayerFull");

        else
            //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Gem"), false);
            gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void ChangePouchSize()
    {
        bool currentTierFound = false;

        GemPouchTier nextTier;

        if (currentPouchSize == 0)
            currentTier = gemPouchTiers[0];
        else
        {
            for (int i = 0; i < gemPouchTiers.Count; i++)
            {
                if (!currentTierFound)
                {
                    if (i == gemPouchTiers.Count - 1)
                    {
                        currentTierFound = true;
                    }

                    else
                    {
                        nextTier = gemPouchTiers[i + 1];
                        if (currentPouchSize >= nextTier.gemNumberThreshold)
                        {
                            currentTier = nextTier;
                        }
                    }
                }
            }
        }

        //Cambiar material del meshRenderer?
        pouchMeshFilter.mesh = currentTier.pouchMesh;
    }
    #endregion

    public void AddScore(int score)
    {
        this.score += score;
        gameUIManager.UpdatePlayerUI(playerNumber, this.score);
    }

    #region Trigger Methods
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ladder")
            climbingLadder = true;    

        if(other.tag == "Minecart")
        {
            if(gemPouch.Count != 0)
            {
                float scoreObtained = 0;
                float scoreMultiplier = 1f;
                int currentGems = gemPouch.Count;

                for(int i = 0; i < currentGems; i++)
                {
                    scoreObtained += gemPouch.Dequeue().value;
                    currentPouchSize--;
                    scoreMultiplier += scoreIncrementPerGemStored;
                }

                if (currentPouchSize < 0) currentPouchSize = 0;

                AddScore(Mathf.CeilToInt(scoreObtained * scoreMultiplier));

                UpdateSpeed();
                CheckPouchFull();
                ChangePouchSize();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ladder")
            climbingLadder = false;
    }
    #endregion
}

[System.Serializable]
public class GemPouchTier
{
    public Mesh pouchMesh;
    public int gemNumberThreshold;
}
