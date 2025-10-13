using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonMultiplayerSetUp : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject _connectingPanel, _roomPanel, _roomLobbyPanel, _playerInRoomTextPrefab;
    [SerializeField] TMP_InputField _roomIdInputF, _playerNameInputF;
    [SerializeField] Button _createBtn, _joinBtn, _gameStartBtn;
    [SerializeField] TMP_Text _roomCreaterName;
    [SerializeField] Transform _playerNameSpawnPostion;
    //bool _connected;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
        _createBtn.onClick.AddListener(CreateRoom);
        _joinBtn.onClick.AddListener(JoinRoom);
        _gameStartBtn.onClick.AddListener(GameStart);
    }

    void Update()
    {
        if (PhotonNetwork.IsConnectedAndReady && !PhotonNetwork.InLobby)
        {
            _connectingPanel.SetActive(false);
            if (!PhotonNetwork.InRoom)
            {
                _roomPanel.SetActive(true);
            }
            PhotonNetwork.JoinLobby();
        }
    }

    void JoinRoom()
    {
        if (_roomIdInputF.text.Length > 2)
        {
            PhotonNetwork.JoinRoom(_roomIdInputF.text);
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LocalPlayer.NickName = _playerNameInputF.text;
        Debug.Log(PhotonNetwork.CurrentRoom);
        _roomPanel.SetActive(false);
        _roomLobbyPanel.SetActive(true);
        _roomCreaterName.text = PhotonNetwork.CurrentRoom.Name + " crated by " + PhotonNetwork.MasterClient.NickName;
        PlayerListRefresh();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log(message);
    }

    void CreateRoom()
    {
        if (_roomIdInputF.text.Length > 2)
        {
            PhotonNetwork.CreateRoom(_roomIdInputF.text);
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        PhotonNetwork.LocalPlayer.NickName = _playerNameInputF.text;
        Debug.Log(PhotonNetwork.CurrentRoom);
        _roomPanel.SetActive(false);
        _roomLobbyPanel.SetActive(true);
        _roomCreaterName.text = PhotonNetwork.CurrentRoom.Name + " crated by " + PhotonNetwork.MasterClient.NickName;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log(message);
    }

    void GameStart()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        PlayerListRefresh();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        PlayerListRefresh();
    }
    
    void PlayerListRefresh()
    {
        foreach (GameObject name in GameObject.FindGameObjectsWithTag("Name"))
        {
            Destroy(name);
        }
        Dictionary<int, Player> _players = PhotonNetwork.CurrentRoom.Players;
        foreach (Player player in _players.Values)
        {
            GameObject _playerNameSpawn = Instantiate(_playerInRoomTextPrefab, _playerNameSpawnPostion);
            _playerNameSpawn.name = player.UserId;
            _playerNameSpawn.GetComponent<TMP_Text>().text = player.NickName;
        }
    }

    //public override void OnPlayerEnteredRoom(Player newPlayer)
    //{
    //    base.OnPlayerEnteredRoom(newPlayer);
    //    GameObject[] nameobj = GameObject.FindGameObjectsWithTag("Name");
    //    foreach (GameObject game in nameobj)
    //    {
    //        Destroy(game);
    //    }
    //    Dictionary<int, Player> i = PhotonNetwork.CurrentRoom.Players;
    //    foreach (Player player in i.Values)
    //    {
    //        GameObject text = Instantiate(_playerNamePrefab, _nameDisplayPosition);
    //        text.name = player.UserId;
    //        text.GetComponent<TMP_Text>().text = player.NickName;
    //    }
    //    _roomCreaterName.text = "Room: '" + PhotonNetwork.CurrentRoom.Name + "' Created by: '" + PhotonNetwork.MasterClient.NickName + "'";
    //}
    //public override void OnPlayerLeftRoom(Player otherPlayer)
    //{
    //    base.OnPlayerLeftRoom(otherPlayer);
    //    GameObject[] nameobj = GameObject.FindGameObjectsWithTag("Name");
    //    foreach (GameObject game in nameobj)
    //    {
    //        Destroy(game);
    //    }
    //    Dictionary<int, Player> i = PhotonNetwork.CurrentRoom.Players;
    //    foreach (Player player in i.Values)
    //    {
    //        GameObject text = Instantiate(_playerNamePrefab, _nameDisplayPosition);
    //        text.name = player.UserId;
    //        text.GetComponent<TMP_Text>().text = player.NickName;
    //    }
    //}

    
}
