using UnityEngine;
using UnityEngine.AI;
using TMPro;
using FMOD;

public class NPC : MonoBehaviour
{
    [SerializeField] private NPCData            _data;
    [SerializeField] private Player             _player;
    [SerializeField] private Transform[]        _waypoints;
    [SerializeField] private Score              _score;
    
    [SerializeField] private Timer _timer;

    public enum State { Dance, Run, Dead, Hurting };

    private NavMeshAgent    _agent;
    private Animator        _animator;
    private Collider        _collider;
    private State           _state;
    private int             _health;
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
        _state = State.Dance;
        _agent.isStopped = true;
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
        _state = State.Dead;

        _agent.isStopped = true;
        _collider.enabled = false;

        _animator.Play("Falling Forward Death");


        //Cant destroy because of load
        //Destroy(gameObject, 10f);
		FMODUnity.RuntimeManager.PlayOneShot("event:/Ded", transform.position);

        AddTime(-5f);
        AddScore(_data.scorePoints);
    }

    void Update()
    {
        switch (_state)
        {
            case State.Run:
                UpdateRun();
                break;
            case State.Hurting:
                UpdateHurt();
                break;
        }
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

    private void AddScore(int addScore)
    {
        _score.ChangeScore(addScore);
    }
    
    private void AddTime(float addTime)
    {
        _timer.changeTimer(addTime);
    }
    
    [System.Serializable]
    public struct SaveData
    {
        public Vector3      position;
        public Quaternion   rotation;
        public State        state;
        public int          health;
        public int          nextWaypoint;
        public float        curAttackCooldown;
        public float        curHurtCooldown;
        public bool         agentIsStopped;
        public Vector3      agentDestination;
        public Vector3      agentVelocity;
        public int          animationState;
        public float        animationTime;
        public bool         collider;
        //public bool         enabled;
    }

    public SaveData GetSaveData()
    {
        SaveData saveData;

        saveData.position           = transform.position;
        saveData.rotation           = transform.rotation;
        saveData.state              = _state;
        saveData.health             = _health;
        saveData.nextWaypoint       = _nextWaypoint;
        saveData.curAttackCooldown  = _curAttackCooldown;
        saveData.curHurtCooldown    = _curHurtCooldown;
        saveData.agentIsStopped     = _agent.isStopped;
        saveData.agentDestination   = _agent.destination;
        saveData.agentVelocity      = _agent.velocity;
        saveData.animationState     = _animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
        saveData.animationTime      = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        saveData.collider           = GetComponent<Collider>().enabled;
        //saveData.enabled            = gameObject.active;
        
        return saveData;
    }

    public void LoadSaveData(SaveData saveData)
    {
        transform.position  = saveData.position;
        transform.rotation  = saveData.rotation;
        _state              = saveData.state;
        _health             = saveData.health;
        _nextWaypoint       = saveData.nextWaypoint;
        _curAttackCooldown  = saveData.curAttackCooldown;
        _curHurtCooldown    = saveData.curHurtCooldown;
        _agent.isStopped    = saveData.agentIsStopped;
        _agent.destination  = saveData.agentDestination;
        _agent.velocity     = saveData.agentVelocity;
        _collider.enabled   = saveData.collider;
        //gameObject.SetActive(saveData.enabled);

        _animator.Play(saveData.animationState, 0, saveData.animationTime);
    }

}
