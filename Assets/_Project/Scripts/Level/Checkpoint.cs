using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject _uiText; // es. "Checkpoint attivato!"
    private bool _activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_activated) return;
        if (!other.CompareTag("Player")) return;

        PlayerCoinCollector playerCoins = other.GetComponent<PlayerCoinCollector>();
        if (playerCoins != null)
        {
            // Salva checkpoint: posizione e monete correnti
            CheckpointManager.Instance.ActivateCheckpoint(transform.position, playerCoins.GetCoins());

            _activated = true;

            // Suono checkpoint
            AudioManager.Instance?.PlayCheckpoint();

            // Mostra UI breve
            if (_uiText != null)
            {
                _uiText.SetActive(true);
                Invoke(nameof(HideUI), 2f); // scompare dopo 2 secondi
            }
        }
    }

    private void HideUI()
    {
        if (_uiText != null)
            _uiText.SetActive(false);
    }
}