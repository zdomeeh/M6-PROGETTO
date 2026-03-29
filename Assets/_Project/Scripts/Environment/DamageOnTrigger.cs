using System.Collections.Generic;
using UnityEngine;

public class DamageOnTrigger : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _damageInterval = 1f;

    private float _timer = 0f;
    private List<LifeController> _playersInTrigger = new List<LifeController>();

    private void OnTriggerEnter(Collider other)
    {
        LifeController life = other.GetComponent<LifeController>();
        if (life != null && !_playersInTrigger.Contains(life))
        {
            life.AddHP(-_damage); // danno immediato
            _playersInTrigger.Add(life);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_playersInTrigger.Count == 0) return;

        _timer += Time.deltaTime;
        if (_timer >= _damageInterval)
        {
            foreach (var player in _playersInTrigger)
                player.AddHP(-_damage);

            _timer = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        LifeController life = other.GetComponent<LifeController>();
        if (life != null && _playersInTrigger.Contains(life))
            _playersInTrigger.Remove(life);
    }
}
