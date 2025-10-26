using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FinalScreen : MonoBehaviour
{
    public TMP_Text doorsInteracted;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene("JDsiahScene");
    }
}
