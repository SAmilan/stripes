using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCounter : MonoBehaviour
{
    public void PlayCountDownSound()
    {
        SoundManager.Instance.PlayCountDownSound();
    }
    public void PlayGOSound()
    {
        SoundManager.Instance.PlayGOSound();
    }
}
