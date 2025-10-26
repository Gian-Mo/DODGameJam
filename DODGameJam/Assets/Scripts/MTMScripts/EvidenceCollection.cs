using UnityEngine;

public class EvidenceCollection : MonoBehaviour
{
    [SerializeField] private string evidence;
    [SerializeField] private GameObject evidencePrefab;
    private void Start()
    {
        ObjectiveTracker.Instance?.RegisterTrackable(evidencePrefab);
    }
    private void OnDestroy()
    {
        if(ObjectiveTracker.Instance != null)
        {
            ObjectiveTracker.Instance.UnregisterTrackable(evidencePrefab);
            if (!string.IsNullOrEmpty(evidence)) ObjectiveTracker.Instance.MarkItemCollected(evidence);
        }
    }
}
