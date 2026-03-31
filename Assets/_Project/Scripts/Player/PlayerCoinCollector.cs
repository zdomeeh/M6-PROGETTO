using UnityEngine;
using UnityEngine.Events;

public class PlayerCoinCollector : MonoBehaviour
{
    [SerializeField] private int _totalCoinsInLevel = 110;

    private int _coins = 0;

    // Evento che passa il numero aggiornato di monete
    public UnityEvent<int> OnCoinsChanged;

    // Aggiunge monete al contatore
    public void AddCoins(int value)
    {
        _coins += value;

            AudioManager.Instance?.PlayPlayerCoin();

        OnCoinsChanged?.Invoke(_coins);
    }

    // Imposta le monete (utile per respawn)
    public void SetCoins(int value)
    {
        _coins = value;
        OnCoinsChanged?.Invoke(_coins);
    }

    public int GetCoins() => _coins;
    public int TotalCoinsInLevel => _totalCoinsInLevel;

    public float GetCompletionPercentage()
    {
        if (_totalCoinsInLevel <= 0) return 0f;
        return (_coins / (float)_totalCoinsInLevel) * 100f;
    }
}