using UnityEngine;

public class MemorableItem : MonoBehaviour
{
    [SerializeField] public string itemName;
    public GameObject player;
    private bool playerInRange = false;
    GameState gameState;
    private void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameState>();
    }
    void Update()
    {
        if (playerInRange)
        {
            gameState.MemoriseItem(itemName);
        }
        if (playerInRange && gameState.fixedRiaMemory && gameState.objectPhotosTaken[itemName]==false)
        {
            gameState.objectPhotosTaken[itemName] = true;
            player.GetComponent<Animator>().SetTrigger("Camera");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            player = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
