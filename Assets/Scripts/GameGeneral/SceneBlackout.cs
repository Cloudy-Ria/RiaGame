using System.Runtime.CompilerServices;
using UnityEngine;

public class SceneBlackout : MonoBehaviour
{
    private GameState gameState;
    private UnityEngine.UI.Image blackoutImage;
    void Start()
    {
         gameState = GameObject.Find("GameState").GetComponent<GameState>();
         blackoutImage = GetComponent<UnityEngine.UI.Image>();
    }

    void Update()
    {
        blackoutImage.color = new Color(0f, 0f, 0f, gameState.blackoutAlpha);
    }
}
