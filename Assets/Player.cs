using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim { get; private set; }
    public PlayerInputSet input { get; private set; }
    public StateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public Rigidbody2D rb { get; private set; }
    [SerializeField] public float moveSpeed ;
    public bool faceRight = true;
    public int facingDir { get; private set; } = 1;
    public float jumpForce = 5;
    public Vector2 wallJumpDirection;
    [Range(0,1)]
    public float inAirMoveMultiplier = 0.7f;
    [Space]
    public float dashDuration = .25f;
    public JumpState jumpState { get; private set; }
    public FallState fallState { get; private set; }
    public WallSlideState wallSlide { get; private set; } 

    public WallJumpState wallJumpState { get; private set; }

    public PlayerDashState playerDashState { get; private set; }
    public PlayerBasicAttack playerBasicAttack { get; private set; }

    public PlayerJumpAttackState jumpAttackState { get; private set; }
    public Vector2 moveInput { get; private set; }
    [Header("Collision Detection")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] public bool groundDetected;
    [SerializeField] public bool wallDetected;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private float wallCheckDistance;
    public float playerDashSpeed = 20;
    public float comboResetTime = 1;
    private Coroutine quededAttack;

    [Header("AttackDetails")]
    public Vector2[] attackVelocity;
    public float attackVelocityDuration = .1f;
 

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        input = new PlayerInputSet();
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine();
        idleState = new PlayerIdleState(stateMachine, this, "idle");
        moveState = new PlayerMoveState(stateMachine, this,"move");
        jumpState = new JumpState(stateMachine, this, "jumpFall");
        fallState = new FallState(stateMachine, this, "jumpFall");
        wallSlide = new WallSlideState(stateMachine, this, "wallSlide");
        wallJumpState = new WallJumpState(stateMachine, this, "jumpFall");
        playerDashState = new PlayerDashState(stateMachine, this, "dash");
        playerBasicAttack = new PlayerBasicAttack(stateMachine, this, "basicAttack");
        jumpAttackState = new PlayerJumpAttackState(stateMachine, this, "jumpAttack");

    }

    private void OnEnable()
    {
        input.Enable();
        //input.Player.Movement.started  : when input just begins, just touch -> action
        //input.Player.Movement.performed : when input is performed, fully pressed the button or hold -> action(movmment)
        // input.Player.Movement.canceled  : when input just begins, after the button is left
        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>() ; //ctx stands for context
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }
    private void OnDisable()
    {
        input.Disable();
    }
    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
        HandleFlip();
        HandleCollisionDetection();
        

    }
    private void HandleFlip()
    {
        if (rb.linearVelocity.x < 0 && faceRight == true)
        {
            Flip();
        }
        else if (rb.linearVelocity.x > 0 && faceRight == false)
        {
            Flip();
        }
    }
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
    }

    public void CallAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger();
    }
    public void Flip()
    {
        transform.Rotate(0, -180, 0);
        faceRight = !faceRight;
        facingDir = -1 * facingDir;
    }
    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance-0.01f, whatIsGround);
        wallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, whatIsWall);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(facingDir * wallCheckDistance, 0));
    }

    public void EnterAttackStateWithDelay()
    {
        if (quededAttack != null)
        {
            StopCoroutine(quededAttack);
        }
        quededAttack = StartCoroutine(EnterAttackStateWithDelayCo());
    }

    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(playerBasicAttack);
    }
} 
