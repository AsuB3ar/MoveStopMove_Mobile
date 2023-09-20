using MoveStopMove.ContentCreation.Weapon;
using MoveStopMove.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoveStopMove.ContentCreation;
using Utilitys;
public static class Cache 
{
    private static Dictionary<int, List<Vector3>> hashCodeToList = new Dictionary<int, List<Vector3>>();
    private static Dictionary<GameObject, BaseCharacter> gameObj2BaseCharacter = new Dictionary<GameObject, BaseCharacter>();
    private static Dictionary<Collider, BaseCharacter> collider2BaseCharacter = new Dictionary<Collider, BaseCharacter>();

    private static Dictionary<GameObject, BaseWeapon> gameObj2BaseWeapon = new Dictionary<GameObject, BaseWeapon>();
    private static Dictionary<GameObject, BaseBullet> gameObj2BaseBullet = new Dictionary<GameObject, BaseBullet>();

    private static Dictionary<GameObject, Item> gameObj2Item = new Dictionary<GameObject, Item>();
    private static Dictionary<GameObject, UIItem> gameObj2UIItem = new Dictionary<GameObject, UIItem>();
    private static Dictionary<GameObject, UITargetIndicator> gameObj2UIIndicator = new Dictionary<GameObject, UITargetIndicator>();
    private static Dictionary<GameObject, VisualEffectController> gameObj2VisualEffectController = new Dictionary<GameObject, VisualEffectController>();
    private static Dictionary<GameObject, AudioSource> gameObj2AudioSource = new Dictionary<GameObject, AudioSource>();
    private static Dictionary<GameObject, Gift> gameObj2Gift = new Dictionary<GameObject, Gift>();
    private static Dictionary<GameObject, Obstance> gameObj2Obstance = new Dictionary<GameObject, Obstance>();

    public static List<Vector3> GetCacheList(int hashCode)
    {
        if (!hashCodeToList.ContainsKey(hashCode))
        {
            hashCodeToList.Add(hashCode, new List<Vector3>());
        }
        hashCodeToList[hashCode].Clear();
        return hashCodeToList[hashCode];
    }

    public static List<Vector3> GetCacheList(int hashCode , List<Vector3> list)
    {
        List<Vector3> vec3 = GetCacheList(hashCode);
        for(int i = 0; i < list.Count; i++)
        {
            vec3.Add(list[i]);
        }
        return vec3;
    }

    public static BaseCharacter GetBaseCharacter(GameObject obj)
    {
        if (!gameObj2BaseCharacter.ContainsKey(obj))
        {
            gameObj2BaseCharacter.Add(obj, obj.GetComponent<BaseCharacter>());
        }
        return gameObj2BaseCharacter[obj];
    }

    public static BaseCharacter GetBaseCharacter(Collider col)
    {
        BaseCharacter scripts = col.GetComponent<BaseCharacter>();
        if (!collider2BaseCharacter.ContainsKey(col))
        {
            collider2BaseCharacter.Add(col, scripts);
        }

        if (!gameObj2BaseCharacter.ContainsKey(col.gameObject))
        {
            gameObj2BaseCharacter.Add(col.gameObject, scripts);
        }
        return collider2BaseCharacter[col];
    }

    public static BaseBullet GetBaseBullet(GameObject obj)
    {
        if (!gameObj2BaseBullet.ContainsKey(obj))
        {
            gameObj2BaseBullet.Add(obj, obj.GetComponent<BaseBullet>());
        }
        return gameObj2BaseBullet[obj];
    }

    public static BaseWeapon GetBaseWeapon(GameObject obj)
    {
        if (!gameObj2BaseWeapon.ContainsKey(obj))
        {
            gameObj2BaseWeapon.Add(obj, obj.GetComponent<BaseWeapon>());
        }
        return gameObj2BaseWeapon[obj];
    }

    public static Item GetItem(GameObject obj)
    {
        if (!gameObj2Item.ContainsKey(obj))
        {
            gameObj2Item.Add(obj, obj.GetComponent<Item>());
        }
        return gameObj2Item[obj];
    }

    public static UIItem GetUIItem(GameObject obj)
    {
        if (!gameObj2UIItem.ContainsKey(obj))
        {
            gameObj2UIItem.Add(obj, obj.GetComponent<UIItem>());
        }
        return gameObj2UIItem[obj];
    }

    public static UITargetIndicator GetUIIndicator(GameObject obj)
    {
        if (!gameObj2UIIndicator.ContainsKey(obj))
        {
            gameObj2UIIndicator.Add(obj, obj.GetComponent<UITargetIndicator>());
        }
        return gameObj2UIIndicator[obj];
    }

    public static VisualEffectController GetVisualEffectController(GameObject obj)
    {
        if (!gameObj2VisualEffectController.ContainsKey(obj))
        {
            gameObj2VisualEffectController.Add(obj, obj.GetComponent<VisualEffectController>());
        }
        return gameObj2VisualEffectController[obj];
    }

    public static AudioSource GetAudioSource(GameObject obj)
    {
        if (!gameObj2AudioSource.ContainsKey(obj))
        {
            gameObj2AudioSource.Add(obj, obj.GetComponent<AudioSource>());
        }
        return gameObj2AudioSource[obj];
    }

    public static Gift GetGift(GameObject obj)
    {
        if (!gameObj2Gift.ContainsKey(obj))
        {
            gameObj2Gift.Add(obj, obj.GetComponent<Gift>());
        }
        return gameObj2Gift[obj];
    }

    public static Obstance GetObstance(GameObject obj)
    {
        if (!gameObj2Obstance.ContainsKey(obj))
        {
            gameObj2Obstance.Add(obj, obj.GetComponent<Obstance>());
        }
        return gameObj2Obstance[obj];
    }
}
