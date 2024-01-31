using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    private static BackgroundMusicController instance = null;

    private AudioSource audioSource;
    public float fadeOutTime = 5.0f; // Duration of the fade out
    private float maxVolume;

    void Awake()
    {
        if (instance == null)
        {
            // This is the first instance - make it the Singleton
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (this != instance)
        {
            // This is not the first instance - destroy it
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject); // Keep the GameObject across scenes

        audioSource = GetComponent<AudioSource>();
        maxVolume = audioSource.volume;
        audioSource.Play();
    }

    void Update()
    {
        // Check if the music is about to end
        if (audioSource.clip.length - audioSource.time <= fadeOutTime)
        {
            // Fade out the music
            audioSource.volume = Mathf.Lerp(maxVolume, 0, (audioSource.clip.length - audioSource.time) / fadeOutTime);
        }
        else
        {
            // Ensure full volume is set when not fading out
            audioSource.volume = maxVolume;
        }

        // Loop the music manually
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
