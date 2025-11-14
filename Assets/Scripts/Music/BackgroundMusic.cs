using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public Transform playerCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCamera = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerCamera.position;
    }

    public void UpdatePlayer()
    {
        playerCamera = GameObject.Find("Main Camera").GetComponent<Transform>();
    }
}
