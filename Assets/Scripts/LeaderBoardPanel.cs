using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject m_LeaderBoardEntryParent;
    [SerializeField]
    private GameObject m_LeaderBoardEntryPrefab;
    #region Unity_Callbacks
    

    #endregion

    #region Private_Methods

    /*private void DownloadScores()
    {
        LB_Controller.instance.ReloadLeaderboard();
    }
    private void RemoveAllUIEntries()
    {
        foreach (Transform child in m_LeaderBoardEntryParent.transform)
        {
            if (child.gameObject.tag == "LeaderBoardRowEntry")
            {
                Destroy(child.gameObject);
            }
        }
    }
    private void OnLeaderboardUpdated(LB_Entry[] entries)
    {
        if (entries != null && entries.Length > 0)
        {
            RemoveAllUIEntries();
            foreach (LB_Entry entry in entries)
            {
                GameObject entryRow = Instantiate(m_LeaderBoardEntryPrefab, m_LeaderBoardEntryParent.transform);
                LeaderboardDetails rowEntry = entryRow.GetComponent<LeaderboardDetails>();
                rowEntry.SetData(entry);
            }
        }
        else if (entries == null)
        {
            Debug.Log("ups something went wrong");
        }
    }*/

    #endregion

}
