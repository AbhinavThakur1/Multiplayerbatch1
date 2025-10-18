using UnityEngine;
using Photon.Pun;

public class UiManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject _player, _enemy;

    private void Start()
    {
        PhotonNetwork.Instantiate(_player.name, _player.transform.position, Quaternion.identity);
        if(PhotonNetwork.LocalPlayer == PhotonNetwork.MasterClient)
        {
            PhotonNetwork.InstantiateRoomObject(_enemy.name, _enemy.transform.position, Quaternion.identity);
        }
    }

}