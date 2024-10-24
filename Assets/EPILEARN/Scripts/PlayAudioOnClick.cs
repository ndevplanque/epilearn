using UnityEngine;
using UnityEngine.UI;

public class PlayAudioOnClick : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the Audio Source
    public AudioClip audioClip; // Reference to the Audio Clip
    public Button playButton; // Reference to the Button

    private void Start()
    {
        // Assign the AudioClip to the AudioSource if it's not already assigned
        if (audioSource != null && audioClip != null)
            audioSource.clip = audioClip; // Manually set the AudioClip in the script

        // Assign the PlayAudio function to the Button's onClick event
        if (playButton != null) playButton.onClick.AddListener(PlayAudio);
    }

    private void PlayAudio()
    {
        if (audioSource != null &&
            !audioSource.isPlaying) audioSource.Play(); // Play the audio when the button is clicked
    }
}