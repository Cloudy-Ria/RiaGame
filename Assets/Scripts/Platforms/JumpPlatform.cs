using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    [SerializeField] private PlayerController pc;
    [SerializeField] private float jumpHeight;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")) 
        {
            pc = col.gameObject.GetComponent<PlayerController>();
            pc.Jump(jumpHeight);
        }
        
    }

}
