using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Button resumeButton;
    public Button quitButton;

    public Button pauseButton;

    public Canvas canvas;

    [HideInInspector] public bool isGamePaused = false;

    private bool isPaused = false;

    void Start()
    {
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        if (isPaused == false && canvas != null && canvas.gameObject.activeSelf)
        {
            canvas.gameObject.SetActive(false);
        }
    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        
        if (isPaused == false)
        {
            isGamePaused = false;
            if (canvas != null)
                canvas.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        else if (isPaused == true)
        {
            isGamePaused = true;
            if (canvas != null)
                canvas.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void OnPauseButtonPushed()
    {
        TogglePauseMenu();
    }

    public void OnHomeButton()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isGamePaused = false;
        SceneManager.LoadScene(0);
    }
}
