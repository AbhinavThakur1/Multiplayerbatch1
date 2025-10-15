using UnityEngine;
using Photon.Pun;

public class UiManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject _player;

    private void Start()
    {
        PhotonNetwork.Instantiate(_player.name, _player.transform.position, Quaternion.identity);
    }

}