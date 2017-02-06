using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    public Transform pausePrefab;
    public static SceneManager instance { get; private set; }
    string currentScene;
	// Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

	void OnLevelWasLoaded(int level) 
	{
        switch (Application.loadedLevelName)
		{
		case "betweenScene":
			Application.LoadLevel(currentScene);
            break;
		default:
            iTween.ColorTo(gameObject, iTween.Hash("a", -0.6f, "ignoretimescale", true, "easeType", "linear", "loopType", "none", "time", 5));
			break;
		}
	}

    void End_Animation()
	{
		Application.LoadLevel ("betweenScene");
	}

    public void LoadLevel(string level)
    {
        currentScene = level;
        iTween.ColorTo(gameObject, iTween.Hash("a", 0.6f, "ignoretimescale", true, "easeType", "linear", "loopType", "none", "time", 3, "oncomplete", "End_Animation"));
    }
}
