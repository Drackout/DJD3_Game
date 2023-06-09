using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD;

public class Player : MonoBehaviour
{
    [SerializeField] private UIManager  _uiManager;
    [SerializeField] private int        _maxHealth;

    private int _health;

    void Start()
    {
        _health = _maxHealth;
    }

    public void Damage(int amount)
    {
        _health = Mathf.Max(_health - amount, 0);
		FMODUnity.RuntimeManager.PlayOneShot("event:/Hurt", transform.position);

        UpdateUI();

        if (_health == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(0);
        }
    }

    private void UpdateUI()
    {
        _uiManager.SetHealthFill((float)_health / _maxHealth);
    }

    public void Heal()
    {
        _health = _maxHealth;
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetButtonDown("cheat_hp"))
        {
            Heal();
        }
    }


    [System.Serializable]
    public struct SaveData
    {
        public int health;
    }

    public SaveData GetSaveData()
    {
        SaveData saveData;

        saveData.health = _health;

        return saveData;
    }

    public void LoadSaveData(SaveData saveData)
    {
        _health = saveData.health;

        UpdateUI();
    }


}
