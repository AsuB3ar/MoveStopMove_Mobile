using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationModule : MonoBehaviour
{
    public event Action<string> UpdateEventAnimationState;

    //NOTE: Specific for game,change to reuse


 
    [SerializeField]
    private Animator Anim;
    public void Activate(string AnimBoolName)
    {
        if (AnimBoolName != null)
            Anim.SetBool(AnimBoolName, true);
    }

    public void Deactivate(string AnimBoolName)
    {
        if (AnimBoolName != null)
            Anim.SetBool(AnimBoolName, false);
    }
    public void ExitAnimator()
    {
        Anim.Rebind();
        Anim.Update(0f);
    }
    public void SetFloat(string name, float value)
    {
        Anim.SetFloat(name, value);      
    }

    public void SetInt(string name, int value)
    {
        ExitAnimator();
        Anim.SetInteger(name, value);
    }
    public void SetBool(string name, bool value)
    {
        Anim.SetBool(name, value);
    }
    public void SetActive(bool p)
    {
        Anim.enabled = p;
    }

    
    public void CallEvent(string code)
    {
        UpdateEventAnimationState.Invoke(code);
    }
}
