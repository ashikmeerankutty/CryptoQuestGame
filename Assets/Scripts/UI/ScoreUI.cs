using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI scoreTextTMP;

    private void Awake()
    {
        scoreTextTMP = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        // Subscribe to the score changed event
        StaticEventHandler.OnScoreChanged += StaticEventHandler_OnScoreChanged;
    }

    private void OnDisable()
    {
        // Unsubscribe from the score changed event
        StaticEventHandler.OnScoreChanged -= StaticEventHandler_OnScoreChanged;
    }

    // Handle score changed event
    private void StaticEventHandler_OnScoreChanged(ScoreChangedArgs scoreChangedArgs)
    {
        // Update UI
        scoreTextTMP.text = "SCORE: " + scoreChangedArgs.score.ToString("###,###0") + "\nMULTIPLIER: x" + scoreChangedArgs.multiplier;
    }

}
