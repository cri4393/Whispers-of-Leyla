using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovements : MonoBehaviour
{
    #region Animations
    [Header("Animatons")]
    private Animator anim;
    private string currentState;
    const string CROUCH = "crouch";
    const string FALLING = "FallingAfterTower";
    const string SLEEP = "Sleep";
    const string AWAKE = "Awake";
    const string RUN_LIMPS = "Limps";
    const string IDLE_LIMPS = "LimpsIdle";
    const string JUMP_LIMPS = "LimpsJump";
    const string IDLE = "Iris_Idle";
    const string RUN = "Iris_Run";
    const string JUMP = "Iris_Jump";
    const string JUMP_MOVING = "Iris_JumpMoving";
    const string JUMP_MOVINGDOWN = "Iris_JumpMovingDown";
    const string JUMP_FALLING = "Iris_Falling";
    const string FALLING_MAXSPEED = "FallMaxSpeed";
    const string SLIDING = "Iris_WallSliding";
    const string WALLJUMP = "Iris_WallJump";
    const string DASH = "Iris_Dash";
    #endregion
    
    #region AudioFootSteps
    [Header("Footsteps")]
    public List<AudioClip> grassFS;
    public List<AudioClip> rockFS;
    public List<AudioClip> woodFS;
    public List<AudioClip> marbleFS;
    public List<AudioClip> mudFS;

    [SerializeField] private AudioSource footstepSource;
    #endregion

    #region Variables Movement
    [Header("MOVEMENTS")]
    public float move = 10f;
    public float moveSpeedIncrease = 0f;
    public float horizontal;
    public bool facingRight = true;
    public bool canFlip = true;
    public bool inBorder;
    public bool canMove = true;
    private float fallSpeedYDampingChangeThreshold;
    public Transform inBorderStart;
    public Vector2 inBorderSize;
    #endregion

    #region Variables Jump
    [Header("JUMP")]
    public float jumpForce = 100f;
    public LayerMask groundLayer;
    public bool grounded;

    //NEW
    public bool isJumpCut;
    public bool isJumpFalling;
    public bool isFallingAtMaxSpeed;
    public bool IsJumping;
    public bool IsWallJumping;
    public bool IsSliding;
    public bool IsAlreadySliding;
    public bool preventForceSlide;
    public float LastPressedJumpTime;
    public bool canJump;

    private int lastWallJumpDir;
    private float wallJumpStartTime;
    #endregion

    #region Variables WallSliding
    [Header("WALL SLIDING")]
    public float wallSlideSpeed = 0;
    public Transform wallCheckPoint;
    public Vector2 wallCheckSize;
    public LayerMask wallslideLayer;
    public bool isWallSliding;
    public bool canSliding = true;
    private bool canWallJumping = true;
    #endregion

    #region Variables Dash
    [Header("DASH")]
    public float dashingPower = 24f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;
    public float dashStunRadius;
    [SerializeField] private LayerMask enemyLayer;
    private bool canDash = true;
    public bool haveStunDash = false;
    [HideInInspector] public bool isDashing;
    private bool isDashingCooldown = false;
    public bool haveDash = true;
    public GameObject dashParticleRight;
    public GameObject dashParticleLeft;
    #endregion

    #region Timers
    [Header("Timers")]
    public float MaxFallSpeedCounter;
    public float LastOnGroundTime;
    public float LastOnWallTime;
    public float LastOnWallRightTime;
    public float LastOnWallLeftTime;
    #endregion

    #region Checks
    [Header("Checks")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    [Space(5)]
    [SerializeField] private Transform frontWallCheckPoint;
    [SerializeField] private Transform backWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(.5f, 1f);
    #endregion

    [Header("OTHERS")]
    public PlayerData Data;
    private Rigidbody2D rb;
    public static PlayerInput input;
    public static PlayerInput PlayerInput;
    private BoxCollider2D box;

    public bool isLimps = false;
    public bool canPressOnlyJump = false;

    void Awake()
    {
        //instance = this;

        input = GetComponent<PlayerInput>();
        PlayerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();

        Cursor.visible = false;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        // playerShield = GetComponent<PlayerShield>();
        // playerLife = GetComponent<PlayerLife>();
        // knockbackScript = GetComponent<KnockbackScript>();
        // impulseSource = GetComponent<CinemachineImpulseSource>();

        //fallSpeedYDampingChangeThreshold = CameraManager.instance.fallSpeedYDampingChangeThreshold;

        Data.runMaxSpeed = 13;

        // if (SceneManager.GetActiveScene().name == "N1Nightmare")
        // {
        //     ActivePlayerControls();
        // }
        //     if (!DataPersistenceManager.instance.disableDataPersistence)
        //     {
        //         DeactivePlayerControls();
        //         if(SceneManager.GetActiveScene().name == "L1IrisRuins" && alreadyLimps)
        //         {
        //             StartCoroutine(ActiveControls());
        //             //Debug.Log("TEST");
        //         }
        //     }

        // if (SceneManager.GetActiveScene().name == "T1Tower" && isRock)
        // {
        //     ManualFlip();
        //     isDoingSomething = true;
        //     ChangeAnimationState(CROUCH);
        // }
        // if (SceneManager.GetActiveScene().name == "T12Tower")
        // {
        //     DeactivePlayerControls();
        //     ChangeAnimationState(FALLING);
        // }
        // if (SceneManager.GetActiveScene().name == "L1IrisRuins" && !alreadyLimps)
        // {
        //     DeactivePlayerControls();
        //     FindObjectOfType<AudioManager>().StopPlaying("Tower");
        //     isSleepAnim = true;
        //     canJump = false;
        //     ChangeAnimationState(SLEEP);
        //     StartCoroutine(AwakeIris());
        // }
    }

    void Update()
    {
        //if (isDoingSomething || isDashing) return;
        /*
        if (isDashing || isSmashing || NewPlayerCombat.instance.isKatana || NewPlayerCombat.instance.isSword)
            return;
        */

        // if (buttonDownPressed)
        // {
        //     if (currentOneWayPlatform != null)
        //     {
        //         StartCoroutine(DisableCollision());
        //     }
        // }

        CheckWorld();
        ResetDash();
        //LerpingY();

        Timers();
        CheckForMaxFallSpeed();
        Gravity();

        CollisionsChecks();
        JumpChecks();
        SlideChecks();

        
        /*
        if (isFallingAtMaxSpeed)
        {
            if(LastOnGroundTime > 0)
            {
                StartCoroutine(FallingAtMaxSpeed());
            }
        }
        */
        
        if (LastOnGroundTime > 0 || LastOnWallTime < 0)
             IsAlreadySliding = false;

        if (IsAlreadySliding)
        {
            IsSliding = true;
            Slide();
        }
        else
            SlideChecks();
        // if(PauseManager.paused) return;
        // pad = Gamepad.current;
    }

    void FixedUpdate()
    {
        // if (PlayerLife.instance.isHealing || isDoingSomething || !NewPlayerCombat.instance.isAttackingH) return;
        // if (NewDialogueManager.GetInstance().dialogueIsPlaying)
        // {
        //     rb.velocity = new Vector3(0, rb.velocity.y, 0);
        //     if (isLimps)
        //         ChangeAnimationState(IDLE_LIMPS);
        //     else 
        //     {
        //         ChangeAnimationState(IDLE);
        //         NewPlayerCombat.instance.isAttacking = false;
        //     }

        //     return;
        // }

        if (isDashing)
        {
             return;
        }
        Flip();

        // if(NewPlayerCombat.instance.attackUp || NewPlayerCombat.instance.attackDown) return;
        
        // if (isSit) return;
        if (isLimps)
            Inputs();
        else if (canPressOnlyJump)
            RunControlledJump();
        else if (!IsWallJumping /* && !playerLife.knockback  && !knockbackScript.IsBeingKnockedBack && !isLimps*/)
            Run(1);

        if (canPressOnlyJump) return;
        if (IsSliding)
            Slide();

    //     if (isLimps && LastOnGroundTime < 0)
    //         ChangeAnimationState(JUMP_LIMPS);

    //     if (isLimps || isSittingUp 
    //         || NewPlayerCombat.instance.isAttacking || NewPlayerCombat.instance.isAttackingH && NewDialogueManager.GetInstance().dialogueIsPlaying) return;
    if(LastOnGroundTime > 0 /*&& !isSleepAnim && !isAwakening*/)
    {
            if (horizontal != 0 /* && !NewDialogueManager.GetInstance().dialogueIsPlaying */)
               ChangeAnimationState(RUN);
            else if (!isFallingAtMaxSpeed)
                ChangeAnimationState(IDLE);
    }
    else
    {
        if (IsSliding /*&& !isSleepAnim*/)
        {
            ChangeAnimationState(SLIDING);
        }
        else if (!IsJumping && !isJumpFalling && !IsWallJumping /*&& !NewPlayerCombat.instance.isAttacking && !NewPlayerCombat.instance.attackUp && !NewPlayerCombat.instance.attackDown && !isSleepAnim && !isAwakening && SceneManager.GetActiveScene().name != "T12Tower"*/)
        {
            ChangeAnimationState(JUMP_FALLING);
        }
    }
    }
    public void MoveInput(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }
    public void JumpInput(InputAction.CallbackContext context)
    {
        if (isDashing) return;
        //if (isSit || NewPlayerCombat.instance.isAttacking || NewPlayerCombat.instance.attackUp || NewPlayerCombat.instance.attackDown || NewDialogueManager.GetInstance().dialogueIsPlaying || isDoingSomething) return; 
        if (context.performed /* && canJump*/)
        {

            OnJumpInput();

            if (NewCanJump() && LastPressedJumpTime > 0)
            {
                // SALTO DELLA TORRE
                // if (canPressOnlyJump)
                // {
                //     if(horizontal > 0)
                //     {
                //         IsJumping = true;
                //         IsWallJumping = false;
                //         isJumpCut = false;
                //         isJumpFalling = false;

                //         NewJump();

                //         //StartCoroutine(DrakeScript.instance.DrakeFinish());
                //     }
                // }
                // else 
                // {
                    IsJumping = true;
                    IsWallJumping = false;
                    isJumpCut = false;
                    isJumpFalling = false;

                    NewJump();
                //}
            }
            else if(NewCanWallJump() && LastPressedJumpTime > 0 && canWallJumping)
            {
                IsWallJumping = true;
                IsJumping = false;
                isJumpCut = false;
                isJumpFalling = false;
                wallJumpStartTime = Time.time;
                lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;
                IsAlreadySliding = false;

                NewWallJump(lastWallJumpDir);
            }
        }
        if (context.canceled && IsJumping && !canPressOnlyJump)
        {
            OnJumpUpInput();
        }
        
    }
    public void DashInput(InputAction.CallbackContext context)
    {
        //if (isSit) return;
        if (context.performed && canDash && haveDash /*&& !NewDialogueManager.instance.dialogueIsPlaying*/)
        {   
            //if(PauseManager.paused) return;
            if (!IsSliding) 
            { 
                StartCoroutine(NormalDash());
                //dust.Play();
            }
            else if (IsSliding)
            {
                Flip();
                StartCoroutine(DashWallSlide());
            }
        }
    }

    private void Inputs()
    {
        //if(ShopNPC.shopOpen || !canMoveInput) return;
        if (canMove /* && !playerLife.knockback && !NewPlayerCombat.instance.knockback */) 
        {
            //if(PauseManager.paused) return;
            Movement();
        }

        if (LastOnGroundTime > 0)
        {
            if (horizontal != 0)
                ChangeAnimationState(RUN_LIMPS);
            else
                ChangeAnimationState(IDLE_LIMPS);
        }
        else
            ChangeAnimationState(JUMP_LIMPS); 
    }

    private void RunControlledJump()
    {
        // if (LastOnGroundTime < 0 && horizontal > 0)
        //     Movement();
    }

    private void CheckWorld()
    {
        //isTouchingWall = Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0, wallslideLayer);
        inBorder = Physics2D.OverlapBox(inBorderStart.position, inBorderSize, 0, groundLayer);
    }

    #region Timers Methods
    private void Timers()
    {
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;
    }
    #endregion

    private void Movement()
    {
        // MOVEMENT PER QUANDO IRIS SARA' DANNEGGIATA 
        // if (!isLimps)
        // {
        //     rb.velocity = new Vector2(horizontal * move * moveSpeedIncrease, rb.velocity.y);
        // }
        // else
        // {
        //     rb.velocity = new Vector2(horizontal * 4 * moveSpeedIncrease, rb.velocity.y);
        // }

        rb.linearVelocity = new Vector2(horizontal * move * moveSpeedIncrease, rb.linearVelocity.y);
    }

    #region Flip
    public void DisableFlip()
    {
        canFlip = false;
    }
    public void EnableFlip()
    {
        canFlip = true;
    }
    private void Flip()
    { 
        //if(isSit || !canMoveInput || canPressOnlyJump) return;
        if((facingRight && horizontal < 0.0f || !facingRight && horizontal >= 0.1f) && canFlip /* && !playerLife.knockback */)
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;  
            transform.localScale = localScale;
            // if(LastOnGroundTime > 0)
            // {
            //     dust.Play();
            // }
        }
    }
    public void ManualFlip()
    {
        // Flip the player via code
        if (facingRight || !facingRight)
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            // if (grounded)
            // {
            //     dust.Play();
            // }
        }
    }
    #endregion
    public void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if (currentState == newState) return;

        //play the animation
        anim.Play(newState);

        //ressign the current state
        currentState = newState;
    }

    #region Gravity
    private void Gravity()
    {
        //Called in Update Method
        if (isDashing) return;

        // if (IsInTransition)
        // {
        //     SetGravityScale(0);
        // }
        /* else */ if (IsSliding)
        {
            SetGravityScale(0);
            MaxFallSpeedCounter = 0;
        }
        else if (isJumpCut)
        {
            SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -Data.maxFallSpeed));
        }
        else if((IsJumping || IsWallJumping || isJumpFalling) && Mathf.Abs(rb.linearVelocity.y) < Data.jumpHangTimeThreshold)
        {
            SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
        }
        else if (rb.linearVelocity.y < 0)
        {
            SetGravityScale(Data.gravityFall);
            //Caps the MaxFallingSpeed
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -Data.maxFallSpeed));
        }
        else
        {
            //Set gravity to default value
            SetGravityScale(Data.gravityScale);
        }
    }
    private void SetGravityScale(float scale)
    {
        rb.gravityScale = scale;
    }

    private void CheckForMaxFallSpeed()
    {
        //if (NewDialogueManager.GetInstance().dialogueIsPlaying || canPressOnlyJump) return;

        if (isJumpFalling)
            MaxFallSpeedCounter -= Time.deltaTime;
        else
            MaxFallSpeedCounter = 0;

        if(MaxFallSpeedCounter <= -1.2f)
        {
            isFallingAtMaxSpeed = true;
        }
    }
    private IEnumerator FallingAtMaxSpeed()
    {
        //CameraShakeManager.instance.CameraShake(impulseSource);
        ChangeAnimationState(FALLING_MAXSPEED);
        //DeactivePlayerControls();
        //RumblePulse(0.25f, .5f, .25f);
        yield return new WaitForSeconds(.40f);
        isFallingAtMaxSpeed = false;
    }
    #endregion

    #region Collisions Checks
    private void CollisionsChecks()
    {
        if (!IsJumping)
        {
            //GroundCheck
            if(Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, groundLayer) && !IsJumping)
            {
                LastOnGroundTime = Data.coyoteTime;
            }

            //RightWallCheck
            if (((Physics2D.OverlapBox(frontWallCheckPoint.position, _wallCheckSize, 0, groundLayer) && facingRight) ||
                (Physics2D.OverlapBox(backWallCheckPoint.position, _wallCheckSize, 0, groundLayer) && !facingRight)) && !IsWallJumping)
                LastOnWallRightTime = Data.coyoteTime;

            if(((Physics2D.OverlapBox(frontWallCheckPoint.position, _wallCheckSize, 0, groundLayer) && !facingRight) ||
                (Physics2D.OverlapBox(backWallCheckPoint.position, wallCheckSize, 0, groundLayer) && facingRight)) && !IsWallJumping)
                LastOnWallLeftTime = Data.coyoteTime;

            LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
        }
    }
    #endregion
    #region Run
    private void Run(float lerpAmount)
    {
        float targetSpeed = horizontal * Data.runMaxSpeed;
        //Smooths changes to are direction and speed
        targetSpeed = Mathf.Lerp(rb.linearVelocity.x, targetSpeed, lerpAmount);

        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning)
        //or trying to decelerate 
        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;

        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;

        if (Data.doConserveMomentum && Mathf.Abs(rb.linearVelocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.linearVelocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }
        if (horizontal == 0 && LastOnGroundTime > 0)
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - rb.linearVelocity.x;

        float movement = speedDif * accelRate;

        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);
        //rb.velocity = new Vector2(horizontal * move * moveSpeedIncrease, rb.velocity.y);
    }
    #endregion
    #region DashFunctions
    private IEnumerator NormalDash()
    {
        // NewPlayerCombat.instance.isAttacking = false;
        // NewPlayerCombat.instance.attackDown = false;
        // NewPlayerCombat.instance.attackUp = false;
        canDash = false;
        isDashing = true;
        //PlayerLife.instance.isInvulnerable = true;
        isDashingCooldown = true;
        ChangeAnimationState(DASH);
        transform.SetParent(null);
        //CinemachineShake.Instance.ShakeCamera();
        //FindObjectOfType<AudioManager>().Play("Dash");
        //RumblePulse(0.1f, .12f, 0.25f);
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        // if(facingRight)
        // {
        //     Instantiate(dashParticleRight, transform.position, Quaternion.identity);
        // }
        // else if(!facingRight)
        // {
        //     Instantiate(dashParticleLeft, transform.position, Quaternion.identity);
        // }
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        MaxFallSpeedCounter = 0;
        if(LastOnGroundTime < 0)
        {
            ChangeAnimationState(JUMP_FALLING);
        }
        //PlayerLife.instance.isInvulnerable = false;
        yield return new WaitForSeconds(dashingCooldown);
        isDashingCooldown = false;
    }
    private IEnumerator DashWallSlide()
    {
        canDash = false;
        isDashing = true;
        //PlayerLife.instance.isInvulnerable = true;
        isDashingCooldown = true;
        anim.SetBool("DoubleJump", false);
        transform.SetParent(null);
        anim.SetBool("isDashing", true);
        //CinemachineShake.Instance.ShakeCamera();
        //FindObjectOfType<AudioManager>().Play("Dash");
        //RumblePulse(0.1f, .12f, 0.25f);
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(-transform.localScale.x * dashingPower, 0f);
        isWallSliding = false;
        anim.SetBool("isWallSliding", false);
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        // if (facingRight)
        // {
        //     Instantiate(dashParticleRight, transform.position, Quaternion.identity);
        // }
        // else if (!facingRight)
        // {
        //     Instantiate(dashParticleLeft, transform.position, Quaternion.identity);
        // }
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        //PlayerLife.instance.isInvulnerable = false;
        anim.SetBool("isDashing", false);
        yield return new WaitForSeconds(dashingCooldown);
        isDashingCooldown = false;
    }
    private void ResetDash()
    {
        if(LastOnGroundTime > 0 && !IsSliding && !isDashingCooldown)
        {
            canDash = true;
        }
    }
    // public void Stun()
    // {
    //     if(haveStunDash)
    //     {
    //         Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(dashPos.position, dashStunRadius, enemyLayer);
    //         foreach (Collider2D collider in detectedObjects)
    //         {
    //             collider.transform.parent.SendMessage("Stun");
    //             Debug.Log("Stun by dash!");
    //         }
    //     }
    // }
    #endregion
    #region Slide
    private void SlideChecks()
    {
        if (CanSlide() && ((LastOnWallLeftTime > 0 && horizontal < 0) || (LastOnWallRightTime > 0 && horizontal > 0)))
        {
            IsSliding = true;
        }
        else
        {
            IsSliding = false;
            /*
            if (preventForceSlide)
            {
                preventForceSlide = false;
            }
            */
        }              
    }
    public bool CanSlide()
    {
        if (LastOnWallTime > 0 && !IsJumping && !IsWallJumping && LastOnGroundTime <= 0 && canSliding)
            return true;
        else
            return false;
    }
    private void Slide()
    {
        /*
        if (!preventForceSlide)
        {
            rb.velocity = Vector2.zero;
            preventForceSlide = true;
        }
        */
        float speedDif = Data.slideSpeed - rb.linearVelocity.y;
        float movement = speedDif * Data.slideAccel;

        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        rb.AddForce(movement * Vector2.up);
        IsAlreadySliding = true;
    }
    #endregion
    #region Jump
    private void NewJump()
    {
        //Make sure we can't call NewJump multiple times from one press
        rb.linearVelocity = Vector2.zero;
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        SetGravityScale(Data.gravityScale);
        //dust.Play();

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        if (horizontal == 0)
            ChangeAnimationState(JUMP);
        else
        {
            if (canPressOnlyJump)
                ChangeAnimationState("JumpBridgeUp");
            else
                ChangeAnimationState(JUMP_MOVING);
        }
    }
    private void JumpChecks()
    {
        if(IsJumping && rb.linearVelocity.y < 0)
        {
            IsJumping = false;

            if (!IsWallJumping)
            {
                isJumpFalling = true;
                if (horizontal == 0 && !canPressOnlyJump /*&& !NewPlayerCombat.instance.isAttacking && !NewPlayerCombat.instance.attackUp && !NewPlayerCombat.instance.attackDown && SceneManager.GetActiveScene().name != "T12Tower" */)
                {
                    ChangeAnimationState(JUMP_FALLING);
                }
                else
                    ChangeAnimationState(JUMP_MOVINGDOWN);
                // else if (!NewPlayerCombat.instance.isAttacking && !NewPlayerCombat.instance.attackUp && !NewPlayerCombat.instance.attackDown && SceneManager.GetActiveScene().name != "T12Tower")
                // {
                //     if(canPressOnlyJump)
                //         ChangeAnimationState("JumpBridgeDown");
                //     else
                //         ChangeAnimationState(JUMP_MOVINGDOWN);
                // }
            }
        }
        
        if(IsWallJumping && Time.time - wallJumpStartTime > Data.wallJumpTime)
        {
            IsWallJumping = false;
        }

        if(LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
            isJumpCut = false;

            if (!IsJumping)
                isJumpFalling = false;
        }
    }

    private bool NewCanJump()
    {
        return LastOnGroundTime > 0 && !IsJumping;
    }
    public void OnJumpInput()
    {
        LastPressedJumpTime = Data.jumpInputBufferTime;
    }
    public void OnJumpUpInput()
    {
        if (CanJumpCut() || CanWallJumpCut())
            isJumpCut = true;
    }

    #endregion
    #region WallJump
    private bool CanJumpCut()
    {
        return IsJumping && rb.linearVelocity.y > 0;
    }
    private bool CanWallJumpCut()
    {
        return IsWallJumping && rb.linearVelocity.y > 0;
    }
    private bool NewCanWallJump()
    {
        return LastPressedJumpTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && (!IsWallJumping || (LastOnWallRightTime > 0 && lastWallJumpDir == 1) /* || (LastOnWallLeftTime > 0 && lastWallJumpDir == -1) */);
    }
    private void NewWallJump(int dir)
    {
        isFallingAtMaxSpeed = false;
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        LastOnWallRightTime = 0;
        LastOnWallLeftTime = 0;
        MaxFallSpeedCounter = 0;
        //anim.SetTrigger("WallJump");

        canFlip = false;
        rb.linearVelocity = Vector2.zero;
        Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
        force.x *= dir; //apply force in opposite direction of wall

        
        if (Mathf.Sign(rb.linearVelocity.x) != Mathf.Sign(force.x))
            force.x -= rb.linearVelocity.x;

        /*
        if(rb.velocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our deisre jump force or greater
            force.y -= rb.velocity.y;
        */
        rb.AddForce(force, ForceMode2D.Impulse);
        ManualFlip();
        ChangeAnimationState(WALLJUMP);
        StartCoroutine(WaitZeroForce());
    }
    private IEnumerator WaitZeroForce()
    {
        yield return new WaitForSeconds(.3f);
        //rb.velocity = Vector2.zero;
        canFlip = true;
        //ChangeAnimationState(JUMP_FALLING);
    }
    #endregion
    #region OnDrawGizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        //Gizmos.DrawCube(groundCheckPoint.position, groundCheckSize);
        //Gizmos.DrawCube(wallCheckPoint.position, wallCheckSize);

        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
        Gizmos.DrawWireCube(frontWallCheckPoint.position, _wallCheckSize);
        Gizmos.DrawWireCube(backWallCheckPoint.position, _wallCheckSize);
    }
    #endregion
}
