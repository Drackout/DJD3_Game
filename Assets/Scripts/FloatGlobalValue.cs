using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatGlobalValue", menuName = "Global Values/Float")]
public class FloatGlobalValue : ScriptableObject
{
    [SerializeField] private float _originalValue;
    [SerializeField] private float _value;

    public void SetValue(float v)
    {
        _value = v;
    }

    public void ChangeValue(float deltaValue)
    {
        _value += deltaValue;
    }

    public float GetValue()
    {
        return _value;
    }

    public float GetOriginalValue()
    {
        return _originalValue;
    }
}
