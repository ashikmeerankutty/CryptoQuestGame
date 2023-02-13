using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerSelectionUI : MonoBehaviour
{
    #region Tooltip
    [Tooltip("Populate with the Sprite Renderer on child gameobject WeaponAnchorPosition/WeaponRotationPoint/Hand")]
    #endregion
    public SpriteRenderer playerHandSpriteRenderer;
    #region Tooltip
    [Tooltip("Populate with the Sprite Renderer on child gameobject HandNoWeapon")]
    #endregion
    public SpriteRenderer playerHandNoWeaponSpriteRenderer;
    #region Tooltip
    [Tooltip("Populate with the Sprite Renderer on child gameobject WeaponAnchorPosition/WeaponRotationPoint/Weapon")]
    #endregion
    public SpriteRenderer playerWeaponSpriteRenderer;
    #region Tooltip
    [Tooltip("Populate with the Animator component")]
    #endregion
    public Animator animator;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerHandSpriteRenderer), playerHandSpriteRenderer);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerHandNoWeaponSpriteRenderer), playerHandNoWeaponSpriteRenderer);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerWeaponSpriteRenderer), playerWeaponSpriteRenderer);
        HelperUtilities.ValidateCheckNullValue(this, nameof(animator), animator);
    }
#endif
    #endregion
}
