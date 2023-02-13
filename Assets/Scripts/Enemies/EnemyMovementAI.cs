using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]

public class EnemyMovementAI : MonoBehaviour
{
    #region Tooltip
    [Tooltip("MovementDetailsSO scriptable object containing movement details such as speed")]
    #endregion Tooltip
    [SerializeField] private MovementDetailsSO movementDetails;
    private Enemy enemy;
    private Stack<Vector3> movementSteps = new Stack<Vector3>();
    private Vector3 playerReferencePosition;
    private Coroutine moveEnemyRoutine;
    private float currentEnemyPathRebuildCooldown;
    private WaitForFixedUpdate waitForFixedUpdate;
    [HideInInspector] public float moveSpeed;
    private bool chasePlayer = false;
    [HideInInspector] public int updateFrameNumber = 1; // default value.  This is set by the enemy spawner.
    private List<Vector2Int> surroundingPositionList = new List<Vector2Int>();

    private void Awake()
    {
        // Load components
        enemy = GetComponent<Enemy>();

        moveSpeed = movementDetails.GetMoveSpeed();
    }

    private void Start()
    {
        // Create waitforfixed update for use in coroutine
        waitForFixedUpdate = new WaitForFixedUpdate();

        // Reset player reference position
        playerReferencePosition = GameManager.Instance.GetPlayer().GetPlayerPosition();

    }

    private void Update()
    {
        MoveEnemy();
    }


    /// <summary>
    /// Use AStar pathfinding to build a path to the player - and then move the enemy to each grid location on the path
    /// </summary>
    private void MoveEnemy()
    {
        // Movement cooldown timer
        currentEnemyPathRebuildCooldown -= Time.deltaTime;

        // Check distance to player to see if enemy should start chasing
        if (!chasePlayer && Vector3.Distance(transform.position, GameManager.Instance.GetPlayer().GetPlayerPosition()) < enemy.enemyDetails.chaseDistance)
        {
            chasePlayer = true;
        }

        // If not close enough to chase player then return
        if (!chasePlayer)
            return;

        // Only process A Star path rebuild on certain frames to spread the load between enemies
        if (Time.frameCount % Settings.targetFrameRateToSpreadPathfindingOver != updateFrameNumber) return;

        // if the movement cooldown timer reached or player has moved more than required distance
        // then rebuild the enemy path and move the enemy
        if (currentEnemyPathRebuildCooldown <= 0f || (Vector3.Distance(playerReferencePosition, GameManager.Instance.GetPlayer().GetPlayerPosition()) > Settings.playerMoveDistanceToRebuildPath))
        {
            // Reset path rebuild cooldown timer
            currentEnemyPathRebuildCooldown = Settings.enemyPathRebuildCooldown;

            // Reset player reference position
            playerReferencePosition = GameManager.Instance.GetPlayer().GetPlayerPosition();

            // Move the enemy using AStar pathfinding - Trigger rebuild of path to player
            CreatePath();

            // If a path has been found move the enemy
            if (movementSteps != null)
            {
                if (moveEnemyRoutine != null)
                {
                    // Trigger idle event
                    enemy.idleEvent.CallIdleEvent();
                    StopCoroutine(moveEnemyRoutine);
                }

                // Move enemy along the path using a coroutine
                moveEnemyRoutine = StartCoroutine(MoveEnemyRoutine(movementSteps));

            }
        }
    }


    /// <summary>
    /// Coroutine to move the enemy to the next location on the path
    /// </summary>
    private IEnumerator MoveEnemyRoutine(Stack<Vector3> movementSteps)
    {
        while (movementSteps.Count > 0)
        {
            Vector3 nextPosition = movementSteps.Pop();

            // while not very close continue to move - when close move onto the next step
            while (Vector3.Distance(nextPosition, transform.position) > 0.2f)
            {
                // Trigger movement event
                enemy.movementToPositionEvent.CallMovementToPositionEvent(nextPosition, transform.position, moveSpeed, (nextPosition - transform.position).normalized);

                yield return waitForFixedUpdate;  // moving the enmy using 2D physics so wait until the next fixed update

            }

            yield return waitForFixedUpdate;
        }

        // End of path steps - trigger the enemy idle event
        enemy.idleEvent.CallIdleEvent();

    }

    /// <summary>
    /// Use the AStar static class to create a path for the enemy
    /// </summary>
    private void CreatePath()
    {
        Room currentRoom = GameManager.Instance.GetCurrentRoom();

        Grid grid = currentRoom.instantiatedRoom.grid;

        // Get players position on the grid
        Vector3Int playerGridPosition = GetNearestNonObstaclePlayerPosition(currentRoom);


        // Get enemy position on the grid
        Vector3Int enemyGridPosition = grid.WorldToCell(transform.position);

        // Build a path for the enemy to move on
        movementSteps = AStar.BuildPath(currentRoom, enemyGridPosition, playerGridPosition);

        // Take off first step on path - this is the grid square the enemy is already on
        if (movementSteps != null)
        {
            movementSteps.Pop();
        }
        else
        {
            // Trigger idle event - no path
            enemy.idleEvent.CallIdleEvent();
        }
    }

    /// <summary>
    /// Set the frame number that the enemy path will be recalculated on - to avoid performance spikes
    /// </summary>
    public void SetUpdateFrameNumber(int updateFrameNumber)
    {
        this.updateFrameNumber = updateFrameNumber;
    }

    /// <summary>
    /// Get the nearest position to the player that isn't on an obstacle
    /// </summary>
    private Vector3Int GetNearestNonObstaclePlayerPosition(Room currentRoom)
    {
        Vector3 playerPosition = GameManager.Instance.GetPlayer().GetPlayerPosition();

        Vector3Int playerCellPosition = currentRoom.instantiatedRoom.grid.WorldToCell(playerPosition);

        Vector2Int adjustedPlayerCellPositon = new Vector2Int(playerCellPosition.x - currentRoom.templateLowerBounds.x, playerCellPosition.y - currentRoom.templateLowerBounds.y);

        int obstacle = Mathf.Min(currentRoom.instantiatedRoom.aStarMovementPenalty[adjustedPlayerCellPositon.x, adjustedPlayerCellPositon.y], currentRoom.instantiatedRoom.aStarItemObstacles[adjustedPlayerCellPositon.x, adjustedPlayerCellPositon.y]);

        // if the player isn't on a cell square marked as an obstacle then return that position
        if (obstacle != 0)
        {
            return playerCellPosition;
        }
        // find a surounding cell that isn't an obstacle - required because with the 'half collision' tiles
        // and tables the player can be on a grid square that is marked as an obstacle
        else
        {
            // Empty surrounding position list
            surroundingPositionList.Clear();

            // Populate surrounding position list - this will hold the 8 possible vector locations surrounding a (0,0) grid square
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (j == 0 && i == 0) continue;

                    surroundingPositionList.Add(new Vector2Int(i, j));
                }
            }


            // Loop through all positions
            for (int l = 0; l < 8; l++)
            {
                // Generate a random index for the list
                int index = Random.Range(0, surroundingPositionList.Count);

                // See if there is an obstacle in the selected surrounding position
                try
                {
                    obstacle = Mathf.Min(currentRoom.instantiatedRoom.aStarMovementPenalty[adjustedPlayerCellPositon.x + surroundingPositionList[index].x, adjustedPlayerCellPositon.y + surroundingPositionList[index].y], currentRoom.instantiatedRoom.aStarItemObstacles[adjustedPlayerCellPositon.x + surroundingPositionList[index].x, adjustedPlayerCellPositon.y + surroundingPositionList[index].y]);

                    // If no obstacle return the cell position to navigate to
                    if (obstacle != 0)
                    {
                        return new Vector3Int(playerCellPosition.x + surroundingPositionList[index].x, playerCellPosition.y + surroundingPositionList[index].y, 0);
                    }

                }
                // Catch errors where the surrounding positon is outside the grid
                catch
                {

                }

                // Remove the surrounding position with the obstacle so we can try again
                surroundingPositionList.RemoveAt(index);
            }


            // If no non-obstacle cells found surrounding the player - send the enemy in the direction of an enemy spawn position
            return (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Length)];

        }
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