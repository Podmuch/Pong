using UnityEngine;
using System.Collections;
using System;

public class GameController : Photon.MonoBehaviour
{
    public static GameController instance { get; private set; }
    public tk2dTextMesh text;
    public tk2dSprite opponentColor;
    public tk2dSprite leftPlayerColor;
    public tk2dTextMesh endMessage;
    public GameObject leftPlayerPrefab;
    public GameObject rightPlayerPrefab;
    public GameObject ballStatic;
    bool isSinglePlayer;
	//0 -left, 1 - right
	private int[] points;

    void Awake()
    {
        if (!photonView.isMine)
            enabled = false;
        instance = this;
        points = new int[2];
        isSinglePlayer = PlayerPrefs.GetString("mode") == "singleplayer";
        if (isSinglePlayer)
        {
            string color = PlayerPrefs.GetString("RightPlayerColor");
            if (color != "")
            {
                string[] rgb = color.Split('@');
                opponentColor.color = new Color(Convert.ToSingle(rgb[0]), Convert.ToSingle(rgb[1]), Convert.ToSingle(rgb[2]));
            }
            Instantiate(ballStatic);
        }
    }

    void OnJoinedRoom()
    {
        Destroy(leftPlayerColor.gameObject);
        Destroy(opponentColor.gameObject);
        Spawnplayer();
    }

    void Spawnplayer()
    {
        Player tmp;
        if (NetworkManager.instance.isMaster)
            tmp = PhotonNetwork.Instantiate(leftPlayerPrefab.name, leftPlayerPrefab.transform.position, leftPlayerPrefab.transform.rotation, 0).GetComponent<Player>();
        else
        {
            tmp = PhotonNetwork.Instantiate(rightPlayerPrefab.name, rightPlayerPrefab.transform.position, rightPlayerPrefab.transform.rotation, 0).GetComponent<Player>();
            PhotonNetwork.Instantiate(ballStatic.name, ballStatic.transform.position, ballStatic.transform.rotation, 0);
        }
        tmp.transform.parent = Ball.instance.transform.parent;
    }

    public void Goal(bool isLeftGate)
    {
		Ball.instance.isLeft = isLeftGate;
        Ball.instance.isStart = true;
        Ball.instance.rigidbody.velocity = Vector3.zero;
        if (PhotonNetwork.isMasterClient || isSinglePlayer)
        {
            if (!isLeftGate)
                points[0]++;
            else
                points[1]++;
        }
		text.text = points [0].ToString() + ":" + points [1].ToString();
        if (points[0] == 5 || points[1] == 5)
            EndGame();
	}

    public void EndGame()
    {
        string message=((points[0]>points[1])?"Left":"Right")+" Player win game! Congratulations!!";
        endMessage.text=message;
        Time.timeScale = 0;
        iTween.MoveTo(gameObject, iTween.Hash("y", 1, "ignoretimescale", true, "easeType", "linear", "Time", 0.3f));
    }

    void ExitButtonOnClick()
    {
        
        Time.timeScale = 1;
        if (!isSinglePlayer)
        {
            PhotonNetwork.Disconnect();
            Destroy(NetworkManager.instance.gameObject);
        }
        SceneManager.instance.LoadLevel("menu");
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(Time.timeScale);
            stream.SendNext(endMessage.text);
            stream.SendNext(text.text);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            Time.timeScale = (float)stream.ReceiveNext();
            endMessage.text = (string)stream.ReceiveNext();
            text.text = (string)stream.ReceiveNext();
        }
    }

    void OnPhotonPlayerDisconnected()
    {
        if (Time.timeScale == 0)
            PauseController.instance.gameObject.SetActive(false);
        EndGame();
    }
}
