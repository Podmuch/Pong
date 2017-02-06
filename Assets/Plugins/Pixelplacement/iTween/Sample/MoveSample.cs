using UnityEngine;
using System.Collections;

public class MoveSample : MonoBehaviour
{	
	void Start(){
		iTween.MoveBy(gameObject, iTween.Hash("y", 100, "easeType", "linear", "loopType", "loop", "Time", 10, "oncomplete", "function"));
	}
	void function(){
		transform.position +=new Vector3(0,100,0);
	}
}

