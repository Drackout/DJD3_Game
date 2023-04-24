using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform _playerModel;
    [SerializeField] private Transform _occlusionPivot;
    [SerializeField] private float _rotationVelocityFactor;
    [SerializeField] private float _maxPitchUpAngle;
    [SerializeField] private float _minPitchDownAngle;
    [SerializeField] private float _resetYawSpeed;
    [SerializeField] private float _zoomAccelerationFactor;
    [SerializeField] private float _zoomDeceleration;
    [SerializeField] private float _minZoomDistance;
    [SerializeField] private float _maxZoomDistance;
    [SerializeField] private float _deocclusionBuffer;
    [SerializeField] private float _deocclusionVelocity;
    [SerializeField] private float _shootRange;

    private Transform _cameraTransform;
    private float _zoomAcceleration;
    private float _zoomVelocity;
    private float _zoomPosition;
    private Vector3 _deocclusionVector;
    private Vector3 _pointTarget;
    private Vector3 _cameraBeforeAim;

    void Start()
    {
        _cameraTransform = GetComponentInChildren<Camera>().transform;
        _zoomVelocity = 0f;
        _deocclusionVector = new Vector3(0, 0, _deocclusionBuffer);
        _zoomPosition = _cameraTransform.localPosition.z;
    }

    void Update()
    {
        UpdatePitch();
        UpdateYaw();
        UpdateZoom();

        PreventOcclusion();
    }



    private void UpdatePitch()
    {
        Vector3 rotation = transform.localEulerAngles;
        rotation.x -= Input.GetAxis("Mouse Y") * _rotationVelocityFactor;

        if (rotation.x < 180f)
            rotation.x = Mathf.Min(rotation.x, _maxPitchUpAngle);
        else
            rotation.x = Mathf.Max(rotation.x, _minPitchDownAngle);

        transform.localEulerAngles = rotation;
    }


    private void UpdateYaw()
    {
        _cameraBeforeAim = transform.localEulerAngles;

        if (Input.GetMouseButton(1)) //1st was resetYaw
        {
            ////////////////////////////////////// not yet able to change the camera correctly
            //ResetYaw();
            RotateToCrosshair();
        }
        else
        {
            Vector3 rotation = transform.localEulerAngles;
            rotation.y += Input.GetAxis("Mouse X") * _rotationVelocityFactor;
            transform.localEulerAngles = rotation;
        }
    }

    
    // Rotate player model to crosshair
    private void RotateToCrosshair()
    {
       if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hitInfo, _shootRange))
           _pointTarget = hitInfo.point;
       else
           _pointTarget = _cameraTransform.position + (_shootRange - _cameraTransform.localPosition.z) * _cameraTransform.forward;

       _playerModel.transform.LookAt(_pointTarget);
    }


    private void ResetYaw()
    {
        Vector3 rotation = transform.localEulerAngles;

        if (rotation.y < 180f && rotation.y != 0f)
            rotation.y = Mathf.Max(rotation.y - Time.deltaTime * _resetYawSpeed, 0f);
        else
            rotation.y = Mathf.Min(rotation.y + Time.deltaTime * _resetYawSpeed, 360f);

        transform.localEulerAngles = rotation;

    }


    private void UpdateZoom()
    {
        UpdateZoomAcceleration();
        UpdateZoomVelocity();
        UpdateZoomPosition();


    }


    private void UpdateZoomAcceleration()
    {
        _zoomAcceleration = Input.GetAxis("Mouse ScrollWheel") * _zoomAccelerationFactor;
    }

    private void UpdateZoomVelocity()
    {
        if (_zoomAcceleration != 0f)
            _zoomVelocity += _zoomAcceleration * Time.deltaTime;
        else if (_zoomVelocity > 0f)
        {
            _zoomVelocity -= _zoomDeceleration * Time.deltaTime;
            _zoomVelocity = Mathf.Max(_zoomVelocity, 0f);
        }
        else if (_zoomVelocity < 0f)
        {
            _zoomVelocity += _zoomDeceleration * Time.deltaTime;
            _zoomVelocity = Mathf.Min(_zoomVelocity, 0f);
        }
    }

    private void UpdateZoomPosition()
    {
        if (_zoomVelocity != 0f)
        {
            Vector3 position = _cameraTransform.localPosition;

            position.z += _zoomVelocity * Time.deltaTime;

            if (position.z < -_maxZoomDistance) 
            {
                position.z = -_maxZoomDistance;
                _zoomVelocity = 0f;
            }
            else if (position.z > -_minZoomDistance)
            {
                position.z = -_minZoomDistance;
                _zoomVelocity = 0f;
            }
            _cameraTransform.localPosition = position;
            _zoomPosition = position.z;
        }
    }

    private void PreventOcclusion()
    {        
        if(Physics.Linecast(_occlusionPivot.position, _cameraTransform.position - _cameraTransform.TransformDirection(_deocclusionVector), out RaycastHit hitInfo))
        {
            if (hitInfo.collider.CompareTag("WorldBoundary"))
            {
                _cameraTransform.position = hitInfo.point + _cameraTransform.TransformDirection(_deocclusionVector);
                _cameraTransform.localPosition = new Vector3(0, 0, _cameraTransform.localPosition.z);
            }
            else
            {
                Vector3 localPosition = _cameraTransform.localPosition;
                localPosition.z += Time.deltaTime * _deocclusionVelocity;
                _cameraTransform.localPosition = localPosition;
            }
        }
        else
        {
            RevertDeocclusion();
        }
    }

    private void RevertDeocclusion()
    {
        Vector3 localPosition = _cameraTransform.localPosition;

        if (localPosition.z > _zoomPosition)
        {
            localPosition.z = Mathf.Max(localPosition.z - Time.deltaTime * _deocclusionVelocity, _zoomPosition);

            Vector3 worldPosition = transform.TransformPoint(localPosition);

            if (!Physics.Linecast(_occlusionPivot.position, worldPosition - _cameraTransform.TransformDirection(_deocclusionVector), out RaycastHit hitInfo))
                _cameraTransform.localPosition = localPosition;
        }
    }
}