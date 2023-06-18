using UnityEngine;
using UnityEngine.AI;
using TMPro;

////// CREATE ANOTHER SCRIPT FOR THE GUN BASED ON THIS ONE pew pew

public class NPC : MonoBehaviour
{
    [SerializeField] private NPCData            _data;
    [SerializeField] private Player             _player;
    [SerializeField] private Transform[]        _waypoints;
    [SerializeField] private Score              _score;
    
    // Using timer for now (change to drop pickup later)
    [SerializeField] private Timer _timer;

    private enum State { Dance, Run, Dead, Hurting };

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
        _animator.SetBool("Run", false);

        StartDance();
    }

    private void StartDance()
    {
        //_animator.SetTrigger("Idle");
        //_animator.SetBool("Dance", false);

        _state = State.Dance;

        _agent.isStopped = true;

        _remainingIdleTime = Random.Range(0f, _data.maxIdleTime);
    }

    private void StartRun()
    {
        //_animator.SetTrigger("Walk");
        _animator.Play("mixamo_com");
        _state = State.Run;

        _agent.SetDestination(_waypoints[Random.Range(0, _waypoints.Length)].position);
        _agent.isStopped = false;
    }

    private void StartHurting()
    {
        _state = State.Hurting;

        _agent.isStopped = true;
        _curHurtCooldown = _data.hurtCooldown;
        
    }

    private void StartCrouchIdling()
    {
        _agent.isStopped = true;
        _animator.SetBool("Idle", true);
    }


    private void Die()
    {
        //_animator.SetBool("Walk", false);
        _state = State.Dead;

        _agent.isStopped = true;
        _collider.enabled = false;

        //_animator.SetTrigger("Dead");
        _animator.Play("Falling Forward Death");
        Destroy(gameObject, 10f);
        // Lift a bit for the animation to work correctly

        // Score stuff
        AddTime(-5f);
        AddScore(_data.scorePoints);
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
            case State.Hurting:
                UpdateHurt();
                break;
        }
    }

    private void UpdateDance()
    {
        // if (IsPlayerOnSight())
        // {
        //     
        //     // StartChasing();
        // }
        // else
        // {
        //     _remainingIdleTime -= Time.deltaTime;

        //     // Makes the GUARD go back after chase
        //     if (_remainingIdleTime <= 0f)
        //     {
        //         _nextWaypoint = (_nextWaypoint + 1) % _waypoints.Length;
        //         StartRun();
        //     }
        // }
    }

    private void UpdateRun()
    {
         if (_agent.remainingDistance == 0f)
         {
            StartCrouchIdling();
         }
    }

    private void UpdateHurt()
    {
        _curHurtCooldown -= Time.deltaTime;

        if (_curHurtCooldown <= 0f)
            StartRun();
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
        if (_state != State.Hurting)
        {
            _health = Mathf.Max(_health - amount, 0);

            if (_health > 0)
                StartRun();
            else
            {
                if (_state != State.Dead)
                    Die();
            }
        }
    }

    // Insta death 1 shot
    // public void Damage(int amount)
    // {
    //     
    //     if (_state != State.Dead)
    //         Die();
    // }


    private void AddScore(int addScore)
    {
        _score.ChangeScore(addScore);
    }
    
    private void AddTime(float addTime)
    {
        _timer.changeTimer(addTime);
    }
    


}
