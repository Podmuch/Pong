using UnityEngine;
using System.Collections;

public class BallStatic : Photon.MonoBehaviour{
    private static Player leftPlayer;
    private static Player rightPlayer;
    private static Ball ball;
    static Vector3 ballHitSpeed;
    bool isSinglePlayer;
    Vector3 lastballposition;

    void Start()
    {
        isSinglePlayer = PlayerPrefs.GetString("mode") == "singleplayer";
        if (isSinglePlayer)
        {
            leftPlayer = FindObjectOfType<Player>();
            rightPlayer = FindObjectOfType<Opponent>();
        }
        else
        {
            Player[] tmp = FindObjectsOfType<Player>();
            if (tmp[0].transform.position.x < 0)
            {
                leftPlayer = tmp[0];
                rightPlayer = tmp[1];
            }
            else
            {
                rightPlayer = tmp[0];
                leftPlayer = tmp[1];
            } 
        }
        ball = Ball.instance;
        ballHitSpeed = new Vector3(30, 0, 0);
    }

    void Update()
    {
        PhotonView.Get(this).RPC("Shoot", PhotonTargets.All);
        PhotonView.Get(this).RPC("FollowPlayer", PhotonTargets.All);
    }

    [RPC]public void Shoot()
    {
        if (isSinglePlayer)
        {
            leftPlayer.ShootFlag = leftPlayer.isShoot;
            rightPlayer.ShootFlag = rightPlayer.isShoot;
        }
        if ((leftPlayer.ShootFlag && ball.isLeft) || (rightPlayer.ShootFlag && !ball.isLeft))
        {
            ball.rigidbody.velocity = ball.isLeft ? ballHitSpeed : -ballHitSpeed;
            ball.isStart = false;
        }
    }

    [RPC]public void FollowPlayer()
    {
        if (ball.isStart)
        {
            lastballposition = (ball.isLeft ? leftPlayer.transform.position : rightPlayer.transform.position)
                                    + (ball.isLeft ? ball.ballDiameter : -ball.ballDiameter) + new Vector3(0, 0, 0.5f);
            ball.transform.position = lastballposition;
        }
    }
}
