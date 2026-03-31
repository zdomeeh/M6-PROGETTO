using UnityEngine;
using UnityEngine.Events;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private float _timeInSeconds = 60f;

    public UnityEvent<float> OnTimeChanged;
    public UnityEvent OnTimeEnded;

    private float _currentTime;
    private bool _isRunning = true;

    void Start()
    {
        _currentTime = _timeInSeconds;
        OnTimeChanged.Invoke(_currentTime); // aggiorna UI con il tempo iniziale
    }

    void Update()
    {
        if (!_isRunning) return; // se il timer non e' attivo, non fare nulla

        _currentTime -= Time.deltaTime; // riduce il tempo ogni frame
        if (_currentTime < 0) _currentTime = 0; // non va sotto zero

        OnTimeChanged.Invoke(_currentTime); // aggiorna UI o eventi con il tempo corrente

        if (_currentTime <= 0)
        {
            _isRunning = false; // ferma il timer
            OnTimeEnded.Invoke(); // segnala che il tempo e' finito
        }
    }

    public void AddTime(float seconds)
    {
        _currentTime += seconds;
        if (_currentTime > _timeInSeconds)
            _currentTime = _timeInSeconds; // non supera il tempo massimo
        OnTimeChanged.Invoke(_currentTime); // aggiorna UI
    }

    public void StopTimer() => _isRunning = false; // ferma il timer

    // reset timer per respawn
    public void ResetTimer(float seconds)
    {
        _timeInSeconds = seconds;
        _currentTime = seconds;
        _isRunning = true;
        OnTimeChanged.Invoke(_currentTime); // aggiorna UI
    }
}