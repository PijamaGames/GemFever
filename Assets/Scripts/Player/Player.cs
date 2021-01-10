using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerAvatar avatar;
    public UserInfo userInfo;

    public bool initialized = false;

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
    public Vector2 joystick = Vector2.zero;
    float throwGemInput = 0f;
    public AndroidInputs androidInputs;
    public bool promptInput = false;

    //States
    public bool climbingLadder = false;
    public bool pickaxeOnLadder = false;
    public bool ladderTopReached = false;
    public bool isWalking = false;
    public bool isInLadder = false;
    public bool isStunned = false;
    public bool isInvulnerable = false;

    //GemPouch
    [SerializeField] MeshRenderer pouchMeshRenderer;
    [SerializeField] MeshFilter pouchMeshFilter;
    [SerializeField] int maxPouchSize;
    [SerializeField] List<GemPouchTier> gemPouchTiers = new List<GemPouchTier>();
    public int currentPouchSize = 0;
    public GemPouchTier currentTier;
    GemPool gemPool;

    //Gems
    Queue<Gem> gemPouch = new Queue<Gem>();
    bool gemThrowOnCooldown = false;

    //UI
    [HideInInspector]public int playerNumber = 0;
    GameUIManager gameUIManager;

    //Animator
    [SerializeField] Animator animator;
    [SerializeField] RuntimeAnimatorController hostAnimator;
    [SerializeField] RuntimeAnimatorController clientAnimator;
    Vector3 groundMeshOrientation = Vector3.zero;
    [SerializeField] GameObject playerMesh;
    bool freeze = false;
    bool climbingAnimation = false;
    bool rotateAnimation = true;

    //Sound
    PersistentAudioSource audioSource;
    [SerializeField] AudioClip walkSound;
    [SerializeField] AudioClip ladderSound;
    [SerializeField] AudioClip gemThrowSound;

    //Network
    [HideInInspector] public NetworkPlayer networkPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        avatar = GetComponent<PlayerAvatar>();

        //Si es el jugador local
        //if(GameManager.isLocalGame || GameManager.isHost)
        androidInputs = FindObjectOfType<AndroidInputs>();

        networkPlayer = GetComponent<NetworkPlayer>();

        gameUIManager = FindObjectOfType<GameUIManager>();

        gemPool = FindObjectOfType<GemPool>();

        audioSource = FindObjectOfType<PersistentAudioSource>();

        if(!PlayerSpawnerManager.isInHub)
            gameUIManager.ActivatePlayerUI(playerNumber, userInfo.id);

        rb = gameObject.GetComponent<Rigidbody>();

        horizontalSpeed = startingHorizontalSpeed;
        verticalSpeed = startingVerticalSpeed;

        maxHorizontalSpeed = startingMaxHorizontalSpeed;
        maxVerticalSpeed = startingMaxVerticalSpeed;

        currentTier = gemPouchTiers[0];
        ChangePouchSize();

        if (!GameManager.isLocalGame)
        {
            if (GameManager.isHost) animator.runtimeAnimatorController = hostAnimator;
            else if (GameManager.isClient) animator.runtimeAnimatorController = clientAnimator;
        }

        groundMeshOrientation = playerMesh.transform.right;
    }

    void Update()
    {
        if (!PlayerSpawnerManager.isInHub && gameUIManager == null)
        {
            gameUIManager = FindObjectOfType<GameUIManager>();
            if (gameUIManager != null)
            {
                if(!GameManager.isLocalGame)
                    gameUIManager.ActivatePlayerUI(playerNumber, userInfo.id);
                else
                {
                    if(GameManager.english)
                        gameUIManager.ActivatePlayerUI(playerNumber, "P");
                    else
                        gameUIManager.ActivatePlayerUI(playerNumber, "J");
                }
                    
            }
        }

        //Mobile
        MobileInputs();

        //PC
        //Máquina del host, pero jugadores clientes
        if (!GameManager.isLocalGame)
        {
            if (GameManager.isHost && userInfo.isClient)
            {
                //Recibir input por red
                joystick = networkPlayer.inputInfo.joystick;
                throwGemInput = networkPlayer.inputInfo.throwGemInput;
                animator.speed = networkPlayer.info.animationSpeed;
                ThrowGem();
            }
            else if(GameManager.isClient && networkPlayer.info != null)
            {
                animator.speed = networkPlayer.info.animationSpeed;
                score = networkPlayer.info.score;

                if (gameUIManager == null)
                {
                    gameUIManager = FindObjectOfType<GameUIManager>();
                    if (gameUIManager != null)
                    {
                        gameUIManager.UpdatePlayerUI(playerNumber, this.score, userInfo.id);
                    }
                }
                else
                    gameUIManager.UpdatePlayerUI(playerNumber, this.score, userInfo.id);

                currentPouchSize = networkPlayer.info.gems;
                ChangePouchSize();
                playerMesh.transform.rotation = Quaternion.Euler(networkPlayer.info.rotation.x, networkPlayer.info.rotation.y, networkPlayer.info.rotation.z);
            }
        }
    }

    private void MobileInputs()
    {
        if (GameManager.isHandheld)
        {
            //Online game
            if (!GameManager.isLocalGame)
            {
                //Caso de las máquinas del host
                if (!GameManager.isHost)
                {
                    //Manda input por red
                    networkPlayer.inputInfo.joystick = androidInputs.GetMovementInput();
                    networkPlayer.inputInfo.throwGemInput = androidInputs.GetThrowGemInput();
                }

                else //Máquina host y jugador host
                {
                    joystick = androidInputs.GetMovementInput();
                    throwGemInput = androidInputs.GetThrowGemInput();
                    ThrowGem();
                }
            }
            //Local game
            else
            {
                joystick = androidInputs.GetMovementInput();
                throwGemInput = androidInputs.GetThrowGemInput();
            }
        }
    }

    public UserInfo GetUserInfo()
    {
        return userInfo;
    }

    public void SetUserInfo(UserInfo info)
    {
        userInfo = info;
        ClientInRoom.players.Add(info.id, this);
        if (avatar == null) avatar = GetComponent<PlayerAvatar>();
        avatar.SetUserInfo(userInfo);
    }

    #region Input Management Methods

    public void MovementInput(InputAction.CallbackContext context)
    {
        if (GameManager.isHandheld) return;

        if (freeze) return;

        //Online game
        if (!GameManager.isLocalGame)
        {
            //Caso de las máquinas del host
            if (!GameManager.isHost)
            {
                //Manda input por red
                networkPlayer.inputInfo.joystick = context.ReadValue<Vector2>();
            }

            else //Máquina host y jugador host
            {
                joystick = context.ReadValue<Vector2>();
            }
        }
        //Local game
        else
        {
            joystick = context.ReadValue<Vector2>();
        }
    }

    public void ThrowGemInput(InputAction.CallbackContext context)
    {
        if (GameManager.isHandheld) return;

        if (freeze) return;

        if (!context.performed || !gameObject.scene.IsValid()) return;

        //Online game
        if (!GameManager.isLocalGame)
        {
            //Caso de las máquinas del host
            if (!GameManager.isHost)
            {
                //Manda input por red
                networkPlayer.inputInfo.throwGemInput = context.ReadValue<float>();
            }

            else //Máquina host y jugador host
            {
                throwGemInput = context.ReadValue<float>();

                ThrowGem();
            }
        }
        //Offline game
        else
        {
            throwGemInput = context.ReadValue<float>();

            ThrowGem();
        }
    }

    public void PromptInput(InputAction.CallbackContext context)
    {
        if (GameManager.isHandheld) return;

        //if (!context.performed || !gameObject.scene.IsValid()) return;

        promptInput = context.ReadValue<float>() == 1;
    }

    private void ThrowGem()
    {
        if (!gemThrowOnCooldown && throwGemInput == 1 && !climbingLadder)
        {
            androidInputs.ResetGemThrowInput();

            Gem thrownGem = TryRemoveGemFromPouch();

            if (thrownGem != null)
            {
                gemThrowOnCooldown = true;
                StartCoroutine(GemThrowCooldown());

                PlaySound(gemThrowSound);

                StartGemAnimation();

                Vector3 direction = Vector3.ProjectOnPlane(playerMesh.transform.forward, Vector3.up);

                thrownGem.ThrowGem(/*playerMesh.transform.forward*/ direction, throwGemPosition.transform.position, gemThrowForce, this);
            }
        }
    }

    #endregion

    //Movement Update
    void FixedUpdate()
    {
        if(rotateAnimation)
            RotatePlayer();

        velocity = new Vector3(0f, rb.velocity.y, 0f);

        if (!isStunned)
        {
            velocity = Movement();

            //Idle or Walk
            isWalking = velocity.x != 0;
            WalkOrIdleOrClimb();          

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

        if(!climbingLadder)
            animator.speed = 1f;

        if (GameManager.isHandheld)
            ThrowGem();
    }

    #region Movement Methods
    Vector3 Movement()
    {
        float horizontalMovement = 0f;
        float verticalMovement = 0f;
        Vector3 finalMovement = Vector3.zero;

        horizontalMovement = Vector3.right.magnitude * joystick.x * horizontalSpeed * Time.deltaTime;

        finalMovement.x = horizontalMovement;

        if (climbingLadder)
        {
            if(animator.GetBool("Idle_Climb") && !animator.GetBool("Climb_MineStair") && !animator.GetBool("Stun"))
            {
                if (joystick.y == 0)
                {
                    pickaxeOnLadder = false;
                    animator.speed = 0f;
                }
                else
                {
                    pickaxeOnLadder = true;
                    animator.speed = 1f;
                }

                if (GameManager.isHost)
                {
                    //Manda input por red
                    if (joystick.y == 0) networkPlayer.info.animationSpeed = 0f;
                    else networkPlayer.info.animationSpeed = 1f;
                }
            }
            else
            {
                animator.speed = 1f;
                if (GameManager.isHost)
                {
                    //Manda input por red
                    networkPlayer.info.animationSpeed = 1f;
                }
            }
                

            rb.useGravity = false;

            if(ladderTopReached)
            {
                if (joystick.y == 0)
                {
                    pickaxeOnLadder = false;
                }
                else
                {
                    pickaxeOnLadder = true;
                }
                if (joystick.y < 0)
                    verticalMovement = Vector3.up.magnitude * joystick.y * verticalSpeed * Time.deltaTime;
            }
            else
            {
                if (joystick.y == 0)
                {
                    pickaxeOnLadder = false;
                }
                else
                {
                    pickaxeOnLadder = true;
                }
                verticalMovement = Vector3.up.magnitude * joystick.y * verticalSpeed * Time.deltaTime;
            }
                
            
            if (!ladderTopReached && (joystick.y != 0 /*|| !touchingTheGround*/))
            {
                playerMesh.transform.forward = Vector3.forward;
                if (GameManager.isHost)
                {
                    //Manda input por red
                    networkPlayer.info.rotation = playerMesh.transform.rotation.eulerAngles;
                }
                climbingAnimation = true;
                rotateAnimation = false;
            }
            else
            {
                climbingAnimation = false;
            }

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
            groundMeshOrientation = -Vector3.right;
        }
        //Derecha
        else if (joystick.x > 0f)
        {
            transform.forward = Vector3.right;
            groundMeshOrientation = Vector3.right;
        }

        if (GameManager.isHost)
        {
            //Manda input por red
            networkPlayer.info.rotation = playerMesh.transform.rotation.eulerAngles;
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

        animator.speed = 1f;
        if (GameManager.isHost)
        {
            //Manda input por red
            networkPlayer.info.animationSpeed = 1f;
        }
        animator.SetBool("Stun", true);
        

        if(!climbingLadder)
        {
            knockbackForce = knockbackForce - (knockbackForce * knockBackReductionPerGemInPouch * gemPouch.Count);
            knockback = knobackDirection * knockbackForce;
        }
    }

    IEnumerator StunTime()
    {
        yield return new WaitForSecondsRealtime(stunTime);
        isStunned = false;
        animator.SetBool("Stun", false);
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

        if(!GameManager.isLocalGame)
        {
            if (GameManager.isHost)
            {
                //Manda input por red
                networkPlayer.info.score = score;
            }
        }

        if (!PlayerSpawnerManager.isInHub)
        {
            if(gameUIManager == null)
            {
                gameUIManager = FindObjectOfType<GameUIManager>();
                if(gameUIManager != null)
                {
                    gameUIManager.ActivatePlayerUI(playerNumber, userInfo.id);
                    gameUIManager.UpdatePlayerUI(playerNumber, this.score, userInfo.id);
                }
            }
            else
                gameUIManager.UpdatePlayerUI(playerNumber, this.score, userInfo.id);
        }
            
    }

    #region Trigger Methods
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LadderTop")
        {
            ladderTopReached = true;
        }

        if (other.tag == "Ladder")
        {
            climbingLadder = true;
        }

        if(other.tag == "Minecart")
        {
            if(gemPouch.Count != 0)
            {
                float scoreObtained = 0;
                float scoreMultiplier = 1f;
                int currentGems = gemPouch.Count;

                for(int i = 0; i < currentGems; i++)
                {
                    Gem gem = gemPouch.Dequeue();

                    scoreObtained += gem.value;
                    currentPouchSize--;
                    scoreMultiplier += scoreIncrementPerGemStored;

                    if (gemPool != null)
                        gemPool.ReturnObjectToPool(gem.gameObject);
                }

                if (currentPouchSize < 0) currentPouchSize = 0;

                AddScore(Mathf.CeilToInt(scoreObtained * scoreMultiplier));

                UpdateSpeed();
                CheckPouchFull();
                ChangePouchSize();
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if(clip != null)
            audioSource.PlayEffect(clip);
    }
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "LadderTop")
        {
            pickaxeOnLadder = false;
            //climbingLadder = false;
            ladderTopReached = false;
            animator.speed = 1f;
            if (GameManager.isHost)
            {
                //Manda input por red
                networkPlayer.info.animationSpeed = 1f;
            }
        }

        if (other.tag == "Ladder")
        {
            rotateAnimation = true;

            climbingLadder = false;
            pickaxeOnLadder = false;
            
            ladderTopReached = false;

            animator.speed = 1f;
            if (GameManager.isHost)
            {
                //Manda input por red
                networkPlayer.info.animationSpeed = 1f;
            }
            animator.SetBool("Idle_Climb", false);

            playerMesh.transform.forward = groundMeshOrientation;
            //CheckPouchFull();
        }
            
    }
    #endregion

    public void PlayWalkSound()
    {
        PlaySound(walkSound);
    }

    public void PlayLadderSound()
    {
        PlaySound(ladderSound);
    }

    #region Animations
    private void WalkOrIdleOrClimb()
    {
        if (!GameManager.isLocalGame && !GameManager.isHost) return;

        if (climbingLadder && climbingAnimation && !ladderTopReached)
        {
            //PlaySound(ladderSound);
            animator.SetBool("Idle_Climb", climbingLadder);
        }
        else
        {
            //PlaySound(walkSound);
            animator.SetBool("Idle_Walk", isWalking);
        }
    }

    public void StartPickaxeAnimation()
    {
        if (!GameManager.isLocalGame && !GameManager.isHost) return;

        //Trepando escalera
        if (/*climbingLadder && pickaxeOnLadder*/ animator.GetBool("Idle_Climb")/*&& false*/)
        {
            animator.SetBool("Climb_MineStair", true);
        }

        //Quieto o andando
        else
        {
            animator.SetBool("Idle_Mine", true);
        }
    }

    public void EndPickaxeAnimation()
    {
        if (!GameManager.isLocalGame && !GameManager.isHost) return;

        if (animator.GetBool("Climb_MineStair"))
            animator.SetBool("Climb_MineStair", false);

        if (animator.GetBool("Idle_Mine"))
            animator.SetBool("Idle_Mine", false);
    }

    public void StartGemAnimation()
    {
        if (!GameManager.isLocalGame && !GameManager.isHost) return;

        animator.SetBool("Idle_Throw", true);
    }

    public void EndGemAnimation()
    {
        if (!GameManager.isLocalGame && !GameManager.isHost) return;

        animator.SetBool("Idle_Throw", false);
    }

    public void PlayVictoryAnimation(int position)
    {
        Freeze();

        if (!GameManager.isLocalGame && !GameManager.isHost) return;

        if (position == 0) PlayFirstPositionAnim();
        else if (position == 1 || position == 2) PlaySecondOrThirdPositionAnim();
        else PlayFourthPositionAnim();

    }

    void PlayFirstPositionAnim()
    {
        animator.SetBool("Victory1", true);
    }

    void PlaySecondOrThirdPositionAnim()
    {
        animator.SetBool("Victory2_3", true);
    }

    void PlayFourthPositionAnim()
    {
        animator.SetBool("Victory4", true);
    }

    void ResetAnimations()
    {
        animator.SetBool("Idle_Walk", false);
        animator.SetBool("Idle_Climb", false);
        animator.SetBool("Idle_Mine", false);
        animator.SetBool("Idle_Throw", false);

        animator.SetBool("Stun", false);
        animator.SetBool("ClimbMineStair", false);

        animator.SetBool("Victory1", false);
        animator.SetBool("Victory2_3", false);
        animator.SetBool("Victory4", false);

        animator.Play("Idle");

        climbingLadder = false;
        pickaxeOnLadder = false;
        ladderTopReached = false;
        isWalking = false;
        isInLadder = false;
        isStunned = false;
        isInvulnerable = false;

        GetComponentInChildren<Pickaxe>().ResetPickaxe();

        //Online game
        if (!GameManager.isLocalGame)
        {
            //Caso de las máquinas del host
            if (GameManager.isHost)
            {
                //Manda input por red
                playerMesh.transform.forward = -Vector3.forward;
                networkPlayer.info.rotation = playerMesh.transform.rotation.eulerAngles;
            }
            else
            {
                playerMesh.transform.rotation = Quaternion.Euler(networkPlayer.info.rotation.x, networkPlayer.info.rotation.y, networkPlayer.info.rotation.z);
            }
        }
        //Local game
        else
        {
            playerMesh.transform.forward = -Vector3.forward;
        }         
    }
    #endregion

    public void Reset()
    {
        currentPouchSize = 0;
        promptInput = false;
        score = 0;
        ResetAnimations();
    }

    public void Freeze()
    {
        ResetAnimations();

        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;

        freeze = true;
    }
}

[System.Serializable]
public class GemPouchTier
{
    public Mesh pouchMesh;
    public int gemNumberThreshold;
}
