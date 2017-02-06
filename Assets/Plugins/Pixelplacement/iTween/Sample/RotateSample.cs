using UnityEngine;
using System.Collections;

public class RotateSample : MonoBehaviour
{	
	void Start(){
		iTween.MoveBy(gameObject, iTween.Hash("y", 100, "easeType", "linear", "loopType", "loop", "Time", 10, "oncomplete", "function"));
		iTween.RotateBy(gameObject, iTween.Hash("z", 1, "easeType", "linear", "loopType", "loop", "Time", 10));
	}
	void function(){
		transform.position +=new Vector3(0,100,0);
	}
}

