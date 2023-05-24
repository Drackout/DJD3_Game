using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private IntGlobalValue _score;

    private TextMeshProUGUI _textObject;

    void Start()
    {
        _score.SetValue(0);
        _textObject = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _textObject.text = _score.GetValue().ToString();
        
    }
}