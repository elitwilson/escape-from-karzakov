using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Paused,
    GameOver
}

public enum PhaseState
{
    Default,
    RandomConflag,
    KarzakovConflag
}

public class GameManager : MonoBehaviour
{
    // Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager Instance { get; private set; }

    public AudioSource AudioSource;
    public GameObject Karzakov;
    public GameObject Player;
    public Conflag Conflag;
    public int DefaultPhaseSeconds = 3;
    private int runtimePhaseSeconds;
    
    public GameState CurrentGameState;
    public PhaseState CurrentPhaseState;

    //private GameState lastGameState;
    private PhaseState lastPhaseState;


    // EVENTS ------------------------------------------
    public delegate void PhaseChangeHandler(PhaseState newState);
    public event PhaseChangeHandler OnPhaseChange;

    public delegate void GameOverHandler();
    public event GameOverHandler OnGameOver;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            // Destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        runtimePhaseSeconds = DefaultPhaseSeconds;
        SwitchGameState(GameState.Playing);
        SwitchPhaseState(PhaseState.Default);
}

    // Update is called once per frame
    void Update()
    {
        if (CurrentPhaseState != lastPhaseState)
        {
            SwitchPhaseState(CurrentPhaseState);
            lastPhaseState = CurrentPhaseState;
        }
    }

    void SwitchGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Playing:
                Time.timeScale = 1;
                break;
            case GameState.Paused:
                Time.timeScale = 0;
                break;
            case GameState.GameOver:
                Debug.Log("GameManager: Game Over!");
                OnGameOver?.Invoke();
                Time.timeScale = 0;
                break;
        }
    }

    void SwitchPhaseState(PhaseState newState)
    {
        OnPhaseChange?.Invoke(newState);
        switch (newState)
        {
            case PhaseState.Default:
                // Start a timer until the next phase
                StartTimer(runtimePhaseSeconds);

                break;
            case PhaseState.RandomConflag:
                break;
            case PhaseState.KarzakovConflag:
                Debug.Log("KarzakovConflag");
                //Conflag.gameObject.SetActive(true);
                //Conflag.StartTimer();
                break;
        }
    }

    // Method to start the timer
    public void StartTimer(int seconds)
    {
        StartCoroutine(TimerCoroutine(seconds));
    }

    private IEnumerator TimerCoroutine(int seconds)
    {
        // Wait for N seconds
        yield return new WaitForSeconds(seconds);

        // Call the method after the timer finishes
        OnTimerFinished();
    }

    private void OnTimerFinished()
    {
        // Code to execute after the timer ends
        Debug.Log("Timer finished!");
        SwitchPhaseState(PhaseState.KarzakovConflag);
        // You can call any method or perform any action here
    }

    public void PlayClipIfNotPlaying(AudioClip clip)
    {
        // Check if the AudioSource is currently playing a clip
        // If not, play the provided clip
        AudioSource.clip = clip;
        AudioSource.Play();
    }

    public void TriggerGameOver()
    {
        StartCoroutine(GameOverTimer());
    }

    private IEnumerator GameOverTimer()
    {
        yield return new WaitForSeconds(1.5f);
        SwitchGameState(GameState.GameOver);
    }

    public void PlayAgain()
    {
        // Get the current scene using SceneManager.GetActiveScene()
        Scene currentScene = SceneManager.GetActiveScene();

        // Reload the scene using SceneManager.LoadScene()
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the required objects
        Karzakov = GameObject.Find("Karzakov");
        Player = GameObject.Find("Player");
        Conflag = GameObject.Find("Conflag").GetComponent<Conflag>();
        runtimePhaseSeconds = DefaultPhaseSeconds;

        SwitchPhaseState(PhaseState.Default);
        SwitchGameState(GameState.Playing);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
