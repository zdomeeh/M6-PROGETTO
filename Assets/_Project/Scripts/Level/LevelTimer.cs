using UnityEngine;
using UnityEngine.Events;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private float _timeInSeconds = 60f;

    public UnityEvent<float> OnTimeChanged; // Evento per aggiornare UI
    public UnityEvent OnTimeEnded; // Evento chiamato quando il tempo finisce

    private float _currentTime;
    private bool _isRunning = true;

    void Start()
    {
        _currentTime = _timeInSeconds; // Imposta il timer al tempo iniziale
        OnTimeChanged.Invoke(_currentTime);
    }

    void Update()
    {
        if (!_isRunning) // Se il timer è fermo, non fare nulla
            return;

        _currentTime -= Time.deltaTime; // Decrementa il timer in base al deltaTime

        if (_currentTime < 0) // Evita valori negativi
            _currentTime = 0;

        OnTimeChanged.Invoke(_currentTime);

        if (_currentTime <= 0) // Se il tempo finisce, ferma il timer e invoca l'evento
        {
            _isRunning = false;
            OnTimeEnded.Invoke();
        }
    }

    public void AddTime(float seconds) // Metodo per aggiungere tempo extra al timer
    {
        _currentTime += seconds;

        // Non fa superare il tempo iniziale
        if (_currentTime > _timeInSeconds)
            _currentTime = _timeInSeconds;

        // Aggiorna l'UI
        OnTimeChanged.Invoke(_currentTime);
    }

    public void StopTimer()
    {
        _isRunning = false; // il timer smette di scorrere
    }
}
