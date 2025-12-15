using UnityEngine;

public class TeleportPlattform : MonoBehaviour
{
    [SerializeField] private Transform playerTrans;
    [SerializeField] private Transform destinationTrans;
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            playerTrans = col.gameObject.GetComponent<Transform>();
            playerTrans.position = playerTrans.position-transform.position+destinationTrans.position;
        }

    }
}
