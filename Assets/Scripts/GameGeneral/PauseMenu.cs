using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private InputAction menuAction;
    private bool gamePaused = false;
    SceneBlackout sceneBlackout;
    PlayerController playerController;
    private GameObject pauseMenuUI;
    private GameObject creditsUI;


    void Start()
    {
        sceneBlackout = GameObject.Find("Blackout").GetComponent<SceneBlackout>();
        menuAction = InputSystem.actions.FindAction("Menu");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        pauseMenuUI = this.gameObject.transform.Find("PauseMenuUI").gameObject;
        creditsUI = this.gameObject.transform.Find("CreditsUI").gameObject;

        pauseMenuUI.SetActive(false);
        creditsUI.SetActive(false);

    }

    
    void Update()
    {
        if (menuAction.triggered)
        {
            if (!gamePaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        gamePaused = true;
        sceneBlackout.SetTo(0.8f);
        playerController.DontMove();
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
    }
    public void Resume()
    {
        gamePaused = false;
        sceneBlackout.SetTo(0f);
        playerController.DoMove();
        Time.timeScale = 1.0f;
        pauseMenuUI.SetActive(false);
    }

    public void OpenCredits()
    {
        pauseMenuUI.SetActive(false);
        creditsUI.SetActive(true);
    }

    public void CloseCredits()
    {
        pauseMenuUI.SetActive(true);
        creditsUI.SetActive(false);
    }

    public void QuitAndSaveGame()
    {
        var gameState = GameObject.Find("GameState").GetComponent<GameState>();
        if (gameState != null)
        {
            gameState.SaveToJson();
        }
        Application.Quit();
        Debug.Log("Quit the game.");
    }

    public void SaveGame()
    {
        var gameState = GameObject.Find("GameState").GetComponent<GameState>();
        if (gameState != null)
        {
            gameState.SaveToJson();
        }
    }

    public void DeleteSaveData()
    {
        PlayerPrefs.DeleteAll();
        BinaryFormatter formatter = new BinaryFormatter();

        string path1 = Application.persistentDataPath + '\\' + "data.muahaha";
        //FileStream fileStream1 = new FileStream(path1, FileMode.Truncate);
        File.Delete(path1);
        //fileStream1.Close();

        string path2 = Application.persistentDataPath + '\\' + "itemsRiaMemory.muahaha";
        //FileStream fileStream2 = new FileStream(path2, FileMode.Truncate);
        File.Delete(path2);
        //fileStream2.Close();

        string path3 = Application.persistentDataPath + '\\' + "sceneIterations.muahaha";
        //FileStream fileStream3 = new FileStream(path3, FileMode.Truncate);
        File.Delete(path3);
        //fileStream3.Close();

        string path4 = Application.persistentDataPath + '\\' + "targetSceneIteration.muahaha";
        //FileStream fileStream4 = new FileStream(path4, FileMode.Truncate);
        File.Delete(path4);
        //fileStream4.Close();

        string path5 = Application.persistentDataPath + '\\' + "scenePhotosTaken.muahaha";
        //FileStream fileStream5 = new FileStream(path5, FileMode.Truncate);
        File.Delete(path5);
        //fileStream5.Close();

    }

}
