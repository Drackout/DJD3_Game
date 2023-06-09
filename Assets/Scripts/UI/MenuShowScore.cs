using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuShowScore : MonoBehaviour
{   
    private TextMeshProUGUI _textObject;
    private int _score;
    
    void Start()
    {
        _score = Score._score;
        _textObject = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (_score != 0)
        {
            _textObject.text = $"Last Score: {_score.ToString()}";
        }
    }
}
