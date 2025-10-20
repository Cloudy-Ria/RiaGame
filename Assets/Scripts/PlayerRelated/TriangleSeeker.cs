using System.Runtime.CompilerServices;
using UnityEngine;

public class TriangleSeeker : MonoBehaviour
{
    [SerializeField] private MouseCoordinates mouseCoordinates;
    [SerializeField] private Vector3 worldPoint;
    [SerializeField] private RaycastHit2D hit;
    [SerializeField] private GameObject activeObject;
    [SerializeField] private LayerMask mask;
    void Update()
    {
        worldPoint = Camera.main.ScreenToWorldPoint(mouseCoordinates.showMouse());
        mask = LayerMask.GetMask("Water");
    }

    private void FixedUpdate()
    {
        hit = Physics2D.Raycast(worldPoint, Vector3.forward, Mathf.Infinity, mask);
        if (hit.collider != null)
        {
            activeObject = hit.collider.gameObject;
            if (activeObject != null)
            {
                activeObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            
        }
        else
        {
            if (activeObject != null)
            {
                activeObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            activeObject = null;
        }
    }
}
