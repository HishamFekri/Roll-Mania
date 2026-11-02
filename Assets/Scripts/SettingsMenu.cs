using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource playerAudioSource; // PSound AudioSource attached to player
    public Toggle muteToggle;
    public Slider volumeSlider;
    
    private void Start()
    {
        // Find PSound AudioSource if not assigned
        if (playerAudioSource == null)
        {
            GameObject psound = GameObject.Find("PSound");
            if (psound != null)
            {
                playerAudioSource = psound.GetComponent<AudioSource>();
            }
        }
        
        // Initialize UI elements
        if (muteToggle != null && playerAudioSource != null)
        {
            muteToggle.isOn = !playerAudioSource.mute;
            muteToggle.onValueChanged.AddListener(OnMuteToggleChanged);
        }
        
        if (volumeSlider != null && playerAudioSource != null)
        {
            volumeSlider.value = playerAudioSource.volume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
    }
    
    public void OnMuteToggleChanged(bool isMuted)
    {
        if (playerAudioSource != null)
        {
            // Toggle is inverted: when toggle is ON, audio is NOT muted
            playerAudioSource.mute = !isMuted;
        }
    }
    
    public void OnVolumeChanged(float volume)
    {
        if (playerAudioSource != null)
        {
            playerAudioSource.volume = volume;
        }
    }
}
