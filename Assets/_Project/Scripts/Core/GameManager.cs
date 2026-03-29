using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerCoinCollector _player;
    [SerializeField] private VictoryUI _victoryUI;
    [SerializeField] private int _requiredCoins = 100; // numero minimo per vittoria

    public void FinishLevel() // Metodo chiamato quando il livello è completato
    {
        if (_victoryUI != null && _player != null) // Controlla che ci siano riferimenti validi a VictoryUI e PlayerCoinCollector
        {
            _victoryUI.ShowVictory(_player, _requiredCoins);             // Chiama la UI per mostrare il pannello di vittoria passando il player e il numero minimo di monete richieste
        }
    }
}
