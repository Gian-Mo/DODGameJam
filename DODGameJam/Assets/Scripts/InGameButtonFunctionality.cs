using UnityEngine;
using UnityEngine.SceneManagement;


public class InGameButtonFunctionality : MonoBehaviour
{
   public void Resume()
   {
        GameManager.instance.stateUnpause();
        ButtonController.instance.ButtonClear();
    }
    public void Restart() {

        GameManager.instance.stateUnpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        GameManager.instance.stateUnpause();
        SceneManager.LoadScene("MainMenu");
    }
  
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;


#else

         Application.Quit(); Application.Quit();


#endif

    }
}
