using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDoor : MonoBehaviour , ISelect
{
    public bool allCollected = false;

    public void Selected()
    {
        ObjectiveTracker.Instance.CheckVictoryCondition();
        if (ObjectiveTracker.Instance.evidenceCount == 3) { 
            allCollected = true;
            if (ObjectiveTracker.Instance.player != null) Destroy(GameManager.instance.player);
            SceneManager.LoadScene("VictoryScene");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
