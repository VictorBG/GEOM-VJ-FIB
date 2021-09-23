using UnityEngine;
using UnityEngine.SceneManagement;

public class MainAudioManager : MonoBehaviour, GameManager.IGameStatus
{
    private AudioSource _audioSource;

    // Start is called before the first frame update
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        GameManager.Instance.SubscribeStatusChange(SceneManager.GetActiveScene().buildIndex, this);
    }

    private void OnDestroy()
    {
        GameManager.Instance.UnsubscribeStatusChange(SceneManager.GetActiveScene().buildIndex);
    }


    public void OnStatusChanged(GameManager.GameStatus oldStatus, GameManager.GameStatus newStatus)
    {
        if (newStatus == GameManager.GameStatus.PAUSED)
        {
            _audioSource.Pause();
        }

        if (oldStatus == GameManager.GameStatus.PAUSED && newStatus == GameManager.GameStatus.PLAYING)
        {
            _audioSource.UnPause();
        }

        if (oldStatus == GameManager.GameStatus.PLAYER_DEAD && newStatus == GameManager.GameStatus.PLAYING)
        {
            _audioSource.Play();
        }

        if (newStatus == GameManager.GameStatus.PLAYER_DEAD || newStatus == GameManager.GameStatus.LEVEL_PASSED)
        {
            _audioSource.Stop();
        }
    }
}