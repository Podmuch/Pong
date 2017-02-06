using UnityEngine;
using System.Collections;

public class Opponent : Player {
    Vector3 ballHitSpeed;

    override public bool isShoot
    {
        get
        {
            return Time.timeScale != 0 && ball.isStart && !ball.isLeft;
        }
    }

    public bool isMove
    {
        get
        {
            return Time.timeScale != 0 && ball.transform.position.y<topBlocker.position.y&&
                    (transform.position.y < topBorder || transform.position.y > ball.transform.position.y)&&
                    (transform.position.y > bottomBorder || transform.position.y < ball.transform.position.y);
        }
    }

	// Use this for initialization
	void Start () {
        ball = Ball.instance;
        ballHitSpeed = new Vector3(30, 0, 0);
        float delta = GetComponent<CapsuleCollider>().height * 0.5f;
        topBorder = topBlocker.position.y - delta;
        bottomBorder = bottomBlocker.position.y + delta;
        StartCoroutine(Move(0.5f));
        StartCoroutine(Shoot(2.0f));
	}

    void Update()
    {
    }

    IEnumerator Move(float frequency)
    {
        while (true)
        {
            for (float timer = 0; timer < frequency; timer += Time.deltaTime)
                yield return 0;
            if (isMove)
                iTween.MoveTo(gameObject, iTween.Hash("y", ball.transform.position.y + ball.rigidbody.velocity.y * frequency, "easeType", "linear", "Time", frequency));
        }
    }

    IEnumerator Shoot(float frequency)
    {
        while (true) 
        {
            for (float timer = 0; timer < frequency; timer += Time.deltaTime)
                yield return 0;
            if (isShoot)
            {
                ball.rigidbody.velocity = ball.isLeft ? ballHitSpeed : -ballHitSpeed;
                ball.isStart = false;
            }
        }
    }
}
