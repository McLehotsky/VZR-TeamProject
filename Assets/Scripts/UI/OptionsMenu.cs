using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    [Header("UI Referencie")]
    public Slider volumeSlider;
    public Slider sensitivitySlider;
    // public Toggle fullscreenToggle; // Tvoj checkbox "X"

    // Data save keys
    private const string PREF_VOLUME = "volume";
    private const string PREF_SENSITIVITY = "sensitivity";
    private const string PREF_FULLSCREEN = "fullscreen";

    void Start()
    {
        // 1. Nacitanie a nastavenie Hlasitosti
        // Default hodnota je 1 (max), ak nic nie je ulozene
        float savedVolume = PlayerPrefs.GetFloat(PREF_VOLUME, 1f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume; 

        // 2. Nacitanie a nastavenie Fullscreenu
        // 1 = true, 0 = false. Default je 1 (fullscreen)
        bool isFullscreen = PlayerPrefs.GetInt(PREF_FULLSCREEN, 1) == 1;
        Screen.fullScreen = isFullscreen;

        // 3. Nacitanie a nastavenie Senzitivity
        // Default napr. 1.0f (alebo ina hodnota podla tvojej hry)
        float savedSens = PlayerPrefs.GetFloat(PREF_SENSITIVITY, 1.0f);
        sensitivitySlider.value = savedSens;
        
        // Poznámka: Senzitivitu musi citat aj tvoj skript na pohyb kamery!
    }

    // --- HLASITOSŤ ---
    // Tuto funkciu priradis na Slider -> OnValueChanged
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; // Najjednoduchsi sposob (ovlada globalnu hlasitost)
        PlayerPrefs.SetFloat(PREF_VOLUME, volume);
        PlayerPrefs.Save();
    }

    // --- FULLSCREEN ---
    // Tuto funkciu priradis na Toggle -> OnValueChanged
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt(PREF_FULLSCREEN, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    // --- ROZLÍŠENIE ---
    // Tieto funkcie priradis na jednotlive tlacidla (Button -> OnClick)
    public void SetResolution2560()
    {
        Screen.SetResolution(2560, 1440, Screen.fullScreen);
        // Volitelne: Zatvor panel s rozlisenim
    }

    public void SetResolution1920()
    {
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
    }

    public void SetResolution1270()
    {
        // Na obrazku mas 1270, ale standard je 1280. Necham podla obrazka.
        Screen.SetResolution(1270, 720, Screen.fullScreen);
    }

    // --- SENZITIVITA MYŠI ---
    // Tuto funkciu priradis na Slider -> OnValueChanged
    public void SetSensitivity(float sensitivity)
    {
        // Ulozime do PlayerPrefs, aby si to tvoj PlayerController mohol precitat
        PlayerPrefs.SetFloat(PREF_SENSITIVITY, sensitivity);
        PlayerPrefs.Save();
        
        // Ak mas pristup k Player skriptu priamo, mozes to aktualizovat aj tu.
        // Napr: PlayerController.instance.mouseSensitivity = sensitivity;
    }
}