using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction interactAction;


    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float acceleration = 7;
    [SerializeField] private float decceleration = 7;
    [SerializeField] private float velPower = 0.9f;
    [SerializeField] private float frictionAmount = 0.2f;
    [SerializeField] private float jumpHeight = 10;
    [SerializeField] private float gravityScale = 20;
    [SerializeField] private float fallGravityMultiplier = 1.5f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private float knockbackStrength = 10f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;
    [SerializeField] private HealthManager healthManager;
    private float moveHor;
    private int grounded = 0;
    private float bufferedJump = Mathf.Infinity;
    [SerializeField] private float bufferedJumpMax = 0.1f;
    public int moveable; //0 = true. Please use ++ and -- to change values.
    private bool jumpActionEnded = true;
    [SerializeField] private bool isBusy = false;
    private bool isInteracting = false;

    GameState gameState;

    private void Awake()
    {

    }
    void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameState>();

        GameObject[] allTransitions = GameObject.FindGameObjectsWithTag("SceneTransition");
        foreach (GameObject sceneTransition in allTransitions)
        {
            if (sceneTransition.GetComponent<SceneTransition>().sceneName == gameState.spawnPointName && sceneTransition.GetComponent<SceneTransition>().transitionType == gameState.transitionType)
            {
                this.transform.position = sceneTransition.transform.GetChild(0).gameObject.transform.position;
                break;
            }
        }

        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        healthManager = GetComponent<HealthManager>();
        sprite.flipX = gameState.flipX;


        if (gameState.maxHealth != 0)
        {
            healthManager.maxHealth = gameState.maxHealth;
            healthManager.currentHealth = gameState.currentHealth;
        }
        moveable = 1;
        animator.enabled = false;


        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        interactAction = InputSystem.actions.FindAction("Interact");

        StartCoroutine(gameState.SceneFadeIn());

    }

    void Update()
    {
        //Button Buffers
        if (jumpAction.inProgress)
        {
            bufferedJump = 0;
        }

        //If the player cuts the jump
        if (!jumpActionEnded && !jumpAction.inProgress)
        {
            jumpActionEnded = true;

            if (rb.linearVelocityY > 0)
            {
                rb.AddForce(Vector2.down * rb.linearVelocityY * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
            }
        }
        //If the player is immovable (in a cutscene, dead, loading in, etc.)
        if (moveable != 0 || IsBusy())
        {
            moveHor = 0;
            animator.SetBool("Walking", false);
            return;
        }
        //Movement
        moveHor = moveAction.ReadValue<Vector2>()[0];
        animator.SetBool("Walking", moveHor != 0);

        

        //Jump
        if (bufferedJump < bufferedJumpMax && jumpAction.inProgress && IsGrounded() && !IsBusy() && jumpActionEnded == true)
        {
            animator.SetTrigger("Jump");
            Jump(jumpHeight);
        }

        isInteracting = interactAction.inProgress;

        //Buffer Timers
        bufferedJump += Time.deltaTime;
    }
    void FixedUpdate()
    {
        //Sprite Flip
        if (moveHor < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveHor > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        //Walk
        float targetSpeed = moveHor * moveSpeed;
        float speedDif = targetSpeed - rb.linearVelocityX;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        rb.AddForce(movement * Vector2.right); //Fluid movement
                                               //rb.linearVelocityX = moveHor * speed; //Old movement
                                               //Friction
        if (IsGrounded() && Mathf.Abs(moveHor) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.linearVelocityX), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(rb.linearVelocityX);
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }

        //Jump Gravity
        if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            grounded += 1;
            animator.SetInteger("Ground", grounded);
            if (grounded == 1)
            {
                animator.ResetTrigger("Jump");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            grounded -= 1;
            animator.SetInteger("Ground", grounded);
        }
    }
    public void Jump(float _jumpHeight)
    {

        rb.AddForce(Vector2.up * _jumpHeight, ForceMode2D.Impulse);
        jumpActionEnded = false;

    }

    public void Knock(Vector2 direction, float _jumpHeight)
    {
        rb.AddForce(direction * _jumpHeight, ForceMode2D.Impulse);
    }

    public void TakeDamage(GameObject source)
    {
        animator.SetTrigger("Hurt");
        float x_direction = transform.position.x - source.transform.position.x;
        float y_direction = transform.position.y - source.transform.position.y;
        x_direction = Mathf.Sign(x_direction);
        y_direction = Mathf.Clamp01(Mathf.Sign(y_direction));
        Knock(new Vector2(x_direction, y_direction*2.5f), knockbackStrength);
    }

    public void Die()
    {
        animator.SetTrigger("Dead");
        moveable += 1;
        StartCoroutine(gameState.RespawnPlayer(1));
    }

    public void DontMove()
    {
        moveable++;
    }
    public void DoMove()
    {
        moveable--;
    }

    public bool IsGrounded()
    {
        return (grounded > 0);
    }
    public bool IsMoveable()
    {
        return moveable == 0;
    }
    public bool IsBusy()
    {
        return isBusy;
    }
    public void StartBeingBusy()
    {
        isBusy = true;
    }

    public void StopBeingBusy()
    {
        isBusy = false;
    }

    public bool IsInteracting()
    {
        return isInteracting;
    }
}
