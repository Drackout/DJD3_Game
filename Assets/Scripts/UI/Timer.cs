using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
    //[SerializeField] private FloatGlobalValue _timer;
    [SerializeField] private float _timerStart;

    private float _timer;
    private TextMeshProUGUI _textObject;

    void Start()
    {
        _timer = _timerStart;
        _textObject = GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        // Reduce time second by second and print
        _timer -= Time.deltaTime;

        // float remainTime = _timer.GetValue();

        float mins = (_timer % 3600f) / 60f;
        float secs = (_timer % 60f);

        _textObject.text = $"{(int)mins}:{(int)secs}";

        if (_timer <= 0f)
            SceneManager.LoadScene(0);
    }


    public void SetTimer(int v)
    {
        _timer += v;
        updateTimerText();
    }


    public void changeTimer(float v)
    {
        _timer += v;
        updateTimerText();
    }


    public void updateTimerText()
    {
        _textObject.text = _timer.ToString();
    }


    [System.Serializable]
    public struct SaveData
    {
        public float timer;
    }

    public SaveData GetSaveData()
    {
        SaveData saveData;
        saveData.timer = _timer;
        return saveData;
    }

    public void LoadSaveData(SaveData saveData)
    {
        _timer = saveData.timer;
        updateTimerText();
    }

}
