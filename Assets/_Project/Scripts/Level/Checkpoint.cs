using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _uiText; // es. "Checkpoint attivato!"

    [Header("Firework")]
    [SerializeField] private GameObject _fireworkPrefab; // prefab particle system per effetto
    [SerializeField] private Transform _fireworkSpawnPoint; // punto da cui far partire il firework

    private bool _activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_activated) return;
        if (!other.CompareTag("Player")) return;

        // Salva checkpoint: posizione e monete correnti
        PlayerCoinCollector playerCoins = other.GetComponent<PlayerCoinCollector>();
        if (playerCoins != null)
        {
            CheckpointManager.Instance.ActivateCheckpoint(transform.position, playerCoins.GetCoins());

            _activated = true;

            // Mostra UI breve
            if (_uiText != null)
            {
                _uiText.SetActive(true);
                Invoke(nameof(HideUI), 2f); // scompare dopo 2 secondi
            }

            // Firework
            if (_fireworkPrefab != null && _fireworkSpawnPoint != null)
            {
                Instantiate(_fireworkPrefab, _fireworkSpawnPoint.position, Quaternion.identity);
                AudioManager.Instance.PlayFirework(); // suono centralizzato
            }

            // Suono checkpoint
            AudioManager.Instance.PlayCheckpoint();
        }
    }

    private void HideUI()
    {
        if (_uiText != null)
            _uiText.SetActive(false);
    }
}