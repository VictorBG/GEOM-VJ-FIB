using Animations;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private LevelLoader _levelLoader;

    private void Awake()
    {
        _levelLoader = GetComponent<LevelLoader>();
    }

    public void OnPlay()
    {
        _levelLoader.Load(2);
    }
    
    public void OnCredits()
    {
        _levelLoader.Load(7);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}