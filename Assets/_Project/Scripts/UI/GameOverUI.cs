using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private LevelTimer _levelTimer;
    [SerializeField] private DoorUnlockUI _doorUnlockUI;

    [Header("Checkpoint")]
    [SerializeField] private Button _reloadCheckpointButton;

    private bool _shown = false;

    private void Start()
    {
        if (_reloadCheckpointButton != null)
            _reloadCheckpointButton.onClick.AddListener(ReloadCheckpoint);
    }

    private void Update()
    {
        if (_reloadCheckpointButton != null)
            _reloadCheckpointButton.interactable = CheckpointManager.Instance?.HasCheckpoint() ?? false;
    }

    public void Show()
    {
        if (_shown) return;
        _shown = true;

        gameObject.SetActive(true);

        _levelTimer?.StopTimer();
        _doorUnlockUI?.HideImmediately();

        AudioManager.Instance?.PlayGameOver();
        AudioManager.Instance?.StopMusic();

        RigidbodyCharacter player = FindObjectOfType<RigidbodyCharacter>();
        if (player != null)
            player.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f;
    }

    public void Hide()
    {
        _shown = false;
        gameObject.SetActive(false);
    }

    private void ReloadCheckpoint()
    {
        Time.timeScale = 1f;

        GameManager gm = FindObjectOfType<GameManager>();
        gm?.RespawnAtCheckpoint();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        AudioManager.Instance?.RestartMusicForScene();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Resetta checkpoint prima di ricaricare il livello
        CheckpointManager.Instance?.ResetCheckpoint();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Riavvia la musica dopo il reload
        AudioManager.Instance?.RestartMusicForScene();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("MainMenu");
    }
}