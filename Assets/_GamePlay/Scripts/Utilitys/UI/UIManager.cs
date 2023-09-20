using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilitys;

public enum UIID
{
    UICGamePlay = 0,
    UICBlockRaycast = 1,

    UICMainMenu = 2,

    UICSetting = 3,
    UICFail = 4,
    UICVictory = 5,
    UICShopSkin = 6,
    UICShopWeapon = 7,
    UICPvpMainMenu = 8,     
    UICPvpFail = 9,
}

[DefaultExecutionOrder(-15)]
public class UIManager : SingletonPersistent<UIManager>
{
    
    private Dictionary<UIID, UICanvas> UICanvas = new Dictionary<UIID, UICanvas>();
    [SerializeField]
    protected Transform OverlayCanvas;
    [SerializeField]
    protected Transform CameraCanvas;
    #region Canvas

    protected override void Awake()
    {
        base.Awake();
    }
    public bool IsOpenedUI(UIID ID)
    {
        return UICanvas.ContainsKey(ID) && UICanvas[ID] != null && UICanvas[ID].gameObject.activeInHierarchy;
    }

    public UICanvas GetUI(UIID ID, RenderMode type = RenderMode.ScreenSpaceOverlay)
    {
        UICanvas canvas;
        Transform parentTF = null;
        switch (type)
        {
            case RenderMode.ScreenSpaceOverlay:
                parentTF = OverlayCanvas;
                break;
            case RenderMode.ScreenSpaceCamera:
                parentTF = CameraCanvas;
                break;
        }
       
        if (!UICanvas.ContainsKey(ID) || UICanvas[ID] == null)
        {
            canvas = Instantiate(Resources.Load<UICanvas>("UI/" + ID.ToString()), parentTF);
            UICanvas[ID] = canvas;
        }

        return UICanvas[ID];
    } 
    
    public T GetUI<T>(UIID ID) where T : UICanvas
    {
        return GetUI(ID) as T;
    }

    public UICanvas OpenUI(UIID ID, RenderMode type = RenderMode.ScreenSpaceOverlay)
    {
        UICanvas canvas = GetUI(ID, type);

        canvas.Setup();
        canvas.Open();

        switch (type)
        {
            case RenderMode.ScreenSpaceCamera:
                canvas.gameObject.transform.SetParent(CameraCanvas);
                break;
            case RenderMode.ScreenSpaceOverlay:
                canvas.gameObject.transform.SetParent(OverlayCanvas);
                break;
        }
        return canvas;
    }  
    
    public T OpenUI<T>(UIID ID, RenderMode type = RenderMode.ScreenSpaceOverlay) where T : UICanvas
    {
        return OpenUI(ID, type) as T;
    }

    public bool IsOpened(UIID ID)
    {
        return UICanvas.ContainsKey(ID) && UICanvas[ID] != null;
    }

    #endregion

    #region Back Button

    private Dictionary<UICanvas, UnityAction> BackActionEvents = new Dictionary<UICanvas, UnityAction>();
    private List<UICanvas> backCanvas = new List<UICanvas>();
    UICanvas BackTopUI {
        get
        {
            UICanvas canvas = null;
            if (backCanvas.Count > 0)
            {
                canvas = backCanvas[backCanvas.Count - 1];
            }

            return canvas;
        }
    }


    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Escape) && BackTopUI != null)
        {
            BackActionEvents[BackTopUI]?.Invoke();
        }
    }

    public void PushBackAction(UICanvas canvas, UnityAction action)
    {
        if (!BackActionEvents.ContainsKey(canvas))
        {
            BackActionEvents.Add(canvas, action);
        }
    }

    public void AddBackUI(UICanvas canvas)
    {
        if (!backCanvas.Contains(canvas))
        {
            backCanvas.Add(canvas);
        }
    }

    public void RemoveBackUI(UICanvas canvas)
    {
        backCanvas.Remove(canvas);
    }

    /// <summary>
    /// CLear backey when comeback index UI canvas
    /// </summary>
    public void ClearBackKey()
    {
        backCanvas.Clear();
    }

    #endregion

    public void CloseUI(UIID ID)
    {
        if (IsOpened(ID))
        {
            GetUI(ID).Close();
        }
    }

}
