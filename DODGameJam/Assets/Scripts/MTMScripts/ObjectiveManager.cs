using UnityEngine;

public class ObjectiveManager : MonoBehaviour , ISelect
{
    [SerializeField] private string item;
    [SerializeField] private GameObject itemPrefab;

    public void Selected()
    {
        if(ObjectiveTracker.Instance != null)
        {
            ObjectiveTracker.Instance.UnregisterTrackable(itemPrefab);
            if (!string.IsNullOrEmpty(item))
            {
                ObjectiveTracker.Instance.MarkItemCollected(item);
                Destroy(itemPrefab);
                GameManager.instance.playerScript.pickUp.resource = GameManager.instance.playerScript.sounds[1];
                GameManager.instance.playerScript.pickUp.Play();
            }
        }
    }

    private void Start()
    {
        ObjectiveTracker.Instance?.RegisterTrackable(itemPrefab);
    }
    private void Update()
    {
        
    }
}
