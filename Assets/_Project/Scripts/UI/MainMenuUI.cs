using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    // Avvia il gioco caricando il livello
    public void StartGame()
    {
        SceneManager.LoadScene("LevelOne");
    }

    // Esce dall’applicazione
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Hai premuto Exit Game"); 
    }
}
