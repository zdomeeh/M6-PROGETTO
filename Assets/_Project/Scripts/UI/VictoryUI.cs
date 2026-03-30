using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _panelVictory;
    [SerializeField] private GameObject _panelPerfectVictory;

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _completionText;

    [Header("Stars")]
    [SerializeField] private GameObject _normalStar;
    [SerializeField] private GameObject _perfectStar;
    [SerializeField] private CanvasGroup _perfectStarCanvas;
    [SerializeField] private float _starFadeDuration = 1f;

    [Header("Fireworks")]
    [SerializeField] private GameObject _fireworkPrefab; // prefab particle system
    [SerializeField] private Transform[] _fireworkSpawnPoints; // punti diversi per 3 fireworks finale
    [SerializeField] private float _fireworkDuration = 2f; // durata in secondi dei firework prima del pannello

    public void ShowVictory(PlayerCoinCollector collector, int requiredCoins)
    {
        if (collector == null) return;

        int collected = collector.GetCoins();
        int total = collector.TotalCoinsInLevel;

        bool perfectRun = collected >= total;
        bool normalVictory = collected >= requiredCoins && collected < total;

        if (_coinsText != null)
            _coinsText.text = collected + " / " + total + " coins";

        if (_normalStar != null) _normalStar.SetActive(false);
        if (_perfectStar != null)
        {
            _perfectStar.SetActive(true);
            if (_perfectStarCanvas != null)
                _perfectStarCanvas.alpha = 0f;
        }

        // Avvia coroutine che gestisce delay e pannello
        StartCoroutine(ShowVictoryRoutine(perfectRun, normalVictory));
    }

    private IEnumerator ShowVictoryRoutine(bool perfectRun, bool normalVictory)
    {
        // Avvio fuochi d'artificio
        if (_fireworkPrefab != null && _fireworkSpawnPoints != null)
        {
            foreach (Transform spawn in _fireworkSpawnPoints)
            {
                if (spawn != null)
                {
                    Instantiate(_fireworkPrefab, spawn.position, Quaternion.identity);
                    AudioManager.Instance?.PlayFirework();
                }
            }
        }

        // Aspetta la durata dei firework prima di mostrare il pannello
        yield return new WaitForSecondsRealtime(_fireworkDuration);

        // Mostra pannelli e stelle
        if (perfectRun)
        {
            _panelVictory?.SetActive(false);
            _panelPerfectVictory?.SetActive(true);

            if (_completionText != null)
                _completionText.text = "110% Completato";

            _normalStar?.SetActive(true);

            if (_perfectStar != null)
            {
                CanvasGroup cg = _perfectStar.GetComponent<CanvasGroup>();
                if (cg == null) cg = _perfectStar.AddComponent<CanvasGroup>();
                cg.alpha = 0f;
                StartCoroutine(FadeInPerfectStar(cg));
            }
        }
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
        else
        {
            Debug.Log("Non hai abbastanza monete per aprire la porta!");
            yield break;
        }

        // Blocca player e pausa gioco
        RigidbodyCharacter player = FindObjectOfType<RigidbodyCharacter>();
        if (player != null)
            player.enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
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
        canvasGroup.alpha = 1f;
        AudioManager.Instance?.PlayPerfectVictory();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
    }
}