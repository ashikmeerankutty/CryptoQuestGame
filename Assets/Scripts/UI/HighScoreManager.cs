using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class HighScoreManager : SingletonMonobehaviour<HighScoreManager>
{
    private HighScores highScores = new HighScores();

    protected override void Awake()
    {
        base.Awake();

        LoadScores();
    }

    /// <summary>
    /// Load Scores From Disk
    /// </summary>
    private void LoadScores()
    {
        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(Application.persistentDataPath + "/DungeonGunnerHighScores.dat"))
        {
            ClearScoreList();

            FileStream file = File.OpenRead(Application.persistentDataPath + "/DungeonGunnerHighScores.dat");

            highScores = (HighScores)bf.Deserialize(file);

            file.Close();

        }
    }

    /// <summary>
    /// Clear All Scores
    /// </summary>
    private void ClearScoreList()
    {
        highScores.scoreList.Clear();
    }

    /// <summary>
    /// Add score to high scores list
    /// </summary>
    public void AddScore(Score score, int rank)
    {
        highScores.scoreList.Insert(rank - 1, score);

        // Maintain the maximum number of scores to save
        if (highScores.scoreList.Count > Settings.numberOfHighScoresToSave)
        {
            highScores.scoreList.RemoveAt(Settings.numberOfHighScoresToSave);
        }

        SaveScores();
    }

    /// <summary>
    /// Save Scores To Disk
    /// </summary>
    private void SaveScores()
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(Application.persistentDataPath + "/DungeonGunnerHighScores.dat");

        bf.Serialize(file, highScores);

        file.Close();
    }

    /// <summary>
    /// Get highscores
    /// </summary>
    public HighScores GetHighScores()
    {
        return highScores;
    }

    /// <summary>
    /// Return the rank of the playerScore compared to the other high scores (returns 0 if the score isn't higher than any in the high scores list)
    /// </summary>
    public int GetRank(long playerScore)
    {
        // If there are no scores currently in the list - then this score must be ranked 1 - then return
        if (highScores.scoreList.Count == 0) return 1;

        int index = 0;

        // Loop through scores in list to find the rank of this score
        for (int i = 0; i < highScores.scoreList.Count; i++)
        {
            index++;

            if (playerScore >= highScores.scoreList[i].playerScore)
            {
                return index;
            }
        }

        if (highScores.scoreList.Count < Settings.numberOfHighScoresToSave)
            return (index + 1);

        return 0;
    }
}