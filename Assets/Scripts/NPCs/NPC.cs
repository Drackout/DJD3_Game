using UnityEngine;
using UnityEngine.AI;
using TMPro;

////// CREATE ANOTHER SCRIPT FOR THE GUN BASED ON THIS ONE pew pew

public class NPC : MonoBehaviour
{
    [SerializeField] private EnemyData          _data;
    [SerializeField] private Player             _player;
    [SerializeField] private Transform[]        _waypoints;
    [SerializeField] private Score              _score;
    [SerializeField] private IntGlobalValue     _currentEnemies;
    
    // Using timer for now (change to drop pickup later)
    [SerializeField] private Timer _timer;

    private enum State { Dance, Run, Dead };

    private NavMeshAgent    _agent;
    private Animator        _animator;
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
        _health             = _data.maxHealth;
        _nextWaypoint       = 0;

            StartDance();
    }

    private void StartDance()
    {
        //_animator.SetTrigger("Idle");
        _animator.SetBool("Walk", false);

        _state = State.Dance;

        _agent.isStopped = true;

        _remainingIdleTime = Random.Range(0f, _data.maxIdleTime);
    }

    private void StartRun()
    {
        //_animator.SetTrigger("Walk");
        _animator.SetBool("Walk", true);
        _state = State.Run;

        _agent.SetDestination(_waypoints[Random.Range(0, _waypoints.Length)].position);
        _agent.isStopped = false;
    }


    private void Die()
    {
        _animator.SetBool("Walk", false);
        _state = State.Dead;

        _agent.isStopped = true;

        _animator.SetTrigger("Die");
        // Lift a bit for the animation to work correctly

        // Score stuff
        AddTime(3f);
        AddScore(_data.scorePoints);
        _currentEnemies.ChangeValue(-1);
    }

    void Update()
    {
        switch (_state)
        {
            case State.Dance:
                UpdateDance();
                break;
            case State.Run:
                UpdateRun();
                break;
        }
    }

    private void UpdateDance()
    {
        if (IsPlayerOnSight())
        {
            
            // StartChasing();
        }
        else
        {
            _remainingIdleTime -= Time.deltaTime;

            // Makes the GUARD go back after chase
            if (_remainingIdleTime <= 0f)
            {
                _nextWaypoint = (_nextWaypoint + 1) % _waypoints.Length;
                StartRun();
            }
        }
    }

    private void UpdateRun()
    {
        if (IsPlayerOnSight())
            print("still checking");
            //StartChasing();
        else if (_agent.remainingDistance == 0f)
        {
            StartDance();
        }
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


    public void Damage(int amount)
    {
        
        if (_state != State.Dead)
            Die();
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
