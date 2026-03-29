using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private LevelTimer _levelTimer; // riferimento per il timer
    [SerializeField] private DoorUnlockUI _doorUnlockUI;

    private bool _shown = false; // per evitare suoni multipli

    // Mostra il pannello di Game Over
    public void Show()
    {
        if (_shown) return; // no suoni multipli
        _shown = true;

        gameObject.SetActive(true);

        if (_levelTimer != null)
            _levelTimer.StopTimer(); // blocca il timer quando muori

        if (_doorUnlockUI != null)
            _doorUnlockUI.HideImmediately(); // nasconde la scritta "Porta sbloccata!"

        AudioManager.Instance?.PlayGameOver();

        // Disabilita il controllo del player
        RigidbodyCharacter player = FindObjectOfType<RigidbodyCharacter>();
        if (player != null)
            player.enabled = false;

        // Sblocca e mostra il cursore per poter cliccare i pulsanti
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f; // pausa il gioco
    }

    // Riavvia il livello corrente
    public void RestartLevel()
    {
        Time.timeScale = 1f;

        // Nasconde e blocca il cursore per il gameplay
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Torna al menu principale
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        Cursor.visible = true; // visibile
        Cursor.lockState = CursorLockMode.None; // sbloccato

        SceneManager.LoadScene("MainMenu");
    }
}