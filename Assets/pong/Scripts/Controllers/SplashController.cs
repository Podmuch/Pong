using UnityEngine;
using System.Collections;

public class SplashController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SceneManager.instance.LoadLevel("menu");
	}
}
