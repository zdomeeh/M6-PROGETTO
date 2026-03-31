using TMPro;
using UnityEngine;
using System.Collections;

public class DoorUnlockUI : MonoBehaviour
{
    [SerializeField] private PlayerCoinCollector _player;
    [SerializeField] private int _requiredCoins = 100;    
    [SerializeField] private TextMeshProUGUI _unlockText; 
    [SerializeField] private AudioSource _unlockSound;   
    [SerializeField] private float _displayTime = 2f;     

    private bool _triggered = false;

    void Start()
    {
        if (_player != null)
            _player.OnCoinsChanged.AddListener(CheckCoins);

        if (_unlockText != null)
            _unlockText.gameObject.SetActive(false); // parte nascosto
    }

    void CheckCoins(int coins)
    {
        if (!_triggered && coins >= _requiredCoins)
        {
            _triggered = true;

            if (_unlockText != null)
            {
                _unlockText.gameObject.SetActive(true); // mostra il testo
                StartCoroutine(HideTextAfterSeconds(_displayTime));
            }

            if (_unlockSound != null)
                _unlockSound.Play(); // suono portaa aperta
        }
    }

    private IEnumerator HideTextAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (_unlockText != null)
            _unlockText.gameObject.SetActive(false); // nasconde il testo
    }

    public void HideImmediately()  // Nasconde subito il testo
    {
        if (_unlockText != null)
            _unlockText.gameObject.SetActive(false);
    }
}
