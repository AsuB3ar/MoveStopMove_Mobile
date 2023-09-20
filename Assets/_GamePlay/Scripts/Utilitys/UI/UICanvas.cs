using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    //public bool IsAvoidBackKey = false;
    [SerializeField]
    protected bool IsDestroyOnClose = false;

    protected RectTransform m_RectTransform;
    public RectTransform RectTransform => m_RectTransform;
    private Animator m_Animator;
    private bool m_IsInit = false;
    private float m_OffsetY = 0;

    private void Awake()
    {
        Init();
    }

    protected void Init()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_Animator = GetComponent<Animator>();
    }

    public virtual void Setup()
    {
        UIManager.Inst.AddBackUI(this);
        UIManager.Inst.PushBackAction(this, BackKey);
    }

    public virtual void BackKey()
    {

    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
        //anim
    }

    public virtual void Close()
    {
        UIManager.Inst.RemoveBackUI(this);
        //anim
        gameObject.SetActive(false);
        if (IsDestroyOnClose)
        {
            Destroy(gameObject);
        }
        
    }


}
