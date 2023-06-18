using UnityEngine;

public class NPCManager : MonoBehaviour
{
    private NPC[] _npcs;

    void Start()
    {
        _npcs = new NPC[transform.childCount];

        for (int i = 0; i < _npcs.Length; ++i)
            _npcs[i] = transform.GetChild(i).GetComponent<NPC>();
    }

    [System.Serializable]
    public struct SaveData
    {
        public NPC.SaveData[] npcsSaveData;
    }

    public SaveData GetSaveData()
    {
        SaveData saveData;

        saveData.npcsSaveData = new NPC.SaveData[_npcs.Length];

        for (int i = 0; i < saveData.npcsSaveData.Length; ++i)
            saveData.npcsSaveData[i] = _npcs[i].GetSaveData();

        return saveData;
    }

    public void LoadSaveData(SaveData saveData)
    {
        for (int i = 0; i < saveData.npcsSaveData.Length; ++i)
            _npcs[i].LoadSaveData(saveData.npcsSaveData[i]);
    }

}
