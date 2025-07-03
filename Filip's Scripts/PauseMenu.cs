using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    private PlayerController playerController;
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
     
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        if (playerController != null)
            playerController.canLook = true;

        Cursor.lockState = CursorLockMode.Locked; // Fix is here
        Cursor.visible = false;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

    }


    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        if (playerController != null)
            playerController.canLook = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quit button pressed!");
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        //Application.Quit();
    }
}
