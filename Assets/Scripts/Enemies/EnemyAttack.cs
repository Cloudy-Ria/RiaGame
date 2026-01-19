using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float chaseSpeed = 5f;

    private Rigidbody2D rb;
    private Transform player;
    private bool facingRight = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * chaseSpeed;

        if (dir.x > 0 && !facingRight) Flip();
        if (dir.x < 0 && facingRight) Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            player = col.transform;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            player = null;
    }
}
