using UnityEngine;
using UnityEngine.Events;

public class PlayerCoinCollector : MonoBehaviour
{
    [SerializeField] private int _totalCoinsInLevel = 110;
    [SerializeField] private PlayerAudio _playerAudio;

    private int _coins = 0;

    // Evento che passa il numero aggiornato di monete
    public UnityEvent<int> OnCoinsChanged;

    // Aggiunge monete al contatore
    public void AddCoins(int value)
    {
        _coins += value;

        if (_playerAudio != null)
        {
            _playerAudio.PlayCoin(); // suono raccolta moneta
        }

        OnCoinsChanged?.Invoke(_coins);
    }

    // Ritorna quante monete ha raccolto il player
    public int GetCoins() => _coins;

    // Ritorna il numero totale di monete nel livello
    public int TotalCoinsInLevel => _totalCoinsInLevel;

    // Calcola la percentuale di completamento del livello basata sulle monete raccolte
    public float GetCompletionPercentage()
    {
        if (_totalCoinsInLevel <= 0) // Evita divisione per zero
            return 0f;

        return (_coins / (float)_totalCoinsInLevel) * 100f; // Percentuale raccolta
    }
}
