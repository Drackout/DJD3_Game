using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemyRanged : MonoBehaviour
{
    [SerializeField] private EnemyData          _data;
    [SerializeField] private Player             _player;
    [SerializeField] private EnemyType          _type;
    [SerializeField] private Transform[]        _waypoints;
    [SerializeField] private Score              _score;
    [SerializeField] private IntGlobalValue     _currentEnemies;
    [SerializeField] private GameObject         _bullet;
    [SerializeField] private Transform          _bulletSpawn;
    [SerializeField] private float              _bulletSpeed;
    
    
    // Using timer for now (change to drop pickup later)
    [SerializeField] private Timer _timer;

    private enum State { Idling, Patrolling, Chasing, Attacking, Hurting, Dead };
    private enum EnemyType {Patroller, Guard, Chaser};

    private NavMeshAgent    _agent;
    private Animator        _animator;
    private Collider        _collider;
    private State           _state;
    private int             _health;
    private float           _remainingIdleTime;
    private int             _nextWaypoint;
    private float           _curAttackCooldown;
    private float           _curHurtCooldown;

    void Start()
    {
        _agent              = GetComponent<NavMeshAgent>();
        _animator           = GetComponent<Animator>();
        _collider           = GetComponent<Collider>();
        _health             = _data.maxHealth;
        _nextWaypoint       = 0;

        if(_type != EnemyType.Chaser)
            StartIdling();
        else
            StartChasing();
    }

    private void StartIdling()
    {
        //_animator.SetTrigger("Idle");
        _animator.SetBool("Walk", false);

        _state = State.Idling;

        _agent.isStopped = true;

        _remainingIdleTime = Random.Range(0f, _data.maxIdleTime);
    }

    private void StartPatrolling()
    {
        //_animator.SetTrigger("Walk");
        _animator.SetBool("Walk", true);
        _state = State.Patrolling;

        _agent.SetDestination(_waypoints[Random.Range(0, _waypoints.Length)].position);
        _agent.isStopped = false;
    }

    private void StartChasing()
    {        
        _animator.SetBool("Walk", true);
        _state = State.Chasing;

        _agent.SetDestination(_player.transform.position);
        _agent.isStopped = false;
    }

    private void StartAttacking()
    {
        _animator.SetBool("Walk", false);

        _state = State.Attacking;

        _agent.isStopped = true;

        Attack();
    }

    private void StartHurting()
    {
        _animator.SetBool("Walk", false);

        _state = State.Hurting;

        _agent.isStopped = true;

        _animator.SetTrigger("Hurt");

        _curHurtCooldown = _data.hurtCooldown;
        
    }

    private void Die()
    {
        _animator.SetBool("Walk", false);
        _state = State.Dead;

        _agent.isStopped = true;
        _collider.enabled = false;

        _animator.SetTrigger("Die");
        Destroy(gameObject, 10f);

        // Score stuff
        AddTime(3f);
        AddScore(_data.scorePoints);
        _currentEnemies.ChangeValue(-1);
    }

    void Update()
    {
        switch (_state)
        {
            case State.Idling:
                UpdateIdle();
                break;
            case State.Patrolling:
                UpdatePatrol();
                break;
            case State.Chasing:
                UpdateChase();
                break;
            case State.Attacking:
                UpdateAttack();
                break;
            case State.Hurting:
                UpdateHurt();
                break;
        }
    }

    private void UpdateIdle()
    {
        if (IsPlayerOnSight())
        {
            
            StartChasing();
        }
        else
        {
            _remainingIdleTime -= Time.deltaTime;

            // Makes the GUARD go back after chase
            if (_remainingIdleTime <= 0f)
            {
                _nextWaypoint = (_nextWaypoint + 1) % _waypoints.Length;
                StartPatrolling();
            }
        }
    }

    private void UpdatePatrol()
    {
        if (IsPlayerOnSight())
            StartChasing();
        else if (_agent.remainingDistance == 0f)
        {
            StartIdling();
        }
    }

    private void UpdateChase()
    {
        
        if (IsPlayerOnSight())
        {
            if (IsPlayerOnAttackRange())
                StartAttacking();
            else
                _agent.SetDestination(_player.transform.position);
        }
        else if (_type == EnemyType.Chaser)
            StartChasing();
        else if(_agent.remainingDistance == 0f)
            StartIdling();
    }

    private void UpdateAttack()
    {
        _curAttackCooldown -= Time.deltaTime;

        if (_curAttackCooldown <= 0f)
        {
            if (IsPlayerOnSight() || _type == EnemyType.Chaser)
            {
                if (IsPlayerOnAttackRange()) 
                    Attack();
                else
                    StartChasing();
            }
            else
                StartIdling();
        }
    }

    private void UpdateHurt()
    {
        _curHurtCooldown -= Time.deltaTime;

        if (_curHurtCooldown <= 0f)
            StartChasing();
    }

    private bool IsPlayerOnSight()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) > _data.sightingRange)
            return false;

        if (Vector3.Angle(_player.transform.position - transform.position, transform.forward) > _data.sightingAngle)
            return false;

        if (Physics.Linecast(transform.position, _player.transform.position, out RaycastHit hitInfo) && hitInfo.collider.transform != _player.transform)
            return false;

        return true;
    }

    private bool IsPlayerOnAttackRange()
    {
        return Vector3.Distance(_player.transform.position, transform.position) <= _data.attackRange;
    }

    private void Attack()
    {
        
            Vector3 playerDirection = _player.transform.position - _bulletSpawn.position;
            
            GameObject currentBullet = Instantiate(_bullet, _bulletSpawn.position, Quaternion.identity);
            currentBullet.transform.forward = playerDirection.normalized;
            currentBullet.GetComponent<Rigidbody>().AddForce(playerDirection.normalized * _bulletSpeed, ForceMode.Impulse);

            _animator.SetTrigger("Shoot");

            _curAttackCooldown = _data.attackCooldown;
    }

    public void Damage(int amount)
    {
        if (_state != State.Hurting)
        {
            _health = Mathf.Max(_health - amount, 0);

            if (_health > 0)
                StartHurting();
            else
            {
                if (_state != State.Dead)
                    Die();
            }
        }
    }

    private void AddScore(int addScore)
    {
        _score.ChangeScore(addScore);
    }
    
    private void AddTime(float addTime)
    {
        _timer.changeTimer(addTime);
    }
    


}
