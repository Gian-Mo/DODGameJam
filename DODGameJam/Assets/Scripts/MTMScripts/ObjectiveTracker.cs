using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.Collections;
using Unity.VisualScripting;

public class ObjectiveTracker : MonoBehaviour 
{
    public static ObjectiveTracker Instance { get; private set; }
    public playerController player; public Transform spawnpoint;
    public List<GameObject> evidence = new List<GameObject>();
    public delegate void EvidenceFound();
    public event EvidenceFound evidenceFound;
    public bool isEnd = false; public bool isVictoryScene = false;
    public int evidenceCount = 0; public int doorsInteractions = 0;

    [Serializable]
    public class SaveData
    {
        public int evidence = 0; public int doors = 0;
        public List<string> collectedEvidence = new List<string>();
        public Dictionary<string, bool> GetEvidenceCollected()
        {
            var dictionary = new Dictionary<string, bool>();
            foreach (var name in collectedEvidence) { dictionary[name] = true; }
            return dictionary;
        }
        public SaveData() { }
        public SaveData(SaveData data)
        {
            evidence = data.evidence; doors = data.doors;
            collectedEvidence = new List<string>(data.collectedEvidence);
        }
    }
    public SaveData currentSave = new SaveData();
    public SaveData sceneStartSave = new SaveData();
    private const string saveData = "SaveData";

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadGame();
    }
    private void Start()
    {
        ResetSave();
        player = GameManager.instance?.playerScript;
        if (player != null)
        {
            player.evidence = 0; player.doors = 0;
            player.SavePlayerData();
        }
        SetSaveToCurrent();
        evidenceFound += Victory;
    }
    public void Update()
    {
        
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        spawnpoint = SpawnPoint(scene.name);
        if (player != null && spawnpoint != null) player.spawnPoint = spawnpoint; player.SpawnPlayer();
        if (scene.name.Equals("VictoryScene")) { StartCoroutine(DelayedCursorUnlock()); isVictoryScene = true; }
        else { Cursor.visible = false; isVictoryScene = false; }
        if (!player) player = GameManager.instance?.playerScript;
        if (player != null)
        {
            if (!player.resetting) { SetPlayerData(sceneStartSave); SetSaveToCurrent(); }
            else
            {
                SetPlayerData(sceneStartSave);
                currentSave = new SaveData(sceneStartSave);
                player.resetting = false;
            }
        }
    }
    void SetPlayerData(SaveData save)
    {
        player.evidence = save.evidence;
        player.doors = save.doors;
    }
    void SetSaveToCurrent()
    {
        sceneStartSave = new SaveData(currentSave);
    }
    private Transform SpawnPoint(string scenename)
    {
        GameObject spawnObject = GameObject.FindGameObjectWithTag("Spawn");
        if (spawnObject != null) return spawnObject.transform;
        return null;
    }
    private IEnumerator DelayedCursorUnlock()
    {
        yield return new WaitForEndOfFrame();
        Cursor.visible = true;
    }
    public void SaveGame()
    {
        if (player != null)
        {
            currentSave.evidence = player.evidence;
            currentSave.doors = player.doors;
        }
        PlayerPrefs.SetInt("Evidence", currentSave.evidence);
        PlayerPrefs.SetInt("Doors", currentSave.doors);
        PlayerPrefs.SetInt("EvidenceCollected", currentSave.collectedEvidence.Count);
        for (int index = 0; index <= currentSave.collectedEvidence.Count; index++)
        {
            PlayerPrefs.SetString("CollectedEvidence_", currentSave.collectedEvidence[index]);
        }
        PlayerPrefs.Save();
    }
    public void LoadGame()
    {
        currentSave = new SaveData();
        currentSave.evidence = PlayerPrefs.GetInt("Evidence");
        currentSave.doors = PlayerPrefs.GetInt("Doors");
        int count = PlayerPrefs.GetInt("CollectedEvidence", CollectedEvidence());
        for (int index = 0; index < count; index++)
        {
            string item = PlayerPrefs.GetString("CollectedEvidence_" + index, "");
            if (!string.IsNullOrEmpty(item)) currentSave.collectedEvidence.Add(item);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoadedForSave;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoadedForSave;
    }
    private void OnSceneLoadedForSave(Scene scene, LoadSceneMode mode)
    {
        SaveGame();
    }
    private void OnScene(Scene scene, LoadSceneMode mode)
    {
        LoadGame();
        SceneManager.sceneLoaded -= OnScene;
    }
    private void ResetSave()
    {
        currentSave = new SaveData();
        sceneStartSave = new SaveData();
        SaveGame();
    }
    public void RegisterTrackable(GameObject item)
    {
        if (!evidence.Contains(item)) evidence.Add(item);
    }
    public void UnregisterTrackable(GameObject item)
    {
        if (evidence.Contains(item)) evidence.Remove(item);
    }
    public int CollectedEvidence()
    {
        if (currentSave.collectedEvidence.Contains("Knife") || currentSave.collectedEvidence.Contains("LipStick") || currentSave.collectedEvidence.Contains("LoveLetter")) evidenceCount++;
        return evidenceCount;
    }
    public void MarkItemCollected(string item)
    {
        if(!currentSave.collectedEvidence.Contains(item)) currentSave.collectedEvidence.Add(item);
        CheckVictoryCondition();
    }
    public void CheckVictoryCondition()
    {
        string[] items = new string[] { "Knife", "LipStick", "LoveLetter" };
        foreach (string item in items) {
            if (!currentSave.collectedEvidence.Contains(item)) return;
        }
        evidenceFound?.Invoke();
        Victory();
    }
    public void Victory()
    {
        isVictoryScene = true;
        SaveGame();
        if (player != null) Destroy(GameManager.instance.player);
        SceneManager.LoadScene("VictoryScene");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
