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
        _textObject = GetComponent<TextMeshProUGUI>();        
    }

    // Update is called once per frame
    void Update()
    {
        _textObject.text = _score.GetValue().ToString();
        
    }
}
