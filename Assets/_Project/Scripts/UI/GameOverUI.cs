using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private LevelTimer _levelTimer;
    [SerializeField] private DoorUnlockUI _doorUnlockUI;

    [Header("Checkpoint")]
    [SerializeField] private Button _reloadCheckpointButton; // pulsante UI reload checkpoint

    private bool _shown = false;

    private void Start()
    {
        if (_reloadCheckpointButton != null)
            _reloadCheckpointButton.onClick.AddListener(ReloadCheckpoint);
    }

    public void Show()
    {
        if (_shown) return;
        _shown = true;

        gameObject.SetActive(true);

        if (_levelTimer != null)
            _levelTimer.StopTimer();

        if (_doorUnlockUI != null)
            _doorUnlockUI.HideImmediately();

        AudioManager.Instance?.PlayGameOver();

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

    // Reload checkpoint invece di restart livello
    private void ReloadCheckpoint()
    {
        Time.timeScale = 1f;

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.RespawnAtCheckpoint();
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("MainMenu");
    }
}