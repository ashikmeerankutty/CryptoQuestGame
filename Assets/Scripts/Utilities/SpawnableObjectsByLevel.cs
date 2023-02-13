using System.Collections.Generic;

[System.Serializable]
public class SpawnableObjectsByLevel<T>
{
    public DungeonLevelSO dungeonLevel;
    public List<SpawnableObjectRatio<T>> spawnableObjectRatioList;

}