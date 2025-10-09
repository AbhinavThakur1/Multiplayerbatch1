using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAi : MonoBehaviour
{
    NavMeshAgent _agent;
    [SerializeField] Image _healthBar;
    [SerializeField] GameObject _target;
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
        if (_trigger && Vector3.Distance(transform.position, _target.transform.position) > _agent.stoppingDistance)
        {
            _animator.SetBool("Attack", false);
            _animator.SetBool("Run", true);
            _agent.SetDestination(_target.transform.position);
        }
        else if (Vector3.Distance(transform.position, _target.transform.position) <= _agent.stoppingDistance)
        {
            Attack();
            _agent.velocity = Vector3.zero;
        }
        else if(Vector3.Distance(transform.position, _target.transform.position) <= _triggerRange || _trigger)
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
        }
        _healthBar.fillAmount = _health / _maxhealth;
    }

    public void GotHit()
    {
        _health -= 20;
        _trigger = true;
    }

    public void Attack()
    {
        _animator.SetBool("Attack", true);
        _animator.SetBool("Run", false);
        _target.GetComponent<PlayerMovement>().GotHit();
    }
}
