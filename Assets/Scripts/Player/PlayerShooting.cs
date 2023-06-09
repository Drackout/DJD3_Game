using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using FMOD;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Player         _player;
    [SerializeField] private UIManager      _uiManager;
    [SerializeField] private Transform      _weapon;
    [SerializeField] private Transform      _barrelEnd;
    [SerializeField] private Transform      _cameraTransform;
    [SerializeField] private Transform      _model;
    [SerializeField] private float          _shootRange;
    [SerializeField] private float          _shootCooldown;
    [SerializeField] private float          _shootRendertime;
    [SerializeField] private int            _shootDamage;
    [SerializeField] private float          _lookAtShotTimer;

    [SerializeField] private GameObject[]   _bloonSpawner;
    [SerializeField] private GameObject[]   _shootBloons;

    //private Transform _cameraTransform;
    private float           _cameraFOV;
    private LineRenderer    _lineRenderer;
    private Collider        _shootTargetCollider;
    private Vector3         _shootTargetPoint;
    private float           _currShootCooldown;
    private float           _currShootRenderTime;
    private float           _currLookAtTimer;
    private int             _bloonSelected;
    private Transform       _bloonSpawnerCurrentLocator;
    private ParticleSystem  _particleSystem;


    void Start()
    {
        _lineRenderer           = GetComponentInChildren<LineRenderer>();
        _particleSystem         = _barrelEnd.GetComponent<ParticleSystem>();
        _currShootCooldown      = _shootCooldown;
        _currShootRenderTime    = _shootRendertime;
        _currLookAtTimer        = _lookAtShotTimer;
        _bloonSelected          = 0;


    }

    void Update()
    {
        UpdateTarget();
        UpdateShoot();


        //LookWhenShot(_currLookAtTimer);
        
    }

    private void UpdateTarget()
    {
        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hitInfo, _shootRange))
        {
            _shootTargetPoint       = hitInfo.point;
            _shootTargetCollider    = hitInfo.collider;
        }
        else
        {
            _shootTargetPoint       = _cameraTransform.position + (_shootRange - _cameraTransform.localPosition.z) * _cameraTransform.forward;
            _shootTargetCollider    = null;
        }

        //_weapon.transform.LookAt(_shootTargetPoint);
        
    }

    private void UpdateShoot()
    {
        _currShootCooldown = Mathf.Min(_currShootCooldown + Time.deltaTime, _shootCooldown);
        _currShootRenderTime = Mathf.Min(_currShootRenderTime + Time.deltaTime, _shootRendertime);

        if (_currShootRenderTime == _shootRendertime)
            _lineRenderer.enabled = false;

        _currShootCooldown = Mathf.Min(_currShootCooldown + Time.deltaTime, _shootCooldown);
        UpdateUI();
        

        if (_currShootCooldown == _shootCooldown && Input.GetButtonDown("Shoot"))
        { 
            Shoot();
            DamageTarget();
        }

        if (_currShootCooldown == _shootCooldown)
            _uiManager.HideWeaponInfo();
        else
            _uiManager.ShowWeaponInfo();
    }

    private void UpdateUI()
    {
        _uiManager.SetWeaponFill(_currShootCooldown / _shootCooldown);
    }

    private void Shoot()
    {
        _particleSystem.Emit(5);
        int randomPosition;
        _currShootCooldown = 0f;
        _currShootRenderTime = 0f;

        _lineRenderer.SetPosition(0, _barrelEnd.position);
        _lineRenderer.SetPosition(1, _shootTargetPoint);

        _lineRenderer.enabled = true;


		FMODUnity.RuntimeManager.PlayOneShot("event:/Shoot", transform.position);
        //_bloonSpawnerCurrentLocator = _bloonSpawner.transform;


        _bloonSelected = (_bloonSelected + 1) % _shootBloons.Length;
        randomPosition = Random.Range(0, _bloonSpawner.Length);

        //transform.LookAt(_player.transform);

        Instantiate(_shootBloons[_bloonSelected], _bloonSpawner[randomPosition].transform.position, Quaternion.identity);


        // bloonSelected = Random.Range(0, _shootBloons.Length);

        //        _bloonSelected = (_bloonSelected + 1) % _shootBloons.Length;

        //        _shootBloons[_bloonSelected].SetActive(true);


        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hitInfo, _shootRange))
            _shootTargetPoint = hitInfo.point;
        else
            _shootTargetPoint = _cameraTransform.position + (_shootRange - _cameraTransform.localPosition.z) * _cameraTransform.forward;

            _model.transform.LookAt(_shootTargetPoint);

        //_currLookAtTimer = 0;
    }

    //  Look where's being shot for X Seconds - NOT USED YET (Review)
    private void LookWhenShot(float remainTime)
    {
        //  if (remainTime <= _lookAtShotTimer && !Input.GetMouseButtonDown(0))
        //  {
        //  }
            // Look where's being shot
        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hitInfo, _shootRange))
            _shootTargetPoint = hitInfo.point;
        else
            _shootTargetPoint = _cameraTransform.position + (_shootRange - _cameraTransform.localPosition.z) * _cameraTransform.forward;

            _model.transform.LookAt(_shootTargetPoint);
            _currLookAtTimer += Time.deltaTime;
    }

    private void DamageTarget()
    {
        if (_shootTargetCollider != null)
        {
            Enemy enemy = _shootTargetCollider.GetComponentInParent<Enemy>();
            EnemyRanged enemyRanged = _shootTargetCollider.GetComponentInParent<EnemyRanged>();
            NPC npc     = _shootTargetCollider.GetComponentInParent<NPC>();

            if (enemy != null)
                enemy.Damage(_shootDamage);

            if (enemyRanged != null)
                enemyRanged.Damage(_shootDamage);
            
            if (npc != null)
                npc.Damage(_shootDamage);
        }
    }

    [System.Serializable]
    public struct SaveData
    {
        public float curShootCooldown;
    }

    public SaveData GetSaveData()
    {
        SaveData saveData;

        saveData.curShootCooldown = _currShootCooldown;

        return saveData;
    }

    public void LoadSaveData(SaveData saveData)
    {
        _currShootCooldown = saveData.curShootCooldown;

        UpdateUI();
    }

}
