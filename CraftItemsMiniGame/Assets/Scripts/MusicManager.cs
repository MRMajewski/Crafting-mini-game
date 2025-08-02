using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private Button musicToggleButton;
    [SerializeField] private Image musicToggleImage;
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;
    [SerializeField] private List<SoundEntry> soundEntries;
    [SerializeField] private Slider volumeSlider;

    private Dictionary<string, AudioClip> soundEffects = new Dictionary<string, AudioClip>();
    private bool isPlaying = true;

    private bool isSliderOn = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        AddSoundsToDictionary(soundEntries);

        UpdateButtonIcon();

        SetVolume(.3f);
        volumeSlider.value = musicSource.volume;

        void AddSoundsToDictionary(List<SoundEntry> soundEntries)
        {
            foreach (var entry in soundEntries)
            {
                if (!soundEffects.ContainsKey(entry.name))
                {
                    soundEffects.Add(entry.name, entry.clip);
                }
                else
                {
                    Debug.LogWarning($"Duplikat nazwy dŸwiêku: {entry.name}");
                }
            }
        }


    }

    public void SetVolume(float value)
    {
        musicSource.volume = value;
        sfxSource.volume = value;

        if (value == 0f)
            ToggleMusic(false);
        else
            ToggleMusic(true);
    }

    public void ToggleMusic(bool isPlaying)
    {
        this.isPlaying = isPlaying;
        UpdateButtonIcon();
    }

    public void ToggleSlider()
    {
        isSliderOn = !isSliderOn;
        volumeSlider.gameObject.SetActive(isSliderOn);
    }
    public void PlaySound(string soundName)
    {
        if (soundEffects.TryGetValue(soundName, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Nie znaleziono dŸwiêku: {soundName}");
        }
    }

    private void UpdateButtonIcon()
    {
       musicToggleImage.sprite = isPlaying ? musicOnSprite : musicOffSprite;
    }

    public void PlayPanelTransitionSFX()
    {
        PlaySound("TransitionSFX");
    }

    [System.Serializable]
    public class SoundEntry
    {
        public string name;
        public AudioClip clip;
    }
}

public static class SoundNames
{
    public const string Theme = "theme";
    public const string Click = "UIClick";
    public const string Error = "error";
    public const string Fanfair = "fanfair";
    public const string Rideoff = "rideoff";
    public const string Woosh = "woosh";
    public const string Kick = "kick";
    public const string Pick = "pick";
    public const string ConfirmUI = "confirmUI";
}


