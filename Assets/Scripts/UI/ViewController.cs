using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ViewController : MonoBehaviour
{
    #region VARS

    //Instance
    public static ViewController instance;

    //Views
    [SerializeField]
    private UIView m_ViewHomePage;
    [SerializeField]
    private UIView m_ViewSinglePlayerGame;
    [SerializeField]
    private UIView m_ViewTwoPlayerGame;
    [SerializeField]
    private UIView m_OptionPanel;
    [SerializeField]
    private UIView m_TwoPlayerGameOverPanel;
    [SerializeField]
    private UIView m_SinglePlayerGameOverPanel;
    [SerializeField]
    private UIView m_TwoPlayerLoopCompleteGameOver;

    //Current View
    private UIView m_CurrentView;

    #endregion

    #region UNITY_CALLBACKS
    void Start()
    {
        ChangeView(m_ViewHomePage);
    }

    private void Awake()
    {
        instance = this;
    }


    #endregion

    #region PUBLIC_METHODS

    /// <summary>
    /// Changes the view.
    /// </summary>
    /// <param name="targetView">Target view.</param>
    public void ChangeView(UIView targetView)
    {
        m_ViewHomePage.Hide();
        m_ViewSinglePlayerGame.Hide();
        m_ViewTwoPlayerGame.Hide();
        m_OptionPanel.Hide();
        m_TwoPlayerGameOverPanel.Hide();
        m_SinglePlayerGameOverPanel.Hide();
        m_TwoPlayerLoopCompleteGameOver.Hide();
        if (m_CurrentView != null)
            m_CurrentView.Hide();
        m_CurrentView = targetView;
        m_CurrentView.Show();
    }

    public void ChangeTwoPlayerView(UIView targetView)
    {
        m_ViewHomePage.Hide();
        m_CurrentView = targetView;
        m_CurrentView.Show();
    }


    #endregion

    #region PRIVATE_METHODS
    

    #endregion

}

