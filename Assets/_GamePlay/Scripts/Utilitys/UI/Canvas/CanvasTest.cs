using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class CanvasTest : UICanvas
{
    public static event Action<bool> _OnUndying;
    [SerializeField]
    Toggle cheatToogle;
    [SerializeField]
    Toggle undying;
    [SerializeField]
    GameObject[] cheatObjects;

    protected void Awake()
    {
        cheatToogle.onValueChanged.AddListener(CheatToggleClick);
        undying.onValueChanged.AddListener(UndyingToggleClick);
    }
    protected void CheatToggleClick(bool value)
    {
        for(int i = 0; i < cheatObjects.Length; i++)
        {
            cheatObjects[i].SetActive(value);
        }
    }

    protected void UndyingToggleClick(bool value)
    {
        _OnUndying?.Invoke(value);
    }
    protected void OnDestroy()
    {
        cheatToogle.onValueChanged.RemoveListener(CheatToggleClick);
        undying.onValueChanged.RemoveListener(UndyingToggleClick);
    }
}
