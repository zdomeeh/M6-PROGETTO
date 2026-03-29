using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
    [SerializeField] private GameObject _panelVictory;
    [SerializeField] private GameObject _panelPerfectVictory;

    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _completionText;

    [SerializeField] private GameObject _normalStar;   // stella grigia
    [SerializeField] private GameObject _perfectStar;  // stella gialla
    [SerializeField] private CanvasGroup _perfectStarCanvas; // canvas per fade della stella

    [SerializeField] private DoorUnlockUI _doorUnlockUI;

    [SerializeField] private float _starFadeDuration = 1f; // durata fade stella perfetta

    public void ShowVictory(PlayerCoinCollector collector, int requiredCoins) // Mostra il pannello di vittoria corretto in base al numero di monete raccolte
    {
        if (collector == null) return;

        if (_doorUnlockUI != null) // nasconde il testo "Porta sbloccata!"
            _doorUnlockUI.HideImmediately();

        int collected = collector.GetCoins(); // Monete raccolte dal player
        int total = collector.TotalCoinsInLevel; // Monete totali nel livello

        bool perfectRun = collected >= total; // Determina se la run è perfetta o normale
        bool normalVictory = collected >= requiredCoins && collected < total;

        if (_coinsText != null)
            _coinsText.text = collected + " / " + total + " coins";

        // RESET STAR
        if (_normalStar != null) _normalStar.SetActive(false);
        if (_perfectStar != null)
        {
            _perfectStar.SetActive(true);
            if (_perfectStarCanvas != null) _perfectStarCanvas.alpha = 0f; // parte invisibile
        }

        // PERFECT RUN
        if (perfectRun)
        {
            _panelVictory?.SetActive(false);
            _panelPerfectVictory?.SetActive(true);

            if (_completionText != null)
                _completionText.text = "110% Completato";

            _normalStar?.SetActive(true); // Mostra subito la stella normale

            // Imposta alpha 0 per la stella perfetta e avvia il fade
            if (_perfectStar != null)
            {
                _perfectStar.SetActive(true);
                CanvasGroup cg = _perfectStar.GetComponent<CanvasGroup>();
                if (cg == null)
                    cg = _perfectStar.AddComponent<CanvasGroup>();

                cg.alpha = 0f;
                StartCoroutine(FadeInPerfectStar(cg));
            }
        }
        // VITTORIA NORMALE
        else if (normalVictory)
        {
            _panelVictory?.SetActive(true);
            _panelPerfectVictory?.SetActive(false);

            if (_completionText != null)
                _completionText.text = "100% Completato";

            _normalStar?.SetActive(true);
            _perfectStar?.SetActive(false);

            AudioManager.Instance?.PlayVictory();
        }
        // Caso in cui il player non ha il minimo delle monete richiesto
        else
        {
            Debug.Log("Non hai abbastanza monete per aprire la porta!");
            return;
        }

        // Disabilita il controllo del player
        RigidbodyCharacter player = FindObjectOfType<RigidbodyCharacter>();
        if (player != null)
            player.enabled = false;

        // Mostra e sblocca il cursore
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f; // pausa il gioco
    }

    private IEnumerator FadeInPerfectStar(CanvasGroup canvasGroup)
    {
        float elapsed = 0f;
        while (elapsed < _starFadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / _starFadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f; // assicura alpha finale

        // Suono sparkle
        AudioManager.Instance?.PlayPerfectVictory();
    }

    public void GoToMainMenu() // Torna al menu principale ripristinando il tempo
    {
        Time.timeScale = 1f;

        Cursor.visible = true;           // visibile
        Cursor.lockState = CursorLockMode.None; // sbloccato

        SceneManager.LoadScene("MainMenu");
    }
}