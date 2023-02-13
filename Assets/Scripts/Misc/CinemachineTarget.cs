using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class CinemachineTarget : MonoBehaviour
{
    private CinemachineTargetGroup cinemachineTargetGroup;

    #region Tooltip
    [Tooltip("Populate with the CursorTarget gameobject")]
    #endregion Tooltip
    [SerializeField] private Transform cursorTarget;

    private void Awake()
    {
        // Load components
        cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCinemachineTargetGroup();
    }

    /// <summary>
    /// Set the cinemachine camera target group.
    /// </summary>
    private void SetCinemachineTargetGroup()
    {
        // Create target group for cinemachine for the cinemachine camera to follow  - group will include the player and screen cursor
        CinemachineTargetGroup.Target cinemachineGroupTarget_player = new CinemachineTargetGroup.Target { weight = 1f, radius = 2.5f, target = GameManager.Instance.GetPlayer().transform };

        CinemachineTargetGroup.Target cinemachineGroupTarget_cursor = new CinemachineTargetGroup.Target { weight = 1f, radius = 1f, target = cursorTarget };

        CinemachineTargetGroup.Target[] cinemachineTargetArray = new CinemachineTargetGroup.Target[] { cinemachineGroupTarget_player, cinemachineGroupTarget_cursor };

        cinemachineTargetGroup.m_Targets = cinemachineTargetArray;

    }

    private void Update()
    {
        cursorTarget.position = HelperUtilities.GetMouseWorldPosition();
    }

}
