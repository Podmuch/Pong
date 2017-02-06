using UnityEngine;
using System.Collections;
using System;

public class Player : Photon.MonoBehaviour
{
	public bool isLeft;
    public Transform topBlocker;
    public Transform bottomBlocker;
	protected float topBorder;
    protected float bottomBorder;
    protected Ball ball;
    Vector3 speed;
    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;
    bool isSinglePlayer;
	public bool isMoveUp
	{
		get
		{
			return Time.timeScale != 0 && transform.position.y<topBorder &&
                ((isLeft && Input.GetKey(KeyCode.W))|| (!isLeft && Input.GetKey(KeyCode.UpArrow)));
		}
	}

	public bool isMoveDown
	{
		get
		{
			return Time.timeScale != 0&&transform.position.y>bottomBorder&&
                ((isLeft && Input.GetKey(KeyCode.S)) ||(!isLeft && Input.GetKey(KeyCode.DownArrow)));
		}
	}

	virtual public bool isShoot
	{
		get 
		{
			return Time.timeScale!=0&&ball.isStart&&isLeft==ball.isLeft&&
                ((isLeft && Input.GetKey(KeyCode.D)) || (!isLeft && Input.GetKey(KeyCode.LeftArrow)));
		}
	}

    public bool ShootFlag;

	void Start()
	{
        ShootFlag = false;
        ball = Ball.instance;
        SetBorder();
        SetColor();
		speed = new Vector3 (0, 1, 0);
        isSinglePlayer=PlayerPrefs.GetString("mode") == "singleplayer";
        syncStartPosition = transform.position;
        syncEndPosition = transform.position;
	}

    void SetBorder()
    {
        float delta = GetComponent<CapsuleCollider>().height * 0.5f;
        foreach (BoxCollider bc in FindObjectsOfType<BoxCollider>())
        {
            if (bc.tag == "Blocker")
            {
                if (bc.transform.position.y > ball.transform.position.y)
                    topBlocker = bc.transform;
                else
                    bottomBlocker = bc.transform;
            }
        }
        topBorder = topBlocker.position.y - delta;
        bottomBorder = bottomBlocker.position.y + delta;
    }

    void SetColor()
    {
        string color = PlayerPrefs.GetString("RightPlayerColor");
        if (color != "")
        {
            if (isLeft)
                color = PlayerPrefs.GetString("LeftPlayerColor");
            string[] rgb = color.Split('@');
            rgb = color.Split('@');
            GetComponent<tk2dSprite>().color = new Color(Convert.ToSingle(rgb[0]), Convert.ToSingle(rgb[1]), Convert.ToSingle(rgb[2]));
        }
    }

	// Update is called once per frame
	void Update () 
	{
        if (isSinglePlayer)
        {
            Move();
        }
        else
        {
            if (photonView.isMine)
                Move();
            else
                SyncedMovement();
        }
    }

    private void SyncedMovement()
    {
        if (Time.timeScale != 0)
        {
            syncTime += Time.deltaTime;
            transform.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
        }
        else
            transform.position = syncEndPosition;
    }

	void Move()
	{
        if (isMoveUp)
			transform.Translate(speed);
		if (isMoveDown)
            transform.Translate(-speed);
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(isShoot);
			ShootFlag=isShoot;
        }
        else
        {
            Vector3 syncPosition = (Vector3)stream.ReceiveNext();
            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;
            syncStartPosition = transform.position;
            syncEndPosition = syncPosition;
            transform.parent = ball.transform.parent;
            ShootFlag = (bool)stream.ReceiveNext();
        }
    }
}
