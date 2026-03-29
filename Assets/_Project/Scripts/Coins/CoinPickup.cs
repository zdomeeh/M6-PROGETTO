using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int _value = 1;

    private void OnTriggerEnter(Collider other)
    {
        PlayerCoinCollector collector = other.GetComponent<PlayerCoinCollector>(); // Controlla se l'oggetto che entra ha il PlayerCoinCollector
        if (collector != null)
        {
            collector.AddCoins(_value); // Aggiorna contatore
            Destroy(gameObject); // Rimuove la moneta
        }
    }
}
