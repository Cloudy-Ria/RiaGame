using System.Collections;
using System.Xml.Serialization;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class SceneTransition : MonoBehaviour
{
    //public int sceneBuildIndex;
    public string sceneName;
    private int scenesCount;
    private string sceneFullName;
    public string transitionType; //Only set when there are multiple transitions between the same two rooms
    private GameState gameState;

    private void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameState>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && gameState.SceneLoadInProgress == false)
        {
            gameState.SceneLoadInProgress = true;
            StartCoroutine(Transition());

            //SceneManager.LoadScene(gameState.GetSceneIteration(sceneName), LoadSceneMode.Single);
            
        }
    }
    IEnumerator Transition()
    {
        //Getting components
        string currentSceneName = SceneManager.GetActiveScene().name;
        currentSceneName = currentSceneName.Substring(0, currentSceneName.Length - 3);
        gameState.SceneLoadInProgress = true;

        //Saving Data
        gameState.spawnPointName = currentSceneName;
        gameState.transitionType = transitionType;
        gameState.OnTransition(currentSceneName);

        //Scene loading
        var newScene = SceneManager.LoadSceneAsync(gameState.GetSceneIteration(sceneName));
        newScene.allowSceneActivation = false;
        float t = 0;

        while (newScene.progress < 0.9f || t < 1)
        {
            t += Time.deltaTime*4;
            t = Mathf.Clamp01(t);
            gameState.blackoutAlpha = t;
            yield return null;
        }

        gameState.SceneLoadInProgress = false;
        newScene.allowSceneActivation = true;
    }

}
