using System.Collections;
using System.Xml.Serialization;
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
    private SceneBlackout sceneBlackout;

    private void Start()
    {
        gameState = GameObject.Find("GameState").GetComponent<GameState>();
        sceneBlackout = GameObject.Find("Blackout").GetComponent<SceneBlackout>();
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
        gameState.respawnScene = sceneName;

        //Scene loading
        var newScene = SceneManager.LoadSceneAsync(gameState.GetSceneIteration(sceneName));
        newScene.allowSceneActivation = false;

        sceneBlackout.FadeTo(1);
        yield return new WaitForSeconds(SceneBlackout.immovableTime);
        sceneBlackout.SetTo(1);

        gameState.SceneLoadInProgress = false;
        newScene.allowSceneActivation = true;
    }

}
