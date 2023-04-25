using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _forwardAcceleration;
    [SerializeField] private float _backwardAcceleration;
    [SerializeField] private float _strafeAcceleration;
    [SerializeField] private float _gravityAcceleration;
    [SerializeField] private float _jumpAcceleration;
    [SerializeField] private float _maxForwardVelocity;
    [SerializeField] private float _maxBackwardVelocity;
    [SerializeField] private float _maxStrafeVelocity;
    [SerializeField] private float _maxFallVelocity;
    [SerializeField] private float _rotationVelocityFactor;
    [SerializeField] private float _timerSlowFall;

    private CharacterController _controller;
    private Vector3 _acceleration;
    private Vector3 _velocity;
    private bool    _startJump;
    private float   _sinPI4;
    private float   _fallValue;
    private float   _timerSlowFallCurrent;
    private Vector3 _pointTarget;

    void Start()
    {
        _controller             = GetComponent<CharacterController>();
        _acceleration           = Vector3.zero;
        _velocity               = Vector3.zero;
        _startJump              = false;
        _sinPI4                 = Mathf.Sin(Mathf.PI / 4);
        _fallValue              = _maxFallVelocity;
        _timerSlowFallCurrent   = _timerSlowFall;

        HideCursor();
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
            UpdateRotation();
        
        CheckForJump();
    }


    private void UpdateRotation()
    {
        float rotation = Input.GetAxis("Mouse X") * _rotationVelocityFactor;

        transform.Rotate(0f, rotation, 0f);
    }

    private void CheckForJump()
    {
        if (Input.GetButtonDown("Jump") && _controller.isGrounded)
        {
            _timerSlowFallCurrent   = _timerSlowFall;
            _startJump = true;
        }
        else 
            SlowFalling();
    }

    // "Bullet jump" to self only, slows falling for X time
    private void SlowFalling(){
        if (Input.GetMouseButton(1) && _timerSlowFallCurrent > 0f)
        {
            _timerSlowFallCurrent -= Time.deltaTime;
            _maxFallVelocity = -1;
        }
        else
            _maxFallVelocity = _fallValue;
    }

    void FixedUpdate()
    {
        UpdateAcceleration();
        UpdateVelocity();
        UpdatePosition();
    }

    private void UpdateAcceleration()
    {
        UpdateForwardAcceleration();
        UpdateStrafeAcceleration();
        UpdateVerticalAcceleration();
    }

    private void UpdateForwardAcceleration()
    {

        //if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hitInfo, _shootRange))
        //    _pointTarget = hitInfo.point;
        //else
        //    _pointTarget = _cameraTransform.position + (_shootRange - _cameraTransform.localPosition.z) * _cameraTransform.forward;
        //    _playerModel.transform.LookAt(_pointTarget);

        float forwardAxis = Input.GetAxis("Forward");

        if (forwardAxis > 0f)
            _acceleration.z = _forwardAcceleration;
        else if (forwardAxis < 0f)
            _acceleration.z = _backwardAcceleration;
        else
            _acceleration.z = 0f;
    }

    private void UpdateStrafeAcceleration()
    {
        float strafeAxis = Input.GetAxis("Strafe");

        if (strafeAxis > 0f)
            _acceleration.x = _strafeAcceleration;
        else if (strafeAxis < 0f)
            _acceleration.x = -_strafeAcceleration;
        else
            _acceleration.x = 0f;
    }

    private void UpdateVerticalAcceleration()
    {
        if (_startJump)
            _acceleration.y = _jumpAcceleration;
        else
            _acceleration.y = _gravityAcceleration;
    }

    private void UpdateVelocity()
    {
        _velocity += _acceleration * Time.fixedDeltaTime;

        if (_acceleration.z == 0f || (_acceleration.z * _velocity.z < 0f))
            _velocity.z = 0f;
        else if (_acceleration.x == 0f)
            _velocity.z = Mathf.Clamp(_velocity.z, _maxBackwardVelocity, _maxForwardVelocity);
        else
            _velocity.z = Mathf.Clamp(_velocity.z, _maxBackwardVelocity * _sinPI4, _maxForwardVelocity * _sinPI4);

        if (_acceleration.x == 0f || (_acceleration.x * _velocity.x < 0f))
            _velocity.x = 0f;
        else if (_acceleration.z == 0f)
            _velocity.x = Mathf.Clamp(_velocity.x, -_maxStrafeVelocity, _maxStrafeVelocity);
        else
            _velocity.x = Mathf.Clamp(_velocity.x, -_maxStrafeVelocity * _sinPI4, _maxStrafeVelocity * _sinPI4);

        if (_controller.isGrounded && !_startJump)
            _velocity.y = -0.1f;
        else
            _velocity.y = Mathf.Max(_velocity.y, _maxFallVelocity);

        _startJump = false;
    }

    private void UpdatePosition()
    {
        Vector3 motion = _velocity * Time.fixedDeltaTime;

        motion = transform.TransformVector(motion);

        _controller.Move(motion);
    }
}
