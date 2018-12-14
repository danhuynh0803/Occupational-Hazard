using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject controlMenu;
    public GameObject soundMenu;
    public GameObject creditMenu;
    public GameObject loadingScreen;
    public GameObject levelController;
    public GameObject gameModeMenu;
    public GameObject mainMenuPanel;
    public GameObject pauseMenu;
    public string GameSceneName;
    public bool isInGameMenu;

    public Stack<GameObject> menuStack;

    private bool isPaused;
    private Fading fadeController;

    void Start()
    {
        isPaused = false;
        menuStack = new Stack<GameObject>();
        fadeController = FindObjectOfType<Fading>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // Close any open windows that is NOT the main pause menu
            if (menuStack.Count > 0)
            {
                GameObject frontMenu = menuStack.Peek();
                frontMenu.SetActive(false);
                menuStack.Pop();
            }
            // If the pause menu is open, this allows players to exit by clicking escape
            else if (menuStack.Count == 0 && isPaused)
            {
                isPaused = false;
            }
            // Opens the pause menu using escape
            else if (menuStack.Count == 0 && !isPaused)
            {
                isPaused = true;
            }
        }

  
        //Handing opening/closing of the pause menu
        if (isPaused && !GameController.isGameOver)
        {
            Pause();
        }
        else
        {
            if(!GameController.isGameOver)
                Resume();
        }
    }

    public void LoadMainMenuScene()
    {
        //SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        isPaused = false;
        StartCoroutine(ChangeScene("MainMenu"));
        //SoundController.Play((int)SFX.Load);
    }
    public void StartGame()
    {
        isPaused = false;
        StartCoroutine(ChangeScene(GameSceneName));
    }


    public void LoadScene(string sceneName)
    {
        if (loadingScreen != null)
        {
            StartCoroutine(ChangeScene(sceneName));
            if (isInGameMenu)
            {
                //GUISoundController.Play((int)GUISFX.Load);
            }
        }
    }

    IEnumerator ChangeScene(string sceneName)
    {
        float fadeTime = fadeController.BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void Pause()
    {
        if (pauseMenu != null)
        {
            Time.timeScale = 0.0f;
            pauseMenu.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void Resume()
    {
        if (pauseMenu != null)
        {
            isPaused = false;
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);
        }
    }

    public void ToggleControlMenu(bool boolean)
    {
        if (controlMenu != null)
        {
            if (isInGameMenu)
            {
                //GUISoundController.Play((int)GUISFX.Click);
            }
            else
            {
                //SoundController.Play((int)SFX.Click);
            }
            if (boolean)
                menuStack.Push(controlMenu);
            else
                menuStack.Pop();
            controlMenu.SetActive(boolean);
        }
    }
    public void ToggleSoundlMenu(bool boolean)
    {
        if (soundMenu != null)
        {
            if (isInGameMenu)
            {
                //GUISoundController.Play((int)GUISFX.Click);
            }
            else
            {
                //SoundController.Play((int)SFX.Click);
            }
            if (boolean)
                menuStack.Push(soundMenu);
            else
                menuStack.Pop();
            soundMenu.SetActive(boolean);
        }
    }
    public void ToggleCredit(bool boolean)
    {
        
        if (creditMenu != null)
        {
            //GUISoundController.Play((int)GUISFX.Click);
            if (boolean)
                menuStack.Push(creditMenu);
            else
                menuStack.Pop();
            creditMenu.SetActive(boolean);
        }
    }
    public void ToggleGameModeMenu(bool boolean)
    {
        mainMenuPanel.SetActive(false);
        if (gameModeMenu != null)
        {
            //GUISoundController.Play((int)GUISFX.Click);
            if (boolean)
                menuStack.Push(gameModeMenu);
            else
            {
                menuStack.Pop();
                mainMenuPanel.SetActive(true);
            }
            gameModeMenu.SetActive(boolean);
        }
    }

    public void ExitGame()
    {
        //GUISoundController.Play((int)GUISFX.Click);
        Application.Quit();
    }

}
