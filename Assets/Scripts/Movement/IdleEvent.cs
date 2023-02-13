using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[DisallowMultipleComponent]
public class IdleEvent : MonoBehaviour
{
    public event Action<IdleEvent> OnIdle;

    public void CallIdleEvent()
    {
        OnIdle?.Invoke(this);
    }
}
