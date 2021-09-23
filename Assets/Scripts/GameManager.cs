using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameStatus
    {
        MENU,
        PLAYING,
        PAUSED,
        PLAYER_DEAD,
        LEVEL_PASSED
    }

    public interface IGameStatus
    {
        void OnStatusChanged(GameStatus oldStatus, GameStatus newStatus);
    }

    public static GameManager Instance;

    [SerializeField] private CanvasGroup endLevelCanvas;
    [SerializeField] private AudioSource endLevelAudio;
    public bool playerAlive;

    public AudioClip keyAudio;
    public AudioClip shieldAppearAudio;
    public AudioClip shieldHitAudio;
    public AudioClip shotAudio;
    public AudioClip electricityAudio;

    public GameStatus CurrentGameStatus;
    private Dictionary<int, IGameStatus> _subscribers;

    public void SubscribeStatusChange(int id, IGameStatus subscription)
    {
        if (!_subscribers.ContainsKey(id))
        {
            _subscribers[id] = subscription;
        }
    }

    public void UnsubscribeStatusChange(int id)
    {
        _subscribers.Remove(id);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        _subscribers = new Dictionary<int, IGameStatus>();
        CurrentGameStatus = GameStatus.MENU;
    }

    private void Start()
    {
        Instance.Playing();
        endLevelCanvas.alpha = 0;
        endLevelCanvas.blocksRaycasts = false;
        endLevelCanvas.interactable = false;
        CurrentGameStatus = GameStatus.MENU;
        SceneManager.LoadScene(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            EndLevel();
        }
    }

    public void EndLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            SceneManager.LoadScene(7);
            return;
        }

        LevelPassed();
        Time.timeScale = 0f;
        endLevelCanvas.alpha = 1f;
        endLevelCanvas.blocksRaycasts = true;
        endLevelCanvas.interactable = true;
        endLevelAudio.Play();
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            Menu();
            return;
        }

        Time.timeScale = 1f;
        endLevelCanvas.alpha = 0f;
        endLevelCanvas.blocksRaycasts = false;
        endLevelCanvas.interactable = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        endLevelCanvas.alpha = 0f;
        endLevelCanvas.blocksRaycasts = false;
        endLevelCanvas.interactable = false;
        SceneManager.LoadScene(1);
    }

    public void Paused()
    {
        SetNewStatus(GameStatus.PAUSED);
    }

    public void PlayerDead()
    {
        playerAlive = false;
        SetNewStatus(GameStatus.PLAYER_DEAD);
    }

    public void Playing()
    {
        playerAlive = true;
        SetNewStatus(GameStatus.PLAYING);
    }

    public void LevelPassed()
    {
        playerAlive = true;
        SetNewStatus(GameStatus.LEVEL_PASSED);
    }

    private void SetNewStatus(GameStatus status)
    {
        if (CurrentGameStatus == status)
        {
            return;
        }

        foreach (var subscription in _subscribers)
        {
            subscription.Value?.OnStatusChanged(CurrentGameStatus, status);
        }

        CurrentGameStatus = status;
    }
}