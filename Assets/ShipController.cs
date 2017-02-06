using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour
{	
	public Transform perspectiveCamera;
	public GameObject explosion;
	void Start(){
		iTween.MoveBy(gameObject, iTween.Hash("y", 100, "easeType", "linear", "loopType", "loop", "Time", 10, "oncomplete", "function"));
		iTween.RotateBy(gameObject, iTween.Hash("z", 1, "easeType", "linear", "loopType", "loop", "Time", 10));
	}
	void function(){
		transform.position +=new Vector3(0,0,0);
	}

	void Shoot(){
		Instantiate (explosion, transform.position+new Vector3(Random.Range(-5,5), Random.Range(-5,5),0), transform.rotation);
	}
	IEnumerator Hit() {
		StartCoroutine( coShake(perspectiveCamera, Vector3.one, Vector3.one, 1.0f ) );
		
		yield return new WaitForSeconds(0.3f);
	}
	IEnumerator coShake( Transform t, Vector3 translateConstraint, Vector3 rotationConstraint, float time ) {
		Vector3 pos = t.position;
		Quaternion rot = t.rotation;
		for (float ut = 0; ut < time; ut += Time.deltaTime) {
			float nt = Mathf.Clamp01( ut / time );
			float strength = 1 - nt;
			
			t.position = pos + Vector3.Scale(Random.onUnitSphere, translateConstraint).normalized * strength * 0.01f;
			t.rotation = rot;
			t.Rotate(Vector3.Scale(Random.onUnitSphere, rotationConstraint), 2.0f * strength);
			
			yield return 0;
		}
		t.position = pos;
		t.rotation = rot;
	}

	void OnCollisionEnter(Collision collision){
		gameObject.SendMessage ("Hit");
	} 
}
