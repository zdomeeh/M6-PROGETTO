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
        // Collega il bottone al metodo di respawn
        if (_reloadCheckpointButton != null)
            _reloadCheckpointButton.onClick.AddListener(ReloadCheckpoint);
    }

    private void Update()
    {
        // Attiva il bottone solo se esiste un checkpoint
        if (_reloadCheckpointButton != null)
            _reloadCheckpointButton.interactable = CheckpointManager.Instance?.HasCheckpoint() ?? false;
    }

    public void Show()
    {
        if (_shown) return; // evita di mostrare la schermata piu' volte
        _shown = true;

        gameObject.SetActive(true); // mostra UI Game Over

        _levelTimer?.StopTimer(); // ferma il timer
        _doorUnlockUI?.HideImmediately(); // nasconde messaggio porta

        AudioManager.Instance?.PlayGameOver(); // riproduce suono game over
        AudioManager.Instance?.StopMusic(); // ferma musica

        RigidbodyCharacter player = FindObjectOfType<RigidbodyCharacter>();
        if (player != null)
            player.enabled = false; // blocca il player

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // mostra il mouse

        Time.timeScale = 0f; // mette in pausa il gioco
    }

    public void Hide()
    {
        _shown = false;
        gameObject.SetActive(false); // nasconde UI Game Over
    }

    private void ReloadCheckpoint()
    {
        Time.timeScale = 1f; // riattiva il gioco

        GameManager gm = FindObjectOfType<GameManager>();
        gm?.RespawnAtCheckpoint(); // respawn dal checkpoint

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // nasconde il mouse

        AudioManager.Instance?.RestartMusicForScene(); // riavvia la musica
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Cancella il checkpoint prima di ricominciare il livello
        CheckpointManager.Instance?.ResetCheckpoint();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ricarica il livello
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("MainMenu"); // torna al menu principale
    }
}