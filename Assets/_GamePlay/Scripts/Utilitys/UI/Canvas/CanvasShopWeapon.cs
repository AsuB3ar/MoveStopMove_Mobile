using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoveStopMove.Manager;
using MoveStopMove.ContentCreation;
using MoveStopMove.Core.Data;
using TMPro;
using UnityEngine.UI;
using MoveStopMove.Core;

public class CanvasShopWeapon : UICanvas
{
    private readonly Vector3 ITEM_ROT = new Vector3(180, 0, 45);
    
    [SerializeField]
    TMP_Text cashText;
    [SerializeField]
    GameObject buyButton;
    [SerializeField]
    TMP_Text buyButtonText;
    [SerializeField]
    GameObject weaponContain;
    [SerializeField]
    float speed;
    [SerializeField]
    Canvas canvas;

    [SerializeField]
    private List<UIItemData> itemToObject = new List<UIItemData>();

    GameData Data;
    ItemData currentWeapon;

    int currentIndex;
    int unlockIndex;
    int currentPrice;
    private void Awake()
    {
        Data = GameManager.Inst.GameData;
        LoadData();
    }

    private void FixedUpdate()
    {
        weaponContain.transform.Rotate(Vector3.up * speed * Time.fixedDeltaTime * 10);
    }

    public void ChangeWeapon(int value)
    {
        itemToObject[currentIndex].itemObject.SetActive(false);

        currentIndex += value;
        if(currentIndex >= itemToObject.Count)
        {
            currentIndex = 0;
        }

        if(currentIndex < 0)
        {
            currentIndex = itemToObject.Count - 1;
        }       
        currentWeapon = itemToObject[currentIndex].itemData;
        itemToObject[currentIndex].itemObject.SetActive(true);
        currentPrice = currentWeapon.price;

        if(currentWeapon.state == ItemState.Unlock)
        {
            EquipWeapon();
            buyButton.SetActive(false);
        }
        else
        {
            buyButton.SetActive(true);
            buyButtonText.text = currentWeapon.price.ToString();
        }

    }
    public void OnBuy()
    {
        if (Data.Cash < currentPrice)
        {
            Debug.Log("Not enough money!!");
            return;
        }

        Data.SetIntData(Player.P_CASH, ref Data.Cash, Data.Cash - currentPrice);
        Data.SetDataState(GameData.POOL_ID_ITEM_NAME, (int)currentWeapon.poolID, (int)ItemState.Unlock);
        currentWeapon.state = ItemState.Unlock;

        
        EquipWeapon();
        cashText.text = Data.Cash.ToString();
    }
    public override void Open()
    {
        base.Open();
        GameplayManager.Inst.SetCameraPosition(CameraPosition.ShopWeapon);

        buyButton.SetActive(false);
        itemToObject[currentIndex].itemObject.SetActive(false);      
        currentIndex = unlockIndex;
        currentWeapon = itemToObject[currentIndex].itemData;
        currentPrice = currentWeapon.price;
        itemToObject[currentIndex].itemObject.SetActive(true);

        cashText.text = Data.Cash.ToString();
    }
    public override void Close()
    {
        base.Close();
    }
    public void CloseButton()
    {
        UIManager.Inst.OpenUI(UIID.UICMainMenu);
        GameplayManager.Inst.SetCameraPosition(CameraPosition.MainMenu);
        SoundManager.Inst.PlaySound(SoundManager.Sound.Button_Click);
        Close();
    }
    public void SetCanvasCamera(Camera camera)
    {
        canvas.worldCamera = camera;
    }
    private void LoadData()
    {
        
        for(int i = 0; i < itemToObject.Count; i++)
        {
            ItemData item = itemToObject[i].itemData;
            item.state = (ItemState)Data.PoolID2State[item.poolID];

            if(item.state == ItemState.Unlock && item.poolID == (PoolID)Data.Weapon)
            {
                unlockIndex = i;
                
            }
        }

    }
    private void EquipWeapon()
    {
        unlockIndex = currentIndex;
        GameObject weapon = PrefabManager.Inst.PopFromPool(currentWeapon.poolID);
        GameplayManager.Inst.PlayerScript.ChangeWeapon(Cache.GetBaseWeapon(weapon));
        buyButton.SetActive(false);
    }
}

[System.Serializable]
public class UIItemData
{
    public ItemData itemData;
    public GameObject itemObject;
}