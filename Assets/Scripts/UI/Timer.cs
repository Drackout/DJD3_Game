using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private FloatGlobalValue _timer;

    private TextMeshProUGUI textObject;

    void Start()
    {
        _timer.SetValue(_timer.GetOriginalValue());
        textObject = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // Reduce time second by second and print
        _timer.ChangeValue(-Time.deltaTime);

        float remainTime = _timer.GetValue();

        float mins = (_timer.GetValue() % 3600f) / 60f;
        float secs = (_timer.GetValue() % 60f);

        textObject.text = $"{(int)mins}:{(int)secs}";

        if (_timer.GetValue() <= 0f)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
