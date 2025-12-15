using Enemies;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifespan;
    [SerializeField] float damage = 5f;
    private float direction;
    private bool hit;
    private float timeAlive = 0f;


    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= lifespan)
        {
            boxCollider.enabled = false;
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }

        if (!collision.gameObject.CompareTag("Player"))
        {
            hit = true;
            boxCollider.enabled = false;
            gameObject.SetActive(false);
        }

    }
    public void SetDirection(float _direction)
    {
        gameObject.SetActive(true);
        direction = _direction;
        hit = false;
        boxCollider.enabled = true;
        timeAlive = 0;

        transform.localScale = new Vector3(transform.localScale.x*Mathf.Sign(transform.localScale.x)*direction, transform.localScale.y, transform.localScale.z);
    }
}
