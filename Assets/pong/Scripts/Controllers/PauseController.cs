using UnityEngine;
using System.Collections;

public class PauseController : Photon.MonoBehaviour
{
    public static PauseController instance { get; private set; }
    bool isPause;
    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;
    bool isSinglePlayer;
    void Awake()
    {
        instance = this;
        isSinglePlayer = PlayerPrefs.GetString("mode") == "singleplayer";
        isPause = false;
        syncStartPosition=transform.position;
        syncEndPosition = transform.position;
    }
	// Use this for initialization
	void StartPause () {
        Time.timeScale=0;
        iTween.MoveTo(gameObject, iTween.Hash("y", 1, "ignoretimescale", true, "easeType", "linear", "Time", 0.3f));
	}

    void Update()
    {
        if (isSinglePlayer)
            Pause();
        else
        {
            if (PhotonNetwork.isMasterClient)
                PhotonView.Get(this).RPC("Pause", PhotonTargets.All);

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

    public void EndPause() 
    {
        iTween.MoveTo(gameObject, iTween.Hash("y", 100, "ignoretimescale", true, "easeType", "linear", "Time", 0.3f, "oncomplete", "End_Animation"));
    }

    void End_Animation()
    {
        Time.timeScale = 1;
    }

    void BackButtonOnClick()
    {
        Time.timeScale = 1;
        if (!isSinglePlayer)
        {
            PhotonNetwork.Disconnect();
            Destroy(NetworkManager.instance.gameObject);
        }
        SceneManager.instance.LoadLevel("menu");
    }
    private void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isPause)
                StartPause();
            else
                EndPause();
            isPause = !isPause;
        }
    }

    [RPC]private void Pause(PhotonMessageInfo info)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isPause)
                StartPause();
            else
                EndPause();
            isPause = !isPause;
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(Time.timeScale);
        }
        else
        {
            Vector3 syncPosition = (Vector3)stream.ReceiveNext();
            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;
            syncStartPosition = transform.position;
            syncEndPosition = syncPosition;
            Time.timeScale = (float)stream.ReceiveNext();
        }
    }
}
