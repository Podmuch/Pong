using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance { get; private set; }
    private string roomName = "RoomName";
    private List<GameObject> roomList;
    public Transform lastItem;
    public GameObject recordPrefab;
    public tk2dUIScrollableArea area;
    public tk2dTextMesh roomNameInput;
    public bool isMaster;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (!PhotonNetwork.connected)
            PhotonNetwork.ConnectUsingSettings("1.0");
        PhotonNetwork.playerName = "Guest" + Random.Range(1, 9999);
        instance = this;
        roomList = new List<GameObject>();
    }

    void OnCreateRoomClick()
    {
		roomName = roomNameInput.text;
        isMaster = true;
        PlayerPrefs.SetString("mode", "multiplayer");
        SceneManager.instance.LoadLevel("pong");
    }

    void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList.ToArray()[i]);
        }
        roomList.Clear();
    }

    void RefreshRoomList()
    {
        ClearRoomList();
        lastItem.transform.position = Vector3.zero;
        float delta = 0;
        foreach (RoomInfo game in PhotonNetwork.GetRoomList())
        {
            if (game.maxPlayers != game.playerCount)
            {
                GameObject record = (GameObject)Instantiate(recordPrefab);
                record.transform.parent = area.contentContainer.transform;
                record.transform.localPosition += new Vector3(0, delta, 0);
                delta -= 5;
                record.transform.FindChild("HostName").GetComponent<tk2dTextMesh>().text = game.name;
                tk2dUIItem button = record.transform.FindChild("JoinButton").GetComponent<tk2dUIItem>();
                string roomName = game.name;
                button.OnClick += () =>
                {
                    isMaster = false;
                    PlayerPrefs.SetString("mode", "multiplayer");
                    SceneManager.instance.LoadLevel("pong");
                    this.roomName = roomName;
                };
                roomList.Add(record);
            }
        }
        lastItem.transform.localPosition = new Vector3(0, delta-8, 0);
        area.ContentLength =Mathf.Abs(delta-10);
    }

    void OnLevelWasLoaded(int level)
    {
        switch (Application.loadedLevelName)
        {
            case "pong":
                if (PlayerPrefs.GetString("mode") == "multiplayer")
                {
                    if (isMaster)
                        PhotonNetwork.CreateRoom(roomName, new RoomOptions() { maxPlayers = 2 }, null);
                    else
                        PhotonNetwork.JoinRoom(roomName);
                }
                else
                {
                    PhotonNetwork.Disconnect();
                    Destroy(gameObject);
                }
                break;
        }
    }

    void OnPhotonJoinRoomFailed()
    {
        GameController.instance.EndGame();
        GameController.instance.endMessage.text="Join to Room Failed";
    }

    void OnPhotonCreateRoomFailed()
    {
        GameController.instance.EndGame();
        GameController.instance.endMessage.text="Create Room Failed";
    }
}
