using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardDetails : MonoBehaviour
{
    #region Private_vars
    [SerializeField]
    private TextMeshProUGUI m_PlayerRank;
    [SerializeField]
    private TextMeshProUGUI m_PlayerName;
    [SerializeField]
    private TextMeshProUGUI m_PlayerScore;
    [SerializeField]
    private Sprite[] m_BgImages;
    [SerializeField]
    private Image m_CurrentBgImage;
    #endregion

    #region Public_Methods
    public void SetData(LeaderBoardData entry)
    {
        m_PlayerRank.text = entry.playerRank + ".";
        m_CurrentBgImage.sprite = (entry.playerRank % Constants.INT_TWO == Constants.INT_ZERO) ? m_BgImages[Constants.INT_ONE] : m_BgImages[Constants.INT_ZERO];
        m_PlayerName.text = entry.playerName;
        m_PlayerScore.text = entry.playerScore.ToString();
    }

    #endregion


}
