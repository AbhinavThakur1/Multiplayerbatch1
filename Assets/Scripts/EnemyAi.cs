using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Photon.Pun;

public class EnemyAi : MonoBehaviourPunCallbacks
{
    NavMeshAgent _agent;
    [SerializeField] Image _healthBar;
    Transform _target;
    float _triggerRange = 8f;
    public float _health, _maxhealth = 100f;
    bool _trigger = false;
    Animator _animator;

    void Start()
    {
        _health = _maxhealth;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_target != null)
        {
            faceTarget(_target.position);
            if (_trigger && Vector3.Distance(transform.position, _target.position) > _agent.stoppingDistance)
            {
                _animator.SetBool("Attack", false);
                _animator.SetBool("Run", true);
                _agent.SetDestination(_target.position);
            }
            else if (Vector3.Distance(transform.position, _target.position) <= _agent.stoppingDistance)
            {
                _animator.SetBool("Attack", true);
                _animator.SetBool("Run", false);
                _agent.velocity = Vector3.zero;
            }
            else if (Vector3.Distance(transform.position, _target.position) <= _triggerRange || _trigger)
            {
                _animator.SetBool("Attack", false);
                _animator.SetBool("Run", true);
                _agent.SetDestination(_target.transform.position);
            }
            else
            {
                _agent.velocity = Vector3.zero;
                _animator.SetBool("Attack", false);
                _animator.SetBool("Run", false);
                _target = null;
            }
        }
        else
        {
            foreach(PlayerMovement _player in FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None))
            {
                if(Vector3.Distance(transform.position, _player.gameObject.transform.position) <= _triggerRange)
                {
                    _target = _player.gameObject.transform;
                }
            }
        }
         _healthBar.fillAmount = _health / _maxhealth;
    }

    public void GotHit(GameObject _player)
    {
        //_target = _player.transform;
        photonView.RPC("TargetUpdate", RpcTarget.AllBuffered, _player.GetPhotonView().ViewID);
        Debug.Log(_player.name);
        _health -= 20f;
        photonView.RPC("UpdateHealth", RpcTarget.AllBuffered, _health);
        _trigger = true;
    }

    [PunRPC]
    void TargetUpdate(int viewId)
    {
        PhotonView _pv = PhotonView.Find(viewId);
        _target = _pv.gameObject.transform;
    }

    public void Attack()
    {
        _target.GetComponent<PlayerMovement>().GotHit();
    }

    [PunRPC]
    void UpdateHealth(float newHealth)
    {
        //health update
        _health = newHealth;
    }

    void faceTarget(Vector3 _lookAt)
    {
        //faces target
        Vector3 dir = (_lookAt - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, transform.position.y, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
    }
}
