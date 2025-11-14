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

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit the game.");
    }

}
