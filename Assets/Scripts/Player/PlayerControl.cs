using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]

public class PlayerControl : MonoBehaviour
{
    #region Tooltip

    [Tooltip("MovementDetailsSO scriptable object containing movement details such as speed")]

    #endregion Tooltip

    [SerializeField] private MovementDetailsSO movementDetails;

    private Player player;
    private bool leftMouseDownPreviousFrame = false;
    private int currentWeaponIndex = 1;
    private float moveSpeed;
    private Coroutine playerRollCoroutine;
    private WaitForFixedUpdate waitForFixedUpdate;
    private float playerRollCooldownTimer = 0f;
    private bool isPlayerMovementDisabled = false;

    [HideInInspector] public bool isPlayerRolling = false;

    private void Awake()
    {
        // Load components
        player = GetComponent<Player>();

        moveSpeed = movementDetails.GetMoveSpeed();
    }

    private void Start()
    {
        // Create waitforfixed update for use in coroutine
        waitForFixedUpdate = new WaitForFixedUpdate();

        // Set Starting Weapon
        SetStartingWeapon();

        // Set player animation speed
        SetPlayerAnimationSpeed();

    }

    /// <summary>
    /// Set the player starting weapon
    /// </summary>
    private void SetStartingWeapon()
    {
        int index = 1;

        foreach (Weapon weapon in player.weaponList)
        {
            if (weapon.weaponDetails == player.playerDetails.startingWeapon)
            {
                SetWeaponByIndex(index);
                break;
            }
            index++;
        }
    }

    /// <summary>
    /// Set player animator speed to match movement speed
    /// </summary>
    private void SetPlayerAnimationSpeed()
    {
        // Set animator speed to match movement speed
        player.animator.speed = moveSpeed / Settings.baseSpeedForPlayerAnimations;
    }

    private void Update()
    {
        // if player movement disabled then return
        if (isPlayerMovementDisabled)
            return;

        // if player is rolling then return
        if (isPlayerRolling) return;

        // Process the player movement input
        MovementInput();

        // Process the player weapon input
        WeaponInput();

        // Process player use item input
        UseItemInput();

        // Player roll cooldown timer
        PlayerRollCooldownTimer();
    }

    /// <summary>
    /// Player movement input
    /// </summary>
    private void MovementInput()
    {
        // Get movement input
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");
        bool rightMouseButtonDown = Input.GetMouseButtonDown(1);

        // Create a direction vector based on the input
        Vector2 direction = new Vector2(horizontalMovement, verticalMovement);

        // Adjust distance for diagonal movement (pythagoras approximation)
        if (horizontalMovement != 0f && verticalMovement != 0f)
        {
            direction *= 0.7f;
        }

        // If there is movement either move or roll
        if (direction != Vector2.zero)
        {
            if (!rightMouseButtonDown)
            {
                // trigger movement event
                player.movementByVelocityEvent.CallMovementByVelocityEvent(direction, moveSpeed);
            }
            // else player roll if not cooling down
            else if (playerRollCooldownTimer <= 0f)
            {
                PlayerRoll((Vector3)direction);
            }

        }
        // else trigger idle event
        else
        {
            player.idleEvent.CallIdleEvent();
        }
    }

    /// <summary>
    /// Player roll
    /// </summary>
    private void PlayerRoll(Vector3 direction)
    {
        playerRollCoroutine = StartCoroutine(PlayerRollRoutine(direction));
    }

    /// <summary>
    /// Player roll coroutine
    /// </summary>
    private IEnumerator PlayerRollRoutine(Vector3 direction)
    {
        // minDistance used to decide when to exit coroutine loop
        float minDistance = 0.2f;

        isPlayerRolling = true;

        Vector3 targetPosition = player.transform.position + (Vector3)direction * movementDetails.rollDistance;

        while (Vector3.Distance(player.transform.position, targetPosition) > minDistance)
        {
            player.movementToPositionEvent.CallMovementToPositionEvent(targetPosition, player.transform.position, movementDetails.rollSpeed, direction, isPlayerRolling);

            // yield and wait for fixed update
            yield return waitForFixedUpdate;

        }

        isPlayerRolling = false;

        // Set cooldown timer
        playerRollCooldownTimer = movementDetails.rollCooldownTime;

        player.transform.position = targetPosition;

    }

    private void PlayerRollCooldownTimer()
    {
        if (playerRollCooldownTimer >= 0f)
        {
            playerRollCooldownTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Weapon Input
    /// </summary>
    private void WeaponInput()
    {
        Vector3 weaponDirection;
        float weaponAngleDegrees, playerAngleDegrees;
        AimDirection playerAimDirection;

        // Aim weapon input
        AimWeaponInput(out weaponDirection, out weaponAngleDegrees, out playerAngleDegrees, out playerAimDirection);

        // Fire weapon input
        FireWeaponInput(weaponDirection, weaponAngleDegrees, playerAngleDegrees, playerAimDirection);

        // Switch weapon input
        SwitchWeaponInput();

        // Reload weapon input
        ReloadWeaponInput();
    }

    private void AimWeaponInput(out Vector3 weaponDirection, out float weaponAngleDegrees, out float playerAngleDegrees, out AimDirection playerAimDirection)
    {
        // Get mouse world position
        Vector3 mouseWorldPosition = HelperUtilities.GetMouseWorldPosition();

        // Calculate direction vector of mouse cursor from weapon shoot position
        weaponDirection = (mouseWorldPosition - player.activeWeapon.GetShootPosition());

        // Calculate direction vector of mouse cursor from player transform position
        Vector3 playerDirection = (mouseWorldPosition - transform.position);

        // Get weapon to cursor angle
        weaponAngleDegrees = HelperUtilities.GetAngleFromVector(weaponDirection);

        // Get player to cursor angle
        playerAngleDegrees = HelperUtilities.GetAngleFromVector(playerDirection);

        // Set player aim direction
        playerAimDirection = HelperUtilities.GetAimDirection(playerAngleDegrees);

        // Trigger weapon aim event
        player.aimWeaponEvent.CallAimWeaponEvent(playerAimDirection, playerAngleDegrees, weaponAngleDegrees, weaponDirection);
    }

    private void FireWeaponInput(Vector3 weaponDirection, float weaponAngleDegrees, float playerAngleDegrees, AimDirection playerAimDirection)
    {
        // Fire when left mouse button is clicked
        if (Input.GetMouseButton(0))
        {
            // Trigger fire weapon event
            player.fireWeaponEvent.CallFireWeaponEvent(true, leftMouseDownPreviousFrame, playerAimDirection, playerAngleDegrees, weaponAngleDegrees, weaponDirection);
            leftMouseDownPreviousFrame = true;
        }
        else
        {
            leftMouseDownPreviousFrame = false;
        }
    }

    private void SwitchWeaponInput()
    {

        print("entered the door with the key");
        // Switch weapon if mouse scroll wheel selecetd
        if (Input.mouseScrollDelta.y < 0f)
        {
            PreviousWeapon();
        }

        if (Input.mouseScrollDelta.y > 0f)
        {
            NextWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeaponByIndex(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetWeaponByIndex(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetWeaponByIndex(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetWeaponByIndex(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetWeaponByIndex(5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetWeaponByIndex(6);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SetWeaponByIndex(7);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SetWeaponByIndex(8);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SetWeaponByIndex(9);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SetWeaponByIndex(10);
        }

        if (Input.GetKeyDown(KeyCode.Minus))
        {
            SetCurrentWeaponToFirstInTheList();
        }

    }

    private void SetWeaponByIndex(int weaponIndex)
    {
        if (weaponIndex - 1 < player.weaponList.Count)
        {
            currentWeaponIndex = weaponIndex;
            player.setActiveWeaponEvent.CallSetActiveWeaponEvent(player.weaponList[weaponIndex - 1]);
        }
    }

    private void NextWeapon()
    {
        currentWeaponIndex++;

        if (currentWeaponIndex > player.weaponList.Count)
        {
            currentWeaponIndex = 1;
        }

        SetWeaponByIndex(currentWeaponIndex);

    }

    private void PreviousWeapon()
    {
        currentWeaponIndex--;

        if (currentWeaponIndex < 1)
        {
            currentWeaponIndex = player.weaponList.Count;
        }

        SetWeaponByIndex(currentWeaponIndex);
    }


    private void ReloadWeaponInput()
    {
        Weapon currentWeapon = player.activeWeapon.GetCurrentWeapon();

        // if current weapon is reloading return
        if (currentWeapon.isWeaponReloading) return;

        // remaining ammo is less than clip capacity then return and not infinite ammo then return
        if (currentWeapon.weaponRemainingAmmo < currentWeapon.weaponDetails.weaponClipAmmoCapacity && !currentWeapon.weaponDetails.hasInfiniteAmmo) return;

        // if ammo in clip equals clip capacity then return
        if (currentWeapon.weaponClipRemainingAmmo == currentWeapon.weaponDetails.weaponClipAmmoCapacity) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            // Call the reload weapon event
            player.reloadWeaponEvent.CallReloadWeaponEvent(player.activeWeapon.GetCurrentWeapon(), 0);
        }

    }

    /// <summary>
    /// Use the nearest item within 2 unity units from the player
    /// </summary>
    private void UseItemInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float useItemRadius = 2f;

            // Get any 'Useable' item near the player
            Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(player.GetPlayerPosition(), useItemRadius);

            // Loop through detected items to see if any are 'useable'
            foreach (Collider2D collider2D in collider2DArray)
            {
                IUseable iUseable = collider2D.GetComponent<IUseable>();

                if (iUseable != null)
                {
                    iUseable.UseItem();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if collided with something stop player roll coroutine
        StopPlayerRollRoutine();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // if in collision with something stop player roll coroutine
        StopPlayerRollRoutine();
    }

    private void StopPlayerRollRoutine()
    {
        if (playerRollCoroutine != null)
        {
            StopCoroutine(playerRollCoroutine);

            isPlayerRolling = false;
        }
    }

    /// <summary>
    /// Enable the player movement
    /// </summary>
    public void EnablePlayer()
    {
        isPlayerMovementDisabled = false;
    }

    /// <summary>
    /// Disable the player movement
    /// </summary>
    public void DisablePlayer()
    {
        isPlayerMovementDisabled = true;
        player.idleEvent.CallIdleEvent();
    }

    /// <summary>
    /// Set the current weapon to be first in the player weapon list
    /// </summary>
    private void SetCurrentWeaponToFirstInTheList()
    {
        // Create new temporary list
        List<Weapon> tempWeaponList = new List<Weapon>();

        // Add the current weapon to first in the temp list
        Weapon currentWeapon = player.weaponList[currentWeaponIndex - 1];
        currentWeapon.weaponListPosition = 1;
        tempWeaponList.Add(currentWeapon);

        // Loop through existing weapon list and add - skipping current weapon
        int index = 2;

        foreach (Weapon weapon in player.weaponList)
        {
            if (weapon == currentWeapon) continue;

            tempWeaponList.Add(weapon);
            weapon.weaponListPosition = index;
            index++;
        }

        // Assign new list
        player.weaponList = tempWeaponList;

        currentWeaponIndex = 1;

        // Set current weapon
        SetWeaponByIndex(currentWeaponIndex);
    }

    #region Validation

#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(movementDetails), movementDetails);
    }

#endif

    #endregion Validation
}