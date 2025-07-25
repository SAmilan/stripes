using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewHomePage : UIView
{
    #region Private_Variables
    [SerializeField]
    private Image m_LoadingPipe;
    [SerializeField]
    private Image[] m_TitleCharachters;
    #endregion

    #region Public_Variables
    #endregion

    #region Unity_Callbacks
    // Start is called before the first frame update
    void  Awake()
    {
        StartCoroutine(FillImageAnimation(m_LoadingPipe));
        StartCoroutine(AnimateTitleCharachters());
    }
    /// <summary>
    /// To animate the characters of the title sequentially
    /// </summary>
    /// <returns></returns>
    private IEnumerator AnimateTitleCharachters()
    {
        foreach (Image target in m_TitleCharachters)
        {
            StartCoroutine(FillImageAnimation(target));
            yield return new WaitForSeconds(1f);
        }
    }
    #endregion

    #region Public_Methods
    #endregion

    #region Private_Methods
    #endregion

    #region Coroutines

    /// <summary>
    /// For animating the pipes in the home panel
    /// </summary>
    /// <returns></returns>
    private IEnumerator FillImageAnimation(Image targetImage)
    {
        targetImage.fillAmount = Constants.ZERO;
        float fillingAmount = Constants.ZERO;
        while(fillingAmount < 1)
        {
            fillingAmount += Constants.HOMEPIPEDELTA;
            yield return new WaitForSeconds(Constants.HOMEPIPEDELTA);
            targetImage.fillAmount = fillingAmount;
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

}
