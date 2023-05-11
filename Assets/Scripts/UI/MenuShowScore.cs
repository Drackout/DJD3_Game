using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuShowScore : MonoBehaviour
{
    [SerializeField] private IntGlobalValue _score;
    
    private TextMeshProUGUI _textObject;
    
    void Start()
    {
        _textObject = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (_score.GetValue() != 0)
        {
            _textObject.text = $"Last Score: {_score.GetValue().ToString()}";
        }
    }
}
