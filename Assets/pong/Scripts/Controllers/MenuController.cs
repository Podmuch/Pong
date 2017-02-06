using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
	public GameObject window1;
	public GameObject window2;
    public GameObject window3;
    public GameObject window4;
    public tk2dSlicedSprite leftPlayerColorSprite;
    public tk2dSlicedSprite rightPlayerColorSprite;

	void Awake()
	{
		window1.SetActive(true);
		window2.SetActive(false);
        window3.SetActive(false);
        window4.SetActive(false);
	}

    void OnStartButtonClick()
    {
        AnimateHideWindow(window1.transform);
        AnimateShowWindow(window3.transform);
    }

    void OnBackButton2Click()
    {
        AnimateHideWindow(window3.transform);
        AnimateShowWindow(window1.transform);
    }

	void OnOptionsClick()
	{
        AnimateHideWindow(window1.transform);
        AnimateShowWindow(window2.transform);
	}

    void OnMultiplayerClick()
    {
        AnimateHideWindow(window3.transform);
        AnimateShowWindow(window4.transform);
    }

    void OnBackButton3Click()
    {
        AnimateHideWindow(window4.transform);
        AnimateShowWindow(window3.transform);
    }

	void OnBackButtonClick()
	{
        Color tmpColor = rightPlayerColorSprite.color;
        PlayerPrefs.SetString("RightPlayerColor", tmpColor.r.ToString() + "@" + tmpColor.g.ToString() + "@" + tmpColor.b.ToString());
        tmpColor = leftPlayerColorSprite.color;
        PlayerPrefs.SetString("LeftPlayerColor", tmpColor.r.ToString() + "@" + tmpColor.g.ToString() + "@" + tmpColor.b.ToString());
        AnimateHideWindow(window2.transform);
        AnimateShowWindow(window1.transform);
	}

    void AnimateShowWindow(Transform t)
    {
        t.gameObject.SetActive(true);
        StartCoroutine(coTweenTransformTo(t, 0.5f, new Vector3(-150, 0, 0), Vector3.zero, 
                         new Vector3(0, 0, 90), Vector3.zero, Vector3.one, Vector3.zero));
    }

    void AnimateHideWindow(Transform t)
    {
        StartCoroutine(coAnimateHideWindow(t));
    }

    IEnumerator coAnimateHideWindow(Transform t)
    {
        yield return StartCoroutine(coTweenTransformTo(t, 0.5f, Vector3.zero, Vector3.one, 
                        Vector3.zero, new Vector3(150,0,0), Vector3.zero, new Vector3(0,0,90)));
        t.gameObject.SetActive(false);
    }

    IEnumerator coTweenTransformTo(Transform transform, float time, Vector3 fromPos, Vector3 fromScale, Vector3 fromEuler, Vector3 toPos, Vector3 toScale, Vector3 toEuler)
    {
        transform.localPosition = fromPos;
        transform.localScale = fromScale;
        transform.localEulerAngles = fromEuler;
        iTween.MoveTo(transform.gameObject, iTween.Hash("x", toPos.x, "y", toPos.y, "z", toPos.z, "easeType", "linear", "Time", time));
        iTween.ScaleTo(transform.gameObject, iTween.Hash("x", toScale.x, "y", toScale.y, "z", toScale.z, "easeType", "linear", "Time", time));
        iTween.RotateTo(transform.gameObject, iTween.Hash("x", toEuler.x, "y", toEuler.y, "z", toEuler.z, "easeType", "linear", "Time", time));
        for (float timer = 0; timer < time; timer += Time.deltaTime)
            yield return 0;
    }

    void OnSingleplayerClick()
    {
        PlayerPrefs.SetString("mode", "singleplayer");
        SceneManager.instance.LoadLevel("pong");
    }
}
