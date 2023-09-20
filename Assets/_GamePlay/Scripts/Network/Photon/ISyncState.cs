using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISyncState 
{
    public enum STATE
    {
        ON_INIT = 0,
        READY = 1,
    }

    public STATE State
    {
        get;
    }
}
