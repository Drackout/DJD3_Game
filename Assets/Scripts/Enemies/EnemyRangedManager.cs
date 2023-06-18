using UnityEngine;

public class EnemyRangedManager : MonoBehaviour
{
    private EnemyRanged[] _enemiesRanged;

    void Update()
    {
        _enemiesRanged = new EnemyRanged[transform.childCount];

        for (int i = 0; i < _enemiesRanged.Length; ++i)
            _enemiesRanged[i] = transform.GetChild(i).GetComponent<EnemyRanged>();
    }

    [System.Serializable]
    public struct SaveData
    {
        public EnemyRanged.SaveData[] enemiesRangedSaveData;
    }

    public SaveData GetSaveData()
    {
        SaveData saveData;

        saveData.enemiesRangedSaveData = new EnemyRanged.SaveData[_enemiesRanged.Length];

        for (int i = 0; i < saveData.enemiesRangedSaveData.Length; ++i)
            saveData.enemiesRangedSaveData[i] = _enemiesRanged[i].GetSaveData();

        return saveData;
    }

    public void LoadSaveData(SaveData saveData)
    {
        for (int i = 0; i < saveData.enemiesRangedSaveData.Length; ++i)
            _enemiesRanged[i].LoadSaveData(saveData.enemiesRangedSaveData[i]);
    }

}
