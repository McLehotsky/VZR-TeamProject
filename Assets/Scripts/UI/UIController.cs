using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject mainMenuCanvas;
    public GameObject optionsCanvas;
    public GameObject howToPlayCanvas;
    public GameObject pauseMenuCanvas;
    public GameObject ResolutionCanvas;
    public GameObject checkboxFullscreen;

    [Header("Scene Settings")]
    public string gameSceneName = "WorldBuilding"; // Presný názov scény, kde sa hrá hra

    // Start Game Button
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Options Button
    public void OpenOptions()
    {
        mainMenuCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
    }

    // How To Play Button
    public void OpenHowToPlay()
    {
        mainMenuCanvas.SetActive(false);
        howToPlayCanvas.SetActive(true);
    }

    // Quit Button
    public void QuitGame()
    {
        Debug.Log("Game Exited");
        Application.Quit();
    }

    public void PauseGame()
    {
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // Back Button
    public void BackToMainMenu()
    {
        optionsCanvas.SetActive(false);
        howToPlayCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        optionsCanvas.SetActive(false);
        pauseMenuCanvas.SetActive(false);
    }

    public void ToggleResolutionOptions()
    {
        ResolutionCanvas.SetActive(!ResolutionCanvas.activeSelf);
    }

    public void ToggleFullscreen()
    {
        checkboxFullscreen.SetActive(!checkboxFullscreen.activeSelf);
    }
}