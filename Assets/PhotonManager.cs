using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;
    public string roomName;

    //public List<RoomInfo> roomInfoList;

    public Dictionary<string, RoomInfo> roomListData;
    public GameObject pvpCommunicatorObject, connectingPanel;
    public bool thisIsMasterClient;
    //public PvPCommunicator myCommunicatorObject, otherCommunicatorObject;

    public string myPlayerID, otherPlayerID;

    //public List<PhantomPlanner.PhantomCompleteDetails> opponentDefenceLineDetails;
    //public List<PVPBattleManager.CharacterName_Pos_PowerUps> otherPlatformCharactersReceived, otherBattleCharactersReceived;



    [System.Serializable]
    public class PlayersInRoomDetails
    {
        public string nickName;
        public int actorNumber;
        public PlayersInRoomDetails(string nickName, int actorNumber)
        {
            this.nickName = nickName;
            this.actorNumber = actorNumber;
        }
    }
    //#region "UNITY_METHODS

    private void Start()
    {
        //myPlayerID = LoginManager.userID;
        ///Getting Phantom Details
    }
    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        thisIsMasterClient = PhotonNetwork.IsMasterClient;
        //Debug.LogError("Ecount: "+EnemyCharactersManager.Instance.enemyCharactersFighting.Count);
    }

    // #endregion

    #region "PhotonMethods" 

    public bool findRoomAlso;

    [ContextMenu("1_Login")]
    public void LoginToPhotonNetwork()
    {
        findRoomAlso = true;
        Debug.LogError("log in.............");
        connectingPanel.SetActive(true);

        if (PhotonNetwork.IsConnected)
        {
            Debug.LogError("AlreadyInLobby");
            FindOpponentPlayer();
        }

        else
        {
            PhotonNetwork.LocalPlayer.NickName = Random.Range(0,10).ToString();
            PhotonNetwork.ConnectUsingSettings();
        }
    }


    [ContextMenu("Quick Match")]
    public void FindOpponentPlayer()
    {
        Hashtable expectedCustomRoomProperties = new Hashtable { { regionNo, 1 }, 
            { maxPlayerCount, 2 } };

        Debug.LogError(expectedCustomRoomProperties);

        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 2);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError("No room found creating new");
        CreateRoomFun();
    }

    RoomOptions roomOptions;

    const string regionNo = "region";
    const string maxPlayerCount = "playerCount";
    public void CreateRoomFun()
    {

        roomOptions = new RoomOptions();
        roomOptions.CustomRoomPropertiesForLobby = new string[2] { regionNo, maxPlayerCount };
        roomOptions.CustomRoomProperties = new Hashtable { { regionNo, 1 }, 
            { maxPlayerCount, 2 } };roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(roomName + Random.Range(0, 500000), roomOptions);

    }

    [ContextMenu("Leave Room")]
    public void LeaveRoom()
    {
        // if (PhotonNetwork.InRoom)
        {
            Debug.LogError("Left Room");
            //Destroy(myCommunicatorObject);
            //myCommunicatorObject = null;
            PhotonNetwork.LeaveRoom(true);
        }
    }

    public void GetAllRooms()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }

        Debug.LogError(roomListData.Values);
    }

    #endregion

    #region "PhotonCallbacks"
    public override void OnConnected()
    {
        Debug.LogError("Connected to Internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogError(PhotonNetwork.LocalPlayer.NickName + " is connected to photon");

        if (findRoomAlso == true)
        {
            findRoomAlso = false;
            FindOpponentPlayer();
        }
        ///Activate lobby panel
    }

    public override void OnPlayerEnteredRoom(Player otherPlayer)
    {
        //Debug.LogError("ABCD");
        otherPlayerID = otherPlayer.NickName;
        Debug.LogError("make Room invisible for others");
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //playersInRoomDetails.Remove(playersInRoomDetails.Find(obj=>obj.actorNumber==otherPlayer.ActorNumber));
        Debug.LogError(otherPlayer.NickName + " Exited Room");
        //otherCommunicatorObject = null;
        //BattleManager.Instance.OpponentLeftTheRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.LogError("Room is Created");
    }

    public override void OnJoinedRoom()
    {
        Debug.LogError("Room is Joined: " + PhotonNetwork.CurrentRoom.Name);
        ///Show Player List
        ///
        //foreach(Player p in PhotonNetwork.PlayerList)
        //{
        //    playersInRoomDetails.Add(new PlayersInRoomDetails(p.NickName, p.ActorNumber));
        //}
        InstantiateCommunicatorObjectOverNetwork();
        //PhotonView photonView = PhotonView.Get(this);
        //photonView.RPC("RestrictRoomEntry", RpcTarget.All);
    }

    public override void OnLeftRoom()
    {
        //LeaveLobby();
    }

    [ContextMenu("Leave Lobby")]
    public void LeaveLobby()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            Debug.LogError(room.Name);

            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
            {
                if (roomListData.ContainsKey(room.Name))
                {
                    roomListData.Remove(room.Name);
                }

                roomListData.Add(room.Name, room);
            }
            else
            {
                if (roomListData.ContainsKey(room.Name))
                {
                    roomListData[room.Name] = room;
                }
                else
                {
                    roomListData.Add(room.Name, room);
                }
            }
        }

        ///Generate Prefab if wanted Here
    }

    public override void OnLeftLobby()
    {
        roomListData.Clear();
    }
    #endregion

    GameObject communicatorObjectInst;
    public void InstantiateCommunicatorObjectOverNetwork()
    {
        GameManager.Instance.StartGame();
        communicatorObjectInst = PhotonNetwork.Instantiate(pvpCommunicatorObject.name, new Vector3(Random.Range(-8, 8), 0, 0), new Quaternion(0, 0, 0, 0));
        communicatorObjectInst.SetActive(true);
        connectingPanel.SetActive(false);
    }
}