using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonMultiplayerSetUp : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject _connectingPanel, _roomPanel, _roomLobbyPanel;
    [SerializeField] TMP_InputField _roomIdInputF, _playerNameInputF;
    [SerializeField] Button _createBtn, _joinBtn, _gameStartBtn;
    [SerializeField] TMP_Text _roomCreaterName;
    bool _connected;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
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
        PhotonNetwork.LoadLevel(1);
    }
}
