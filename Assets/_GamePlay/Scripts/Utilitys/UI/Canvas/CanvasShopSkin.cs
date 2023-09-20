using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoveStopMove.Manager;
using MoveStopMove.ContentCreation;
using UnityEngine.UI;
using Utilitys;
using MoveStopMove.Core.Data;
using TMPro;
using MoveStopMove.Core;

public class CanvasShopSkin : UICanvas
{
    [SerializeField]
    TMP_Text cashText;  
    [SerializeField]
    Button buyButton;
    [SerializeField]
    TMP_Text buyButtonText;
    [SerializeField]
    List<Button> tabButtons;
    [SerializeField]
    private List<ItemData> hairItemDatas = new List<ItemData>();
    [SerializeField]
    private List<ItemData> pantItemDatas = new List<ItemData>(); 
    [SerializeField]
    private List<ScrollViewController> scrollViews;
    private List<UIItem> items = new List<UIItem>();

    private GameData Data;
    private Button currentButtonTab;

    private UIItem currentItem;

    private ItemData hairUnlock;
    private ItemData pantUnlock;

    private int currentPrice;
    private void Awake()
    {
        Data = GameManager.Inst.GameData;
        currentButtonTab = tabButtons[0];
        LoadData();
    }
    private void Start()
    {
        currentButtonTab.Select();
        buyButton.gameObject.SetActive(false);
        for(int i = 0; i < hairItemDatas.Count; i++)
        {
            UIItem UIItemScript = scrollViews[0].AddUIItem(hairItemDatas[i]);       
            Subscribe(UIItemScript);        
        }

        for(int i = 0; i < pantItemDatas.Count; i++)
        {
            UIItem UIItemScript = scrollViews[1].AddUIItem(pantItemDatas[i]);
            Subscribe(UIItemScript);
        }
    }
    public override void Open()
    {
        base.Open();
        GameplayManager.Inst.SetCameraPosition(CameraPosition.ShopSkin);
        cashText.text = Data.Cash.ToString();
        
    }
    public void OpenTab(int type)
    {
        buyButton.gameObject.SetActive(false);
        for (int i = 0; i < scrollViews.Count; i++)
        {
            if (i == type)
            {
                scrollViews[i].gameObject.SetActive(true);
                continue;
            }
            scrollViews[i].gameObject.SetActive(false);
            SoundManager.Inst.PlaySound(SoundManager.Sound.Button_Click);
        }
    }
    public void CloseButton()
    {
        UIManager.Inst.OpenUI(UIID.UICMainMenu);
        Close();

        GameplayManager.Inst.PlayerScript.ChangeHair(hairUnlock.poolID);
        GameplayManager.Inst.PlayerScript.ChangePant(pantUnlock.pant);
    }
    
    public void Subscribe(UIItem item)
    {
        items.Add(item);
        item.OnSelectItem += OnItemClick;
    }

    public void Unsubscribe(UIItem item)
    {
        items.Remove(item);
        item.OnSelectItem -= OnItemClick;
    }

    public void OnItemClick(UIItem item)
    {      
        buyButton.gameObject.SetActive(true);           
        currentItem = item;       
        currentPrice = item.Price;        

        SoundManager.Inst.PlaySound(SoundManager.Sound.Button_Click);
        if (item.Type == UIItemType.Hair)
        {
            GameplayManager.Inst.PlayerScript.ChangeHair(item.ItemName);
            currentButtonTab = tabButtons[0];

            if(item.State == ItemState.Unlock)
            {
                buyButton.gameObject.SetActive(false);
                hairUnlock = item.ItemData;//Need to change (UI is not allowed to contain data from controller)
            }
        }
        else if(item.Type == UIItemType.Pant)
        {
            GameplayManager.Inst.PlayerScript.ChangePant(item.PantType);
            currentButtonTab = tabButtons[1];

            if (item.State == ItemState.Unlock)
            {
                buyButton.gameObject.SetActive(false);
                pantUnlock = item.ItemData;//Need to change (UI is not allowed to contain data from controller)
            }
        }
        buyButtonText.text = currentPrice.ToString();
        currentButtonTab.Select();
    }
    public void OnBuy()
    {
        if (Data.Cash < currentPrice)
        {
            Debug.Log("Not enough money!!");
            return;
        }

        switch (currentItem.Type)
        {
            case UIItemType.Hair:
                hairUnlock = currentItem.ItemData; //Need to change (UI is not allowed to contain data from controller)
                hairUnlock.state = ItemState.Unlock;
                currentItem.SetLock(ItemState.Unlock);
                Data.SetIntData(Player.P_CASH, ref Data.Cash, Data.Cash - currentPrice);
                Data.SetDataState(GameData.POOL_ID_ITEM_NAME, (int)currentItem.ItemName, 1);
                break;
            case UIItemType.Pant:
                pantUnlock = currentItem.ItemData;//Need to change (UI is not allowed to contain data from controller)
                pantUnlock.state = ItemState.Unlock;
                currentItem.SetLock(ItemState.Unlock);
                Data.SetIntData(Player.P_CASH, ref Data.Cash, Data.Cash - currentPrice);
                Data.SetDataState(GameData.PANT_SKIN_ITEM_NAME, (int)currentItem.PantType, 1);
                break;
        }
        buyButton.gameObject.SetActive(false);
        cashText.text = Data.Cash.ToString();
    }

    private void LoadData()
    {     
        for(int i = 0; i < hairItemDatas.Count; i++)
        {
            hairItemDatas[i].state = (ItemState)Data.PoolID2State[hairItemDatas[i].poolID];
            if(hairItemDatas[i].state == ItemState.Unlock)
            {
                hairUnlock = hairItemDatas[i];
            }
        }

        for(int i = 0; i < pantItemDatas.Count; i++)
        {
            pantItemDatas[i].state = (ItemState)Data.PantSkin2State[pantItemDatas[i].pant];
            
            if(pantItemDatas[i].state == ItemState.Unlock)
            {
                pantUnlock = pantItemDatas[i];
            }
        }
    }
}
