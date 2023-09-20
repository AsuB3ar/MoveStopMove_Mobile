using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using MoveStopMove.ContentCreation;

public enum UIItemType
{
    Hair = 0,
    Pant = 1,
    Weapon = 2
}
public class UIItem : MonoBehaviour
{
    public event Action<UIItem> OnSelectItem;
    [SerializeField]
    PoolID itemName;
    [SerializeField]
    UIItemType type;
    [SerializeField]
    PantSkin pantType;
    [SerializeField]
    int price;
    [SerializeField]
    ItemState state;

    [SerializeField]
    Image icon;
    [SerializeField]
    Image background;
    [SerializeField]
    GameObject lockIcon;

    private ItemData data;
    public ItemData ItemData => data;

    public PoolID ItemName => itemName;
    public UIItemType Type => type;
    public PantSkin PantType => pantType;
    public int Price => price;

    public ItemState State => state;
    Color color;
    private void Start()
    {
        color = background.color;
    }

    public void OnItemClicked()
    {
        OnSelectItem?.Invoke(this);
    }

    

    public void SetData(ItemData data)
    {
        this.data = data;
        this.itemName = data.poolID;
        this.type = data.type;
        this.pantType = data.pant;
        this.price = data.price;
        this.state = data.state;

        SetIcon(data.icon);
    }
 

    public void SetLock(ItemState value)
    {
        state = value;
        if(state == ItemState.Lock)
        {
            lockIcon.SetActive(true);
        }
        else if(state == ItemState.Unlock)
        {
            lockIcon.SetActive(false);
        }
        
    }
    private void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }
}
