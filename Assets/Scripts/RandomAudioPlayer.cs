using UnityEngine;
using System.Collections; // Required for using coroutines

/// <summary>
/// This script attaches to a GameObject with an AudioSource component.
/// It continuously plays and pauses the audio clip with randomized durations.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class RandomAudioPlayer : MonoBehaviour
{
    [Header("Play Interval Settings")]
    [Tooltip("The average time in seconds the audio will play before pausing.")]
    [SerializeField] private float meanPlayTime = 5.0f;

    [Tooltip("The random variation (+/-) from the mean play time.")]
    [SerializeField] private float playTimeRange = 2.0f;

    [Header("Pause Interval Settings")]
    [Tooltip("The average time in seconds the audio will stay paused.")]
    [SerializeField] private float meanPauseTime = 3.0f;

    [Tooltip("The random variation (+/-) from the mean pause time.")]
    [SerializeField] private float pauseTimeRange = 1.0f;

    // Private reference to the AudioSource component on this GameObject
    private AudioSource audioSource;

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Disable 'Play On Awake' so this script has full control.
        audioSource.playOnAwake = false;
    }

    /// <summary>
    /// Called on the frame when a script is enabled, just before any of the Update methods are called.
    /// </summary>
    void Start()
    {
        // Start the repeating play/pause cycle.
        StartCoroutine(PlayPauseCycle());
    }

    /// <summary>
    /// A coroutine that handles the logic for playing and pausing the audio source.
    /// </summary>
    private IEnumerator PlayPauseCycle()
    {
        // Start the audio clip from the beginning.
        audioSource.Play();

        // Use an infinite loop to make the cycle repeat forever.
        while (true)
        {
            // --- PLAYING STATE ---
            // Calculate a random duration for the audio to play.
            float playDuration = meanPlayTime + Random.Range(-playTimeRange, playTimeRange);
            // Ensure the duration is never a negative number.
            playDuration = Mathf.Max(0f, playDuration);

            // Wait for that amount of time.
            yield return new WaitForSeconds(playDuration);

            // --- PAUSED STATE ---
            // Pause the audio if it's currently playing.
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }

            // Calculate a random duration for the audio to be paused.
            float pauseDuration = meanPauseTime + Random.Range(-pauseTimeRange, pauseTimeRange);
            pauseDuration = Mathf.Max(0f, pauseDuration);

            // Wait for that amount of time.
            yield return new WaitForSeconds(pauseDuration);

            // Unpause the audio to resume playback from where it left off.
            audioSource.UnPause();
        }
    }
}