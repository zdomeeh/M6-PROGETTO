using UnityEngine;

public class CoinTimeBonusPickup : MonoBehaviour
{
    [SerializeField] private float _timeBonus = 10f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerCoinCollector collector = other.GetComponent<PlayerCoinCollector>();
        if (collector != null)
        {
            // Trova il LevelTimer in scena
            LevelTimer timer = FindObjectOfType<LevelTimer>();
            if (timer != null)
            {
                timer.AddTime(_timeBonus); // aggiunge tempo
            }

            // Suono del time coin tramite AudioManager
            AudioManager.Instance?.PlayTimeCoin();

            Destroy(gameObject); // rimuove la moneta bonus
        }
    }
}
