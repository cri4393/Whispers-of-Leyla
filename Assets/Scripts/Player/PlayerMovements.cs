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
    const string JUMP = "Jump";
    const string JUMP_MOVING = "JumpMoving";
    const string JUMP_MOVINGDOWN = "JumpMovingDown";
    const string JUMP_FALLING = "Falling";
    const string FALLING_MAXSPEED = "FallMaxSpeed";
    const string SLIDING = "WallSlide";
    const string WALLJUMP = "WallJump";
    const string DASH = "Dash";
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
    public bool canSliding;
    private bool canWallJumping = false;
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
        //ResetDash();
        //LerpingY();

        //Timers();
        CheckForMaxFallSpeed();
        Gravity();

        CollisionsChecks();
        //JumpChecks();
        //SlideChecks();

        
        /*
        if (isFallingAtMaxSpeed)
        {
            if(LastOnGroundTime > 0)
            {
                StartCoroutine(FallingAtMaxSpeed());
            }
        }
        */

        // if (LastOnGroundTime > 0 || LastOnWallTime < 0)
        //     IsAlreadySliding = false;

        /*
        if (IsAlreadySliding)
        {
            IsSliding = true;
            Slide();
        }
        else
            SlideChecks();
        */
        
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

        // if (isDashing)
        // {
        //     return;
        // }
        Flip();

        // if(NewPlayerCombat.instance.attackUp || NewPlayerCombat.instance.attackDown) return;
        
        // if (isSit) return;
        if (isLimps)
            Inputs();
        // else if (canPressOnlyJump)
        //     RunControlledJump();
        else if (!IsWallJumping /* && !playerLife.knockback  && !knockbackScript.IsBeingKnockedBack && !isLimps*/)
            Run(1);

        //if (canPressOnlyJump) return;
        //if (IsSliding)
        //    Slide();

    //     if (isLimps && LastOnGroundTime < 0)
    //         ChangeAnimationState(JUMP_LIMPS);

    //     if (isLimps || isSittingUp 
    //         || NewPlayerCombat.instance.isAttacking || NewPlayerCombat.instance.isAttackingH && NewDialogueManager.GetInstance().dialogueIsPlaying) return;
    //     if(LastOnGroundTime > 0 && !isSleepAnim && !isAwakening)
    //     {
            if (horizontal != 0 /* && !NewDialogueManager.GetInstance().dialogueIsPlaying */)
               ChangeAnimationState(RUN);
            else if (!isFallingAtMaxSpeed)
                ChangeAnimationState(IDLE);
    //}
    //     else
    //     {
    //         if (IsSliding && !isSleepAnim)
    //         {
    //             ChangeAnimationState(SLIDING);
    //         }
    //         else if (!IsJumping && !isJumpFalling && !IsWallJumping && !NewPlayerCombat.instance.isAttacking && !NewPlayerCombat.instance.attackUp && !NewPlayerCombat.instance.attackDown && !isSleepAnim && !isAwakening && SceneManager.GetActiveScene().name != "T12Tower")
    //         {
    //             ChangeAnimationState(JUMP_FALLING);
    //         }
    //     }
    }
    public void MoveInput(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
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
        if (LastOnGroundTime < 0 && horizontal > 0)
            Movement();
    }

    private void CheckWorld()
    {
        //isTouchingWall = Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0, wallslideLayer);
        inBorder = Physics2D.OverlapBox(inBorderStart.position, inBorderSize, 0, groundLayer);
    }

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
        // if (isDashing) return;

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
