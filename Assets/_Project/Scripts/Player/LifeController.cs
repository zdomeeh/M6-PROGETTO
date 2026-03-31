using UnityEngine;
using UnityEngine.Events;

public class LifeController : MonoBehaviour
{
    [SerializeField] private int _currentHP = 100;
    [SerializeField] private int _maxHP = 100;
    [SerializeField] private bool _fullHPOnStart = true;

    [SerializeField] private UnityEvent<int, int> _onHPChanged;
    [SerializeField] private UnityEvent _onDefeated;

    // Eventi pubblici per GameManager
    public UnityEvent<int, int> OnHPChanged => _onHPChanged;
    public UnityEvent OnDefeated => _onDefeated;

    private void Start()
    {
        if (_fullHPOnStart)
        {
            SetHP(_maxHP); // Imposta vita piena all’inizio
        }
    }

    public int GetHP() => _currentHP;       // ritorna HP attuali
    public int GetMaxHP() => _maxHP;        // ritorna HP massimi

    public void AddHP(int amount)
    {
        if (_currentHP <= 0) return;        // se gia’ morto, non fare nulla

        SetHP(_currentHP + amount);         // aggiorna HP

        if (amount < 0 && _currentHP > 0)
        {
            AudioManager.Instance?.PlayPlayerDamage(); // suono danno
        }
    }

    public void SetHP(int hp)
    {
        hp = Mathf.Clamp(hp, 0, _maxHP);    // limita HP tra 0 e massimo

        if (hp != _currentHP)
        {
            _currentHP = hp;
            _onHPChanged.Invoke(_currentHP, _maxHP); // aggiorna UI

            if (_currentHP == 0)
            {
                _onDefeated.Invoke();       // trigger evento morte
            }
        }
    }
}