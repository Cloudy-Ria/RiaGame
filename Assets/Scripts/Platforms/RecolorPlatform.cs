using UnityEngine;

public class RecolorPlatform : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerSprite;
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            playerSprite = col.gameObject.GetComponent<SpriteRenderer>();
            playerSprite.color = GetComponent<SpriteRenderer>().color;
        }

    }
}
