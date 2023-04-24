using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform _weapon;
    [SerializeField] private Transform _barrelEnd;
    [SerializeField] private float _shootRange;
    [SerializeField] private float _shootcooldown;
    [SerializeField] private float _shootRendertime;
    [SerializeField] private Camera _Camera;

    private Transform _cameraTransform;
    private float _cameraFOV;
    private LineRenderer _lineRenderer;
    private Vector3 _shootTarget;
    private float _currShootCooldown;
    private float _currShootRenderTime;


    void Start()
    {
        _cameraTransform = GetComponentInChildren<Camera>().transform;
        _Camera.fieldOfView = 60f;
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        _currShootCooldown = _shootcooldown;
        _currShootRenderTime = _shootRendertime;
    }

    void Update()
    {
        UpdateTarget();
        UpdateShoot();
        
        if (!Input.GetMouseButton(1))
            _Camera.fieldOfView = 60f;
        else
            _Camera.fieldOfView = 30f;
    }

    private void UpdateTarget()
    {
        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hitInfo, _shootRange))
            _shootTarget = hitInfo.point;
        else
            _shootTarget = _cameraTransform.position + (_shootRange - _cameraTransform.localPosition.z) * _cameraTransform.forward;

        _weapon.transform.LookAt(_shootTarget);
    }

    private void UpdateShoot()
    {
        _currShootCooldown = Mathf.Min(_currShootCooldown + Time.deltaTime, _shootcooldown);
        _currShootRenderTime = Mathf.Min(_currShootRenderTime + Time.deltaTime, _shootRendertime);

        if (_currShootRenderTime == _shootRendertime)
            _lineRenderer.enabled = false;

        if (_currShootCooldown == _shootcooldown && Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        _currShootCooldown = 0f;
        _currShootRenderTime = 0f;

        _lineRenderer.SetPosition(0, _barrelEnd.position);
        _lineRenderer.SetPosition(1, _shootTarget);

        _lineRenderer.enabled = true;
    }



}
