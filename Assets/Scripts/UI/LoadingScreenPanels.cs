using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenPanels : UIView 
{
    [SerializeField]
    UIView[] m_PlayeViews;
    private void OnEnable()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1.5f);
        ViewController.instance.ChangeView(m_PlayeViews[GameManager.instace.CurrentGame]);
    }
}
