using UnityEngine;

public class DangerZone : MonoBehaviour
{
    [SerializeField] private int _damagePerTick = 20;
    [SerializeField] private float _damageInterval = 0.3f; // intervallo tra danni

    private float _timer = 0f;
    private LifeController _currentLife = null;

    private void OnTriggerEnter(Collider other)
    {
        LifeController life = other.GetComponent<LifeController>(); // Controlla se l'oggetto che entra ha un LifeController
        if (life == null)
            return;

        // Danno immediato appena cade
        life.AddHP(-_damagePerTick);

        _currentLife = life; // Salva il riferimento alla vita del player e resetta il timer
        _timer = 0f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_currentLife == null) // Se non c'è un player attivo nella zona, non fare nulla
            return;

        _timer += Time.deltaTime;  // Aggiorna il timer

        if (_timer >= _damageInterval) // Se il timer supera l'intervallo definito, infligge danno e resetta il timer
        {
            _currentLife.AddHP(-_damagePerTick);
            _timer = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<LifeController>() != null) // Quando il player esce dalla zona, resetta riferimenti e timer
        {
            _currentLife = null;
            _timer = 0f;
        }
    }
}
