using UnityEngine;
using System.Collections;

public class Ball : Photon.MonoBehaviour
{

    public static Ball instance { get; private set; }
    public bool isStart;
    public bool isLeft;
    public Vector3 ballDiameter;
    public Explosion explosionPrefab;
    float initMinSpeedX = 25.0f;
    float initMaxSpeed = 60.0f;
    float minSpeedX;
    float maxSpeed;
    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition;
    private Vector3 syncEndPosition;

    bool isSinglePlayer;

    void Awake()
    {
        syncStartPosition = transform.position;
        syncEndPosition = transform.position;
        instance = this;
        ballDiameter = new Vector3(2.5f, 0, 0);
        isSinglePlayer = PlayerPrefs.GetString("mode") == "singleplayer";
        isLeft = isSinglePlayer ? Random.Range(0.0f, 1.0f) > 0.5f : true;
        isStart = true;
    }
    // Use this for initialization
    void Start()
    {
        int ballNumber = PlayerPrefs.GetInt("Ball");
        SelectRightBall(ballNumber);
        minSpeedX = initMinSpeedX;
        maxSpeed = initMaxSpeed;
        StartCoroutine(ChangeMaxSpeed(3.0f));
    }

    void Update()
    {
        if (PhotonNetwork.isMasterClient || isSinglePlayer)
        {
            if (!isStart && Mathf.Abs(rigidbody.velocity.x) < minSpeedX)
                rigidbody.velocity = new Vector3(rigidbody.velocity.x < 0 ? -minSpeedX : minSpeedX, rigidbody.velocity.y, rigidbody.velocity.z);
            if (!isStart && rigidbody.velocity.magnitude > maxSpeed)
                rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
        }
        else
            SyncedMovement();
    }

    void SyncedMovement()
    {
        if (Time.timeScale != 0)
        {
            syncTime += Time.deltaTime;
            transform.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
        }
        else
            transform.position = syncEndPosition;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Gates")
        {
            minSpeedX = initMinSpeedX;
            maxSpeed = initMaxSpeed;
            if (isSinglePlayer)
                Instantiate(explosionPrefab, transform.position, transform.rotation);
            else
                PhotonNetwork.Instantiate(explosionPrefab.name, transform.position, transform.rotation, 0);
            GameController.instance.Goal(transform.position.x < 0);
        }
    }

    IEnumerator ChangeMaxSpeed(float frequency)
    {
        while (true)
        {
            for (float timer = 0; timer < frequency; timer += Time.deltaTime)
                yield return 0;
            if (!isStart && minSpeedX < 100)
            {
                maxSpeed += 1.0f;
                minSpeedX += 1.0f;
            }
        }
    }

    void SelectRightBall(int ballNumber)
    {
        tk2dSprite sprite = GetComponent<tk2dSprite>();
        switch (ballNumber)
        {
            case 0:
                sprite.color = new Color(0, 1.0f, 0);
                break;
            case 1:
                sprite.color = new Color(1.0f, 0, 0);
                break;
            case 2:
                sprite.color = new Color(0, 0, 1.0f);
                break;
            case 3:
                sprite.spriteId += 2;
                sprite.color = new Color(1.0f, 0.682f, 0);
                break;
            case 4:
                sprite.spriteId += 1;
                sprite.color = new Color(0, 0.835f, 1.0f);
                break;
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Vector3 syncPosition = Vector3.zero;
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(isLeft);
            stream.SendNext(isStart);
        }
        else
        {
            syncPosition = (Vector3)stream.ReceiveNext();
            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;

            syncStartPosition = transform.position;
            syncEndPosition = syncPosition;
            this.isLeft = (bool)stream.ReceiveNext();
            this.isStart = (bool)stream.ReceiveNext();
        }
    }
}
