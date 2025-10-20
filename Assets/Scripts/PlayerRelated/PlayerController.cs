using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction jumpAction;

    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float acceleration = 7;
    [SerializeField] private float decceleration = 7;
    [SerializeField] private float velPower = 0.9f;
    [SerializeField] private float frictionAmount = 0.2f;
    [SerializeField] private float jumpHeight = 10;
    [SerializeField] private float gravityScale = 20;
    [SerializeField] private float fallGravityMultiplier = 1.5f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;
    [SerializeField] private HealthManager healthManager;
    private float moveHor;
    private int grounded = 0;
    private float bufferedJump = Mathf.Infinity;
    [SerializeField] private float bufferedJumpMax = 0.2f;
    public int moveable;
    private bool jumpInputReleased = true;
    GameState gameState;

    private void Awake()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameState>();

        GameObject[] allTransitions = GameObject.FindGameObjectsWithTag("SceneTransition");
        foreach (GameObject sceneTransition in allTransitions)
        {
            if (sceneTransition.GetComponent<SceneTransition>().sceneName==gameState.spawnPointName && sceneTransition.GetComponent<SceneTransition>().transitionType==gameState.transitionType)
            {
                this.transform.position = sceneTransition.transform.GetChild(0).gameObject.transform.position;
                break;
            }
        }

        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        healthManager = GetComponent<HealthManager>();


        if (gameState.maxHealth != 0)
        {
            healthManager.maxHealth = gameState.maxHealth;
            healthManager.currentHealth = gameState.currentHealth;
        }

    }
    void Start()
    {
        moveable = 0;
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        
    }

    void Update()
    {
        if (moveable == 0)
        {
            moveHor = moveAction.ReadValue<Vector2>()[0];
        }
        else
        {
            moveHor = 0;
        }
        if (jumpAction.inProgress)
        {
            bufferedJump = 0;
        }
        if (moveable==0 && bufferedJump < bufferedJumpMax && moveable == 0 && grounded > 0)
        {
            animator.SetTrigger("Jump");//AnimationEventJump() gets triggered
        }
        if (!jumpInputReleased&& !jumpAction.inProgress)
        {
            if (rb.linearVelocityY > 0)
            {
                rb.AddForce(Vector2.down * rb.linearVelocityY * (1-jumpCutMultiplier), ForceMode2D.Impulse);
            }
            jumpInputReleased = true;
        }
        animator.SetBool("Walking", moveHor != 0);
        bufferedJump += Time.deltaTime;
    }
    void FixedUpdate()
    {
        if (moveHor < -0.01f)
        {
            sprite.flipX = true;
        }
        else if (moveHor > 0.01f)
        {
            sprite.flipX = false;
        }

        #region Walk
        float targetSpeed = moveHor * moveSpeed;
        float speedDif = targetSpeed - rb.linearVelocityX;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        rb.AddForce(movement * Vector2.right); //Fluid movement
                                               //rb.linearVelocityX = moveHor * speed; //Old movement
        #endregion
        #region Friction
        if (grounded>0 && Mathf.Abs(moveHor) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.linearVelocityX), Mathf.Abs(frictionAmount));
            amount*=Mathf.Sign(rb.linearVelocityX);
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
        #endregion

        #region Jump Gravity
        if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
        #endregion
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            grounded+=1;
            animator.SetInteger("Ground", grounded);
            if (grounded==1)
            {
                animator.ResetTrigger("Jump");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            grounded-=1;
            animator.SetInteger("Ground", grounded);
        }
    }


    public void Jump(float _jumpHeight)
    {
        
        rb.AddForce(Vector2.up * _jumpHeight, ForceMode2D.Impulse);
        jumpInputReleased = false;

    }
    public void AnimationEventJump()
    {
        Jump(jumpHeight);
    }
    public void TakeDamage()
    {
        animator.SetTrigger("Hurt");
        //To-do: Physical knockback
    }

    public void Die()
    {
        animator.SetTrigger("Dead");
        moveable += 1;
        //GameObject overlay = GameObject.Find("/PlayerInterface/DeathOverlay");
        //overlay.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        StartCoroutine(gameState.RespawnPlayer(1));
    }

}
