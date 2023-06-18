using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Enemy[] _enemies;

    void Start()
    {
        _enemies = new Enemy[transform.childCount];

        for (int i = 0; i < _enemies.Length; ++i)
            _enemies[i] = transform.GetChild(i).GetComponent<Enemy>();
    }

    [System.Serializable]
    public struct SaveData
    {
        public Enemy.SaveData[] enemiesSaveData;
    }

    public SaveData GetSaveData()
    {
        SaveData saveData;

        saveData.enemiesSaveData = new Enemy.SaveData[_enemies.Length];

        for (int i = 0; i < saveData.enemiesSaveData.Length; ++i)
            saveData.enemiesSaveData[i] = _enemies[i].GetSaveData();

        return saveData;
    }

    public void LoadSaveData(SaveData saveData)
    {
        for (int i = 0; i < saveData.enemiesSaveData.Length; ++i)
            _enemies[i].LoadSaveData(saveData.enemiesSaveData[i]);
    }

}
