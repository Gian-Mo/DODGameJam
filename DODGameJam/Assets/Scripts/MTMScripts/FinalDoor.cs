using UnityEngine;

public class FinalDoor : MonoBehaviour , ISelect
{
    public bool allCollected = false;

    public void Selected()
    {
        ObjectiveTracker.Instance.CheckVictoryCondition();
        if (ObjectiveTracker.Instance.evidenceCount == 3) { allCollected = true; ObjectiveTracker.Instance.Victory(); }
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
