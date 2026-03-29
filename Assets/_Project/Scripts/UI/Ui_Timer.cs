using TMPro;
using UnityEngine;

public class Ui_Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;

    public void UpdateTimer(float time) // Aggiorna il testo del timer in UI
    {
        int seconds = Mathf.CeilToInt(time);
        _timerText.text = "Time: " + seconds;
    }
}
