using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntGlobalValue", menuName = "Global Values/Int")]
public class IntGlobalValue : ScriptableObject
{
    [SerializeField] private int value;

    public void SetValue(int v)
    {
        value = v;
    }

    public void ChangeValue(int deltaValue)
    {
        value += deltaValue;
    }

    public int GetValue()
    {
        return value;
    }
}
