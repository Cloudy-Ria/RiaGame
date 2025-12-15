using UnityEngine;

public class PlayerMeleeAttackHitbox : MonoBehaviour
{
    private PlayerAttack playerAttack;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Attack.");
        /*if (collision.gameObject.CompareTag("Monster"))
        {
            //DamageMonster()
        }*/
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerAttack.DeactivatePogoHitbox();
        }
    }
}
