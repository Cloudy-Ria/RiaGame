using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class SceneTransition : MonoBehaviour
{
    //public int sceneBuildIndex;
    public string sceneName;
    private int scenesCount;
    private string sceneFullName;
    public string transitionType; //Only set when there are multiple transitions between the same two rooms
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            currentSceneName = currentSceneName.Substring(0, currentSceneName.Length - 3);
            var gameState = GameObject.Find("GameState").GetComponent<GameState>();
            
            gameState.spawnPointName = currentSceneName;
            gameState.transitionType = transitionType;

            gameState.OnTransition(currentSceneName);

            SceneManager.LoadScene(gameState.GetSceneIteration(sceneName), LoadSceneMode.Single);
        }
    }
}
