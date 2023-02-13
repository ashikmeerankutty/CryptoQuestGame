using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Environment : MonoBehaviour
{
    // Attach this class to environment game objects whose lighting gets faded in

    #region Header References
    [Space(10)]
    [Header("References")]
    #endregion
    #region Tooltip
    [Tooltip("Populate with the SpriteRenderer component on the prefab")]
    #endregion

    public SpriteRenderer spriteRenderer;

    #region Validation

#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(spriteRenderer), spriteRenderer);
    }

#endif

    #endregion Validation

}
