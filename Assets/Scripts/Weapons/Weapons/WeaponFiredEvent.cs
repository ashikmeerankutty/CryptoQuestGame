using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[DisallowMultipleComponent]
public class WeaponFiredEvent : MonoBehaviour
{
    public event Action<WeaponFiredEvent, WeaponFiredEventArgs> OnWeaponFired;

    public void CallWeaponFiredEvent(Weapon weapon)
    {
        OnWeaponFired?.Invoke(this, new WeaponFiredEventArgs() { weapon = weapon });
    }
}

public class WeaponFiredEventArgs : EventArgs
{
    public Weapon weapon;
}