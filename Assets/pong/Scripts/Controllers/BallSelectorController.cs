using UnityEngine;
using System.Collections;

public class BallSelectorController : MonoBehaviour {

    public tk2dUIToggleButton[] availableBalls;

    void Awake()
    {
        availableBalls[0].IsOn = false;
        availableBalls[1].IsOn = false;
        availableBalls[2].IsOn = false;
        availableBalls[3].IsOn = false;
        availableBalls[4].IsOn = false;
        availableBalls[PlayerPrefs.GetInt("Ball")].IsOn = true;
    }
    void FirstBallSelected()
    {
        availableBalls[0].IsOn = true;
        availableBalls[1].IsOn = false;
        availableBalls[2].IsOn = false;
        availableBalls[3].IsOn = false;
        availableBalls[4].IsOn = false;
        PlayerPrefs.SetInt("Ball", 0);
    }

    void SecondBallSelected()
    {
        availableBalls[0].IsOn = false;
        availableBalls[1].IsOn = true;
        availableBalls[2].IsOn = false;
        availableBalls[3].IsOn = false;
        availableBalls[4].IsOn = false;
        PlayerPrefs.SetInt("Ball", 1);
    }

    void ThirdBallSelected()
    {
        availableBalls[0].IsOn = false;
        availableBalls[1].IsOn = false;
        availableBalls[2].IsOn = true;
        availableBalls[3].IsOn = false;
        availableBalls[4].IsOn = false;
        PlayerPrefs.SetInt("Ball", 2);
    }

    void FourthBallSelected()
    {
        availableBalls[0].IsOn = false;
        availableBalls[1].IsOn = false;
        availableBalls[2].IsOn = false;
        availableBalls[3].IsOn = true;
        availableBalls[4].IsOn = false;
        PlayerPrefs.SetInt("Ball", 3);
    }

    void FifthBallSelected()
    {
        availableBalls[0].IsOn = false;
        availableBalls[1].IsOn = false;
        availableBalls[2].IsOn = false;
        availableBalls[3].IsOn = false;
        availableBalls[4].IsOn = true;
        PlayerPrefs.SetInt("Ball", 4);
    }
}
