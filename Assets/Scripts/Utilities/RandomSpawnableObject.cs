using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnableObject<T>
{
    private struct chanceBoundaries
    {
        public T spawnableObject;
        public int lowBoundaryValue;
        public int highBoundaryValue;
    }

    private int ratioValueTotal = 0;
    private List<chanceBoundaries> chanceBoundariesList = new List<chanceBoundaries>();
    private List<SpawnableObjectsByLevel<T>> spawnableObjectsByLevelList;

    /// <summary>
    /// Constructor
    /// </summary>
    public RandomSpawnableObject(List<SpawnableObjectsByLevel<T>> spawnableObjectsByLevelList)
    {
        this.spawnableObjectsByLevelList = spawnableObjectsByLevelList;
    }

    public T GetItem()
    {
        int upperBoundary = -1;
        ratioValueTotal = 0;
        chanceBoundariesList.Clear();
        T spawnableObject = default(T);

        foreach (SpawnableObjectsByLevel<T> spawnableObjectsByLevel in spawnableObjectsByLevelList)
        {
            // check for current level
            if (spawnableObjectsByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel())
            {
                foreach (SpawnableObjectRatio<T> spawnableObjectRatio in spawnableObjectsByLevel.spawnableObjectRatioList)
                {
                    int lowerBoundary = upperBoundary + 1;

                    upperBoundary = lowerBoundary + spawnableObjectRatio.ratio - 1;

                    ratioValueTotal += spawnableObjectRatio.ratio;

                    // Add spawnable object to list;
                    chanceBoundariesList.Add(new chanceBoundaries() { spawnableObject = spawnableObjectRatio.dungeonObject, lowBoundaryValue = lowerBoundary, highBoundaryValue = upperBoundary });

                }
            }
        }

        if (chanceBoundariesList.Count == 0) return default(T);

        int lookUpValue = Random.Range(0, ratioValueTotal);

        // loop through list to get seleted random spawnable object details
        foreach (chanceBoundaries spawnChance in chanceBoundariesList)
        {
            if (lookUpValue >= spawnChance.lowBoundaryValue && lookUpValue <= spawnChance.highBoundaryValue)
            {
                spawnableObject = spawnChance.spawnableObject;
                break;
            }
        }


        return spawnableObject;
    }

}
