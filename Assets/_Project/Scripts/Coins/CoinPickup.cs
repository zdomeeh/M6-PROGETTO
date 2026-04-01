using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public enum CoinType
    {
        Normal,      // Moneta normale che aumenta solo il punteggio
        TimeBonus    // Moneta che aggiunge tempo al timer
    }

    [Header("Type")]
    [SerializeField] private CoinType _coinType = CoinType.Normal;

    [Header("Normal Coin")]
    [SerializeField] private int _coinValue = 1;

    [Header("Time Bonus Coin")]
    [SerializeField] private float _timeBonus = 10f;

    private void OnTriggerEnter(Collider other)
    {
        // Controlla se l'oggetto che entra nel trigger e' il player
        PlayerCoinCollector collector = other.GetComponent<PlayerCoinCollector>();

        // Se non e' il player, esce dalla funzione
        if (collector == null)
            return;

        // Gestisce il tipo di moneta
        switch (_coinType)
        {
            case CoinType.Normal:
                // Aggiunge le monete normali al totale del player
                collector.AddCoins(_coinValue);
                break;

            case CoinType.TimeBonus:
                // Trova il timer del livello
                LevelTimer timer = FindObjectOfType<LevelTimer>();

                // Se il timer esiste, aggiunge il bonus di tempo
                if (timer != null)
                    timer.AddTime(_timeBonus);

                // Riproduce il suono della moneta tempo
                AudioManager.Instance?.PlayTimeCoin();
                break;
        }

        // Salva la moneta raccolta nel sistema checkpoint
        CheckpointManager.Instance?.RegisterCollectedCoin(gameObject);

        // Disattiva la moneta dopo la raccolta
        gameObject.SetActive(false);
    }
}