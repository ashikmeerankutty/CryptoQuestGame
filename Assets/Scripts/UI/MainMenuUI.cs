using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    #region Header OBJECT REFERENCES
    [Space(10)]
    [Header("OBJECT REFERENCES")]
    #endregion Header OBJECT REFERENCES
    #region Tooltip
    [Tooltip("Populate with the enter the dungeon play button gameobject")]
    #endregion Tooltip
    [SerializeField] private GameObject playButton;
    #region Tooltip
    [Tooltip("Populate with the quit button gameobject")]
    #endregion
    [SerializeField] private GameObject quitButton;
    #region Tooltip
    [Tooltip("Populate with the high scores button gameobject")]
    #endregion
    [SerializeField] private GameObject highScoresButton;
    #region Tooltip
    [Tooltip("Populate with the instructions button gameobject")]
    #endregion
    [SerializeField] private GameObject instructionsButton;
    #region Tooltip
    [Tooltip("Populate with the return to main menu button gameobject")]
    #endregion
    [SerializeField] private GameObject returnToMainMenuButton;
     #region Tooltip
    [Tooltip("Populate with the wallet connection button gameobject")]
    #endregion
    private bool isInstructionSceneLoaded = false;
    private bool isHighScoresSceneLoaded = false;
    private bool isWalletConnectionSceneLoaded = false;

    private void Start()
    {
        // Play Music
        MusicManager.Instance.PlayMusic(GameResources.Instance.mainMenuMusic, 0f, 2f);

        // Load Character selector scene additively
        SceneManager.LoadScene("CharacterSelectorScene", LoadSceneMode.Additive);

        returnToMainMenuButton.SetActive(false);
    }


    /// <summary>
    /// Called from the Play Game / Enter The Dungeon Button
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene("MainGameScene");
    }


    /// <summary>
    /// Called from the High Scores Button
    /// </summary>
    public void LoadHighScores()
    {
        playButton.SetActive(false);
        quitButton.SetActive(false);
        highScoresButton.SetActive(false);
        instructionsButton.SetActive(false);
        // walletConnectionButton.SetActive(false);
        isHighScoresSceneLoaded = true;

        SceneManager.UnloadSceneAsync("CharacterSelectorScene");

        returnToMainMenuButton.SetActive(true);

        // Load High Score scene additively
        SceneManager.LoadScene("HighScoreScene", LoadSceneMode.Additive);
    }

    /// <summary>
    /// Called from the Return To Main Menu Button
    /// </summary>
    public void LoadCharacterSelector()
    {
        returnToMainMenuButton.SetActive(false);

        if (isHighScoresSceneLoaded)
        {
            SceneManager.UnloadSceneAsync("HighScoreScene");
            isHighScoresSceneLoaded = false;
        }
        else if (isInstructionSceneLoaded)
        {
            SceneManager.UnloadSceneAsync("InstructionsScene 1");
            isInstructionSceneLoaded = false;
        }
        // else if (isWalletConnectionSceneLoaded)
        // {
        //     SceneManager.UnloadSceneAsync("WalletConnectionScene");
        //     isWalletConnectionSceneLoaded = false;
        // }

        playButton.SetActive(true);
        quitButton.SetActive(true);
        highScoresButton.SetActive(true);
        instructionsButton.SetActive(true);
        // walletConnectionButton.SetActive(true);

        // Load character selector scene additively
        SceneManager.LoadScene("CharacterSelectorScene", LoadSceneMode.Additive);
    }

    /// <summary>
    /// Called from the Instructions Button
    /// </summary>
    public void LoadInstructions()
    {
        playButton.SetActive(false);
        quitButton.SetActive(false);
        highScoresButton.SetActive(false);
        instructionsButton.SetActive(false);
        // walletConnectionButton.SetActive(false);
        isInstructionSceneLoaded = true;

        SceneManager.UnloadSceneAsync("CharacterSelectorScene");

        returnToMainMenuButton.SetActive(true);

        // Load instructions scene additively
        SceneManager.LoadScene("InstructionsScene 1", LoadSceneMode.Additive);
    }

    /// <summary>
    /// Called from the Instructions Button
    /// </summary>
    public void LoadWalletConnection()
    {
        playButton.SetActive(false);
        quitButton.SetActive(false);
        highScoresButton.SetActive(false);
        instructionsButton.SetActive(false);
        // walletConnectionButton.SetActive(false);
        isWalletConnectionSceneLoaded = true;

        SceneManager.UnloadSceneAsync("CharacterSelectorScene");

        returnToMainMenuButton.SetActive(true);

        // Load instructions scene additively
        SceneManager.LoadScene("WalletConnectionScene", LoadSceneMode.Additive);
    }

    /// <summary>
    /// Quit the game - this method is called from the onClick event set in the inspector
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }


    #region Validation
#if UNITY_EDITOR
    // Validate the scriptable object details entered
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(playButton), playButton);
        HelperUtilities.ValidateCheckNullValue(this, nameof(quitButton), quitButton);
        HelperUtilities.ValidateCheckNullValue(this, nameof(highScoresButton), highScoresButton);
        HelperUtilities.ValidateCheckNullValue(this, nameof(instructionsButton), instructionsButton);
        HelperUtilities.ValidateCheckNullValue(this, nameof(returnToMainMenuButton), returnToMainMenuButton);
    }
#endif
    #endregion
}
