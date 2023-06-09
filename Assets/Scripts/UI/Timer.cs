using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
    //[SerializeField] private FloatGlobalValue _timer;
    [SerializeField] private float _timerStart;

    private float _timer123;
    private TextMeshProUGUI _textObject;

    void Start()
    {
        _timer123 = _timerStart;
        _textObject = GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        // Reduce time second by second and print
        _timer123 -= Time.deltaTime;

        // float remainTime = _timer.GetValue();

        float mins = (_timer123 % 3600f) / 60f;
        float secs = (_timer123 % 60f);

        _textObject.text = $"{(int)mins}:{(int)secs}";

        if (_timer123 <= 0f)
            SceneManager.LoadScene(0);
    }


    public void SetTimer(int v)
    {
        _timer123 += v;
        updateTimerText();
    }


    public void changeTimer(float v)
    {
        _timer123 += v;
        updateTimerText();
    }


    public void updateTimerText()
    {
        _textObject.text = _timer123.ToString();
    }
}
