using UnityEngine;
using System.Collections;

public class Explosion : Photon.MonoBehaviour {
    void Start()
    {
        StartCoroutine(Destroy());
    }
    IEnumerator Destroy()
    {
        Ball.instance.transform.position += new Vector3(0, 200, 0);
        for (float timer = 0; timer < 0.5f; timer += Time.deltaTime)
            yield return 0;
        Destroy(gameObject);
    }
}
