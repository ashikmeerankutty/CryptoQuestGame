using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[DisallowMultipleComponent]
public class ReloadWeaponEvent : MonoBehaviour
{
    public event Action<ReloadWeaponEvent, ReloadWeaponEventArgs> OnReloadWeapon;

    /// <summary>
    /// Specify the weapon to have it's clip reloaded.  If the total ammo is also to be increased then specify the topUpAmmoPercent.
    /// </summary>
    public void CallReloadWeaponEvent(Weapon weapon, int topUpAmmoPercent)
    {
        OnReloadWeapon?.Invoke(this, new ReloadWeaponEventArgs() { weapon = weapon, topUpAmmoPercent = topUpAmmoPercent });
    }
}


public class ReloadWeaponEventArgs : EventArgs
{
    public Weapon weapon;
    public int topUpAmmoPercent;
}