using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    #region Private_variables
    [SerializeField]
    private int m_SinglePlayerScore;
    [SerializeField]
    private int m_TwoPlayerScore_Player1;
    [SerializeField]
    private int m_TwoPlayerScore_Player2;
    #endregion

    #region Public_Variables
    public static ScoreManager instance;
    public int SinglePlayerScore => m_SinglePlayerScore;
    public int TwoPlayerScore_Player1 => m_TwoPlayerScore_Player1;
    public int TwoPlayerScore_Player2 => m_TwoPlayerScore_Player2;
    #endregion

    #region Unity_CallBacks
    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        ViewSinglePlayerGame.s_OnScoreUpdates += OnSinglePlayerScoreUpdate;
        ViewTwoPlayerGame.s_OnPlayer1ScoreUpdates += OnTwoPlayerMode_Player1ScoreUpdates;
        ViewTwoPlayerGame.s_OnPlayer2ScoreUpdates += OnTwoPlayerMode_Player2ScoreUpdates;
    }
    private void OnDisable()
    {
        ViewSinglePlayerGame.s_OnScoreUpdates -= OnSinglePlayerScoreUpdate;
        ViewTwoPlayerGame.s_OnPlayer1ScoreUpdates -= OnTwoPlayerMode_Player1ScoreUpdates;
        ViewTwoPlayerGame.s_OnPlayer2ScoreUpdates -= OnTwoPlayerMode_Player2ScoreUpdates;
    }

    #endregion

    #region Private_methods
    private void OnSinglePlayerScoreUpdate(int currentScore)
    {
        m_SinglePlayerScore += currentScore;
        Debug.Log("Your Score is: " + m_SinglePlayerScore);
    }
    
    private void OnTwoPlayerMode_Player1ScoreUpdates(int currentScore)
    {
        m_TwoPlayerScore_Player1 += currentScore;
        Debug.Log("Player 1 Score is: " + m_TwoPlayerScore_Player1);
    }

    private void OnTwoPlayerMode_Player2ScoreUpdates(int currentScore)
    {
        m_TwoPlayerScore_Player2 += currentScore;
        Debug.Log("Player 2 score: " + m_TwoPlayerScore_Player2);

    }

    #endregion

}
