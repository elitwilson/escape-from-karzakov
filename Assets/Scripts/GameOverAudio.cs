using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverAudio : MonoBehaviour
{
    public static GameOverAudio Instance = null;

    public AudioClip[] gameOverClips; // Array to hold game over clips
    private AudioSource audioSource;
    private int currentIndex = 0;

    private void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GameManager.Instance.OnGameOver += PlayRandomGameOverClip;
        ShuffleClips();
    }

    void OnDestroy()
    {
        GameManager.Instance.OnGameOver -= PlayRandomGameOverClip;
    }

    // Shuffle the clips array
    private void ShuffleClips()
    {
        for (int i = 0; i < gameOverClips.Length; i++)
        {
            int rnd = Random.Range(i, gameOverClips.Length);
            AudioClip temp = gameOverClips[rnd];
            gameOverClips[rnd] = gameOverClips[i];
            gameOverClips[i] = temp;
        }
    }

    // Play the next clip in the shuffled array
    public void PlayRandomGameOverClip()
    {
        if (gameOverClips.Length == 0) return; // No clips to play

        audioSource.clip = gameOverClips[currentIndex];
        audioSource.Play();

        // Move to the next clip, reshuffle and reset if at the end of the array
        if (++currentIndex >= gameOverClips.Length)
        {
            ShuffleClips();
            currentIndex = 0;
        }
    }
}
