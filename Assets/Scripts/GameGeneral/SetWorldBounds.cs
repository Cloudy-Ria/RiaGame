using UnityEngine;

public class SetWorldBounds : MonoBehaviour
{
    private void Awake()
    {
        var bounds = GetComponent<BoxCollider2D>().bounds;
        Globals.WorldBounds = bounds;
    }
}
