using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    // Static to be accessed in the Main Menu
    public static int _score;
    private TextMeshProUGUI _textObject;

    void Start()
    {
        _textObject = GetComponent<TextMeshProUGUI>();
        _score = 0;
        UpdateScoreText();
    }

    public int GetScore()
    {
        return _score;
    }

    public void ChangeScore(int v)
    {
        _score += v;
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        _textObject.text = _score.ToString();
    }

}
