using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Conflag : MonoBehaviour
{
    public float timeRemaining = 3f; // Start time in seconds
    public TextMeshPro timerText; // Reference to the TextMeshProUGUI component
    public bool TimerIsRunning = false;
    public AudioClip[] Clips;
    public GameObject Renderer;
    public GameObject Explosion;

    private int lastSecond = -1;

    private void Start()
    {
        GameManager.Instance.OnPhaseChange += HandlePhaseChange;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnPhaseChange -= HandlePhaseChange;
    }

    private void Update()
    {
        if (TimerIsRunning)
        {
            if (timeRemaining > 0)
            {
                // Decrease the time remaining
                timeRemaining -= Time.deltaTime;

                // Update the TextMeshPro text
                int currentSecond = Mathf.CeilToInt(timeRemaining);
                timerText.text = currentSecond.ToString();

                // Check if we've passed into a new second
                if (lastSecond != currentSecond)
                {
                    lastSecond = currentSecond;
                    GameManager.Instance.PlayClipIfNotPlaying(Clips[currentSecond]);
                }
            }
            else
            {
                // When the countdown is over, you can set the text to "0" or do something else
                timerText.text = "0";
                // Disable or destroy the script if you don't need it anymore
                // Destroy(this);
                // Or disable this GameObject
                // gameObject.SetActive(false);
                if (lastSecond != 0)
                {
                    GameManager.Instance.PlayClipIfNotPlaying(Clips[0]);
                    lastSecond = 0;
                }
                Explosion.SetActive(true);
                TimerIsRunning = false;
                GameManager.Instance.TriggerGameOver();
            }
        }
    }

    public void StartTimer()
    {
        if (TimerIsRunning)
        {
            // If the timer is already running, do nothing
            return;
        }

        // Start the timer
        TimerIsRunning = true;
    }

    // Method to handle the event
    private void HandlePhaseChange(PhaseState newPhase)
    {
        // Handle the phase change here
        Debug.Log("Phase changed to: " + newPhase);
        Renderer.SetActive(true);
        StartTimer();
    }
}
