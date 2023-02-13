using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ActivateRooms : MonoBehaviour
{
    #region Header POPULATE WITH MINIMAP CAMERA
    [Header("POPULATE WITH MINIMAP CAMERA")]
    #endregion Header
    [SerializeField] private Camera miniMapCamera;

    private Camera cameraMain;

    // Start is called before the first frame update
    private void Start()
    {
        // Cache main camera
        cameraMain = Camera.main;

        InvokeRepeating("EnableRooms", 0.5f, 0.75f);
    }

    private void EnableRooms()
    {
        // if currently showing the dungeon map UI don't process
        if (GameManager.Instance.gameState == GameState.dungeonOverviewMap)
            return;

        HelperUtilities.CameraWorldPositionBounds(out Vector2Int miniMapCameraWorldPositionLowerBounds, out Vector2Int miniMapCameraWorldPositionUpperBounds, miniMapCamera);

        HelperUtilities.CameraWorldPositionBounds(out Vector2Int mainCameraWorldPositionLowerBounds, out Vector2Int mainCameraWorldPositionUpperBounds, cameraMain);


        // Iterate through dungeon rooms
        foreach (KeyValuePair<string, Room> keyValuePair in DungeonBuilder.Instance.dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;

            // If room is within miniMap camera viewport then activate room game object
            if ((room.lowerBounds.x <= miniMapCameraWorldPositionUpperBounds.x && room.lowerBounds.y <= miniMapCameraWorldPositionUpperBounds.y) && (room.upperBounds.x >= miniMapCameraWorldPositionLowerBounds.x && room.upperBounds.y >= miniMapCameraWorldPositionLowerBounds.y))
            {
                room.instantiatedRoom.gameObject.SetActive(true);

                // If room is within main camera viewport then activate environment game objects
                if ((room.lowerBounds.x <= mainCameraWorldPositionUpperBounds.x && room.lowerBounds.y <= mainCameraWorldPositionUpperBounds.y) && (room.upperBounds.x >= mainCameraWorldPositionLowerBounds.x && room.upperBounds.y >= mainCameraWorldPositionLowerBounds.y))
                {
                    room.instantiatedRoom.ActivateEnvironmentGameObjects();
                }
                else
                {
                    room.instantiatedRoom.DeactivateEnvironmentGameObjects();
                }


            }
            else
            {
                room.instantiatedRoom.gameObject.SetActive(false);
            }

        }
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(miniMapCamera), miniMapCamera);
    }
#endif
    #endregion
}