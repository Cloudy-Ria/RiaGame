using UnityEngine;

public class CameraItem : MonoBehaviour
{
    public PlayerController player;
    private bool playerInRange = false;
    GameState gameState;

    private void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameState>();
        if (gameState.fixedRiaMemory)
        {
            gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if (playerInRange && player.IsInteracting())
        {
            gameState.FixRiaMemory();
            Debug.Log("Obtained Camera!");
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            player = collision.gameObject.GetComponent<PlayerController>();
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
