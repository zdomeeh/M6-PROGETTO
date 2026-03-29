using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private int _requiredCoins = 100;
    [SerializeField] private VictoryUI _victoryUI;

    private void OnTriggerEnter(Collider other)
    {
        PlayerCoinCollector collector = other.GetComponent<PlayerCoinCollector>(); // Controlla se l'oggetto che entra ha un PlayerCoinCollector
        if (collector == null)
            return;

        if (collector.GetCoins() >= _requiredCoins) // Se il player ha raccolto abbastanza monete
        {
            _victoryUI.ShowVictory(collector, _requiredCoins); // Mostra il pannello di vittoria passando il player e il numero minimo di monete richieste
        }
        else
        {
            Debug.Log("Non hai abbastanza monete");
        }
    }
}
