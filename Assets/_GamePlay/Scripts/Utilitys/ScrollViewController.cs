
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilitys {
    using MoveStopMove.ContentCreation;
    using MoveStopMove.Manager;
    public class ScrollViewController : MonoBehaviour
    {
        [SerializeField]
        Transform contentTranform;
        List<GameObject> uiItemObjects = new List<GameObject>();

        public UIItem AddUIItem(ItemData data)
        {
            GameObject uiItemObject = PrefabManager.Inst.PopFromPool(PoolID.UIItem);
            uiItemObject.transform.position = Vector3.zero;

            UIItem UIItemScript = Cache.GetUIItem(uiItemObject);
            UIItemScript.SetData(data);
            UIItemScript.SetLock(data.state);

            Vector3 initScale = uiItemObject.transform.localScale;
            uiItemObject.transform.SetParent(contentTranform);
            uiItemObject.transform.localScale = initScale; //NOTE: Change to not scale
            uiItemObjects.Add(uiItemObject);

            return UIItemScript;
        }
        public bool RemoveUIItem(UIItem uiItem)
        {
            GameObject uiItemObj = uiItem.gameObject;
            if (uiItemObjects.Contains(uiItemObj))
            {
                PrefabManager.Inst.PushToPool(uiItemObj, PoolID.UIItem);
                uiItemObjects.Remove(uiItemObj);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}