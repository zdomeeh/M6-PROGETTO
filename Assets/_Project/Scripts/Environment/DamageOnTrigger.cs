using System.Collections.Generic;
using UnityEngine;

public class DamageOnTrigger : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _damageInterval = 1f;

    private float _timer = 0f;
    private List<LifeController> _playersInTrigger = new List<LifeController>();

    private void OnTriggerEnter(Collider other) // Quando qualcosa entra nel trigger
    {
        LifeController life = other.GetComponent<LifeController>(); // Controlla se ha un LifeController
        if (life != null && !_playersInTrigger.Contains(life))
        {
            life.AddHP(-_damage); // danno immediato
            _playersInTrigger.Add(life);
        }
    }

    private void OnTriggerStay(Collider other) // Quando qualcosa resta dentro il trigger
    {
        if (_playersInTrigger.Count == 0) return; // Se non ci sono giocatori dentro, non fare nulla

        _timer += Time.deltaTime; // Aggiunge il tempo passato dall'ultimo frame

        if (_timer >= _damageInterval) // Se è passato abbastanza tempo, fai danno di nuovo
        {
            foreach (var player in _playersInTrigger)
                player.AddHP(-_damage); // fai danno

            _timer = 0f; // resetta il timer
        }
    }

    private void OnTriggerExit(Collider other) // Quando qualcosa esce dal trigger
    {
        LifeController life = other.GetComponent<LifeController>();
        if (life != null && _playersInTrigger.Contains(life))
            _playersInTrigger.Remove(life);
    }
}
