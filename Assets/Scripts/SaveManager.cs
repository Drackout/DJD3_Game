using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private string         _saveFilename;
    [SerializeField] private Player         _player;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerShooting _playerShooting;
    [SerializeField] private CameraControl  _cameraControl;
    //[SerializeField] private EnemyManager   _enemyManager;

    private string _saveFilePath;

    void Start()
    {
        _saveFilePath = Application.persistentDataPath + "/" + _saveFilename;
    }

    void Update()
    {
        if (Input.GetButtonDown("QuickSave"))
            QuickSaveGame();
        else if (Input.GetButtonDown("QuickLoad"))
            QuickLoadGame();
    }

    [System.Serializable]
    private struct GameSaveData
    {
        public Player.SaveData          playerSaveData;
        public PlayerMovement.SaveData  playerMovementSaveData;
        public PlayerShooting.SaveData  playerShootingSaveData;
        public CameraControl.SaveData   cameraControlSaveData;
        //public EnemyManager.SaveData    enemyManagerSaveData;
    }

    private void QuickSaveGame()
    {
        GameSaveData saveData;

        saveData.playerSaveData         = _player.GetSaveData();
        saveData.playerMovementSaveData = _playerMovement.GetSaveData();
        saveData.playerShootingSaveData = _playerShooting.GetSaveData();
        saveData.cameraControlSaveData  = _cameraControl.GetSaveData();
        //saveData.enemyManagerSaveData   = _enemyManager.GetSaveData();

        string jsonSaveData = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(_saveFilePath, jsonSaveData);

        print("Game saved.");
    }

    private void QuickLoadGame()
    {
        if (File.Exists(_saveFilePath))
        {
            string jsonSaveData = File.ReadAllText(_saveFilePath);
            GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(jsonSaveData);

            _player.LoadSaveData(saveData.playerSaveData);
            _playerMovement.LoadSaveData(saveData.playerMovementSaveData);
            _playerShooting.LoadSaveData(saveData.playerShootingSaveData);
            _cameraControl.LoadSaveData(saveData.cameraControlSaveData);
            //_enemyManager.LoadSaveData(saveData.enemyManagerSaveData);

            print("Game loaded.");
        }
    }

}
