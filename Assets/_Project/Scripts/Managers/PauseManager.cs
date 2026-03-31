using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    [Header("Player")]
    [SerializeField] private RigidbodyCharacter _player;

    private bool _isPaused = false;

    private void Start()
    {
        // Imposta i valori iniziali degli slider dal AudioManager
        if (_musicSlider != null && AudioManager.Instance != null)
        {
            _musicSlider.value = AudioManager.Instance.MusicVolume;
            _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (_sfxSlider != null && AudioManager.Instance != null)
        {
            _sfxSlider.value = AudioManager.Instance.SFXVolume;
            _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        // Nasconde il pannello di pausa all'inizio
        if (_pausePanel != null)
            _pausePanel.SetActive(false);
    }

    private void Update()
    {
        // Premi ESC per aprire/chiudere pausa
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused) ResumeGame();
            else PauseGame();
        }
    }

    // --- Pause/Resume pubblici per Button ---
    public void PauseGame()
    {
        _isPaused = true;

        if (_pausePanel != null)
            _pausePanel.SetActive(true);

        Time.timeScale = 0f; // blocca il gioco

        if (_player != null)
            _player.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        _isPaused = false;

        if (_pausePanel != null)
            _pausePanel.SetActive(false);

        Time.timeScale = 1f; // riprendi gioco

        if (_player != null)
            _player.enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // --- Metodi slider pubblici ---
    public void SetMusicVolume(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicVolume(value);
    }

    public void SetSFXVolume(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetSFXVolume(value);
    }
}