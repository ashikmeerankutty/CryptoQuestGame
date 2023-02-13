using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class AnimatePlayer : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        // Load components
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        // Subscribe to movement by velocity event
        player.movementByVelocityEvent.OnMovementByVelocity += MovementByVelocityEvent_OnMovementByVelocity;

        // Subscribe to movement to position event
        player.movementToPositionEvent.OnMovementToPosition += MovementToPositionEvent_OnMovementToPosition;

        // Subscribe to idle event
        player.idleEvent.OnIdle += IdleEvent_OnIdle;

        // Subscribe to weapon aim event
        player.aimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponAim;
    }

    private void OnDisable()
    {
        // Unsubscribe from movement by velocity event
        player.movementByVelocityEvent.OnMovementByVelocity -= MovementByVelocityEvent_OnMovementByVelocity;

        // Unsubscribe from movement to position event
        player.movementToPositionEvent.OnMovementToPosition -= MovementToPositionEvent_OnMovementToPosition;

        // Unsubscribe from idle event
        player.idleEvent.OnIdle -= IdleEvent_OnIdle;

        // Unsubscribe from weapon aim event event
        player.aimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponAim;
    }

    /// <summary>
    /// On movement by velocity event handler
    /// </summary>
    private void MovementByVelocityEvent_OnMovementByVelocity(MovementByVelocityEvent movementByVelocityEvent, MovementByVelocityArgs movementByVelocityArgs)
    {
        InitializeRollAnimationParameters();
        SetMovementAnimationParameters();
    }

    /// <summary>
    /// On movement to position event handler
    /// </summary>
    private void MovementToPositionEvent_OnMovementToPosition(MovementToPositionEvent movementToPositionEvent, MovementToPositionArgs movementToPositionArgs)
    {
        InitializeAimAnimationParameters();
        InitializeRollAnimationParameters();
        SetMovementToPositionAnimationParameters(movementToPositionArgs);

    }

    /// <summary>
    /// On idle event handler
    /// </summary>
    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {
        InitializeRollAnimationParameters();
        SetIdleAnimationParameters();
    }

    /// <summary>
    /// On weapon aim event handler
    /// </summary>
    private void AimWeaponEvent_OnWeaponAim(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {
        InitializeAimAnimationParameters();
        InitializeRollAnimationParameters();
        SetAimWeaponAnimationParameters(aimWeaponEventArgs.aimDirection);
    }

    /// <summary>
    /// Initialise aim animation parameters
    /// </summary>
    private void InitializeAimAnimationParameters()
    {
        player.animator.SetBool(Settings.aimUp, false);
        player.animator.SetBool(Settings.aimUpRight, false);
        player.animator.SetBool(Settings.aimUpLeft, false);
        player.animator.SetBool(Settings.aimRight, false);
        player.animator.SetBool(Settings.aimLeft, false);
        player.animator.SetBool(Settings.aimDown, false);
    }

    private void InitializeRollAnimationParameters()
    {
        player.animator.SetBool(Settings.rollDown, false);
        player.animator.SetBool(Settings.rollRight, false);
        player.animator.SetBool(Settings.rollLeft, false);
        player.animator.SetBool(Settings.rollUp, false);
    }


    /// <summary>
    /// Set movement animation parameters
    /// </summary>
    private void SetMovementAnimationParameters()
    {
        player.animator.SetBool(Settings.isMoving, true);
        player.animator.SetBool(Settings.isIdle, false);
    }

    /// <summary>
    /// Set movement to position animation parameters
    /// </summary>
    private void SetMovementToPositionAnimationParameters(MovementToPositionArgs movementToPositionArgs)
    {
        // Animate roll
        if (movementToPositionArgs.isRolling)
        {
            if (movementToPositionArgs.moveDirection.x > 0f)
            {
                player.animator.SetBool(Settings.rollRight, true);
            }
            else if (movementToPositionArgs.moveDirection.x < 0f)
            {
                player.animator.SetBool(Settings.rollLeft, true);
            }
            else if (movementToPositionArgs.moveDirection.y > 0f)
            {
                player.animator.SetBool(Settings.rollUp, true);
            }
            else if (movementToPositionArgs.moveDirection.y < 0f)
            {
                player.animator.SetBool(Settings.rollDown, true);
            }
        }
    }

    /// <summary>
    /// Set movement animation parameters
    /// </summary>
    private void SetIdleAnimationParameters()
    {
        player.animator.SetBool(Settings.isMoving, false);
        player.animator.SetBool(Settings.isIdle, true);
    }

    /// <summary>
    /// Set aim animation parameters
    /// </summary>
    private void SetAimWeaponAnimationParameters(AimDirection aimDirection)
    {
        // Set aim direction
        switch (aimDirection)
        {
            case AimDirection.Up:
                player.animator.SetBool(Settings.aimUp, true);
                break;

            case AimDirection.UpRight:
                player.animator.SetBool(Settings.aimUpRight, true);
                break;

            case AimDirection.UpLeft:
                player.animator.SetBool(Settings.aimUpLeft, true);
                break;

            case AimDirection.Right:
                player.animator.SetBool(Settings.aimRight, true);
                break;

            case AimDirection.Left:
                player.animator.SetBool(Settings.aimLeft, true);
                break;

            case AimDirection.Down:
                player.animator.SetBool(Settings.aimDown, true);
                break;

        }

    }

}
