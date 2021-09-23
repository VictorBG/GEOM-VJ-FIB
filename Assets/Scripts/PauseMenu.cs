using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup pauseMenu;
    private bool _paused;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        _paused = false;
        pauseMenu.alpha = 0f;
        pauseMenu.blocksRaycasts = false;
        pauseMenu.interactable = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    private void Pause()
    {
        if (_paused)
        {
            Resume();
            return;
        }

        GameManager.Instance.Paused();

        if (SceneManager.GetActiveScene().buildIndex <= 1)
        {
            return;
        }

        _paused = true;
        Time.timeScale = 0f;
        pauseMenu.alpha = 1f;
        pauseMenu.blocksRaycasts = true;
        pauseMenu.interactable = true;
    }

    public void Resume()
    {
        if (_paused)
        {
            GameManager.Instance.Playing();
            _paused = false;
            pauseMenu.alpha = 0f;
            pauseMenu.blocksRaycasts = false;
            pauseMenu.interactable = false;
            Time.timeScale = 1f;
        }
    }

    public void Menu()
    {
        if (_paused)
        {
            _paused = false;
            pauseMenu.alpha = 0f;
            pauseMenu.blocksRaycasts = false;
            pauseMenu.interactable = false;
            Time.timeScale = 1f;
            SceneManager.LoadScene(1);
        }
    }
}