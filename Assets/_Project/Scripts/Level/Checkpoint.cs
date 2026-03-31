using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _uiText;

    [Header("Firework")]
    [SerializeField] private GameObject _fireworkPrefab; // prefab particle system
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
                AudioManager.Instance.PlayFirework(); // suono
            }

            // Suono checkpoint
            AudioManager.Instance.PlayCheckpoint();
        }
    }

    private void HideUI()
    {
        if (_uiText != null) // Se c'e' un oggetto UI assegnato
            _uiText.SetActive(false); // lo nasconde
    }
}