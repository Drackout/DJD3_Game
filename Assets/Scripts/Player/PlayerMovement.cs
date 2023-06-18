using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _model;
    [SerializeField] private Transform _body;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private UIManager _uiManager;
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
    [SerializeField] private float _cameraRange;
    [SerializeField] private float  _sprintVelocityFactor;
    [SerializeField] private int    _maxStamina;
    [SerializeField] private int    _staminaRegenRate;
    [SerializeField] private int    _jumpStaminaCost;
    [SerializeField] private int    _sprintStaminaRate;

    private CharacterController _controller;
    private float               _stamina;
    private Vector3             _acceleration;
    private Vector3             _velocity;
    private bool                _isGrounded;
    private bool                _startJump;
    private float               _sinPI4;
    private float               _fallValue;
    private float               _timerSlowFallCurrent;
    private Vector3             _pointTarget;
    private float               _rotationValue;
    private Vector3             _lastRotation;
    private bool                _sprint;
    private Animator            _animator;

    void Start()
    {
        _controller             = GetComponent<CharacterController>();
        _animator               = _body.GetComponent<Animator>();
        _stamina                = _maxStamina;
        _acceleration           = Vector3.zero;
        _velocity               = Vector3.zero;
        _isGrounded             = false;
        _startJump              = false;
        _sinPI4                 = Mathf.Sin(Mathf.PI / 4);
        _fallValue              = _maxFallVelocity;
        _timerSlowFallCurrent   = _timerSlowFall;
        _sprint                 = false;

        HideCursor();
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void AddStamina(float amount)
    {
        _stamina = Mathf.Min(_stamina + amount, _maxStamina);

        UpdateUI();
    }

    private void DecStamina(float amount)
    {
        _stamina = Mathf.Max(_stamina - amount, 0f);

        UpdateUI();
    }

    private void UpdateUI()
    {
        _uiManager.SetStaminaFill(_stamina / _maxStamina);
    }

    void Update()
    {
        UpdateStamina();
        UpdateRotation();
        CheckForJump();
        CheckForSprint();
    }

    private void UpdateStamina()
    {
        if (_stamina == _maxStamina || !_isGrounded)
            return;

        if (!_sprint || (Input.GetAxis("Forward") == 0f && Input.GetAxis("Strafe") == 0f))
            AddStamina(_staminaRegenRate * Time.deltaTime);
    }

    private void UpdateRotation()
    {
        if (Input.GetMouseButton(1))
        {
        float rotation = Input.GetAxis("Mouse X") * _rotationVelocityFactor;
        transform.Rotate(0f, rotation, 0f);
        }
    }

    private void CheckForJump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded && _stamina >= _jumpStaminaCost)
        {
            _animator.SetBool("WalkForward", false);
            _animator.SetTrigger("Jump");
            DecStamina(_jumpStaminaCost);
            _timerSlowFallCurrent   = _timerSlowFall;
            _startJump = true;
        }
        else 
            SlowFalling();
    }

    private void CheckForSprint()
    {
        _sprint = Input.GetButton("Sprint") && _isGrounded && _stamina > 0f;
    }

    // "Bullet jump" to self only, slows falling for X seconds
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
        if (_acceleration.z == 0 && _acceleration.x == 0f)
        {
            _animator.SetBool("WalkForward", false);
        }
    }

    private void UpdateForwardAcceleration()
    {
        float forwardAxis = Input.GetAxis("Forward");

        if (forwardAxis > 0f)
        {
            _animator.SetBool("WalkForward", true);
            CheckCrosshair(1);
            RotateModel(0f);
            _acceleration.z = _forwardAcceleration;
        }
        else if (forwardAxis < 0f)
        {
            _animator.SetBool("WalkForward", true);
            CheckCrosshair(2);
            RotateModel(180f);
            _acceleration.z = _backwardAcceleration;
        }
        else
        {
            _acceleration.z = 0f;
        }

    }

    private void UpdateStrafeAcceleration()
    {
        float strafeAxis = Input.GetAxis("Strafe");

        if (strafeAxis > 0f)
        {
            _animator.SetBool("WalkForward", true);
            CheckCrosshair(3);
            RotateModel(90f);
            _acceleration.x = _strafeAcceleration;
        }
        else if (strafeAxis < 0f)
        {           
            _animator.SetBool("WalkForward", true);
            CheckCrosshair(4);
            RotateModel(270f);
            _acceleration.x = -_strafeAcceleration;
        }
        else
        {
            _acceleration.x = 0f;
        }
    }

    // The player faces the crosshair and keeps its position
    private void CheckCrosshair(int side)
    {

            // get the max distance
            _pointTarget = _cameraTransform.position + (_cameraRange - _cameraTransform.localPosition.z) * _cameraTransform.forward;

            // always point to the max distance
            transform.LookAt(_pointTarget);

        // Small fix for front and back.. not working that great, would require a better way to create all this
        if (side == 1)
        {
            _model.transform.localEulerAngles = new Vector3(-transform.transform.localEulerAngles.x ,_model.rotation.y, _model.rotation.z);
            _body.transform.localEulerAngles = new Vector3(_model.transform.localEulerAngles.x, 0f, 0f);
        }
        else if (side == 2)
        {
            _model.transform.localEulerAngles = new Vector3(-transform.transform.localEulerAngles.x ,_model.rotation.y, _model.rotation.z);
            _body.transform.localEulerAngles = new Vector3(-_model.transform.localEulerAngles.x, 0f, 0f);
        }
        else
        {
            _pointTarget = _cameraTransform.position + (_cameraRange - _cameraTransform.localPosition.z) * _cameraTransform.forward;
            _body.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        }
        //


            _model.transform.localEulerAngles = _lastRotation;
    }

    // The model rotates X angles of the current player direction
    private void RotateModel(float angle)
    {
        float rotationAngle = 0f;

        if (!Input.GetMouseButton(1))
        {
                rotationAngle = angle;    
        }
        else 
            rotationAngle = 0f;
            
        _model.transform.localEulerAngles = new Vector3(0f, rotationAngle, 0f);
        //_lastRotation = _model.transform.localEulerAngles;
    }

    private void UpdateVerticalAcceleration()
    {
        if (_startJump)
        {
            _acceleration.y = _jumpAcceleration;
        }
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

        if (_sprint && (_velocity.z != 0f || _velocity.x != 0f))
        {
            _velocity.z *= _sprintVelocityFactor;
            _velocity.x *= _sprintVelocityFactor;
            DecStamina(_sprintStaminaRate * Time.fixedDeltaTime);
        }

        if (_isGrounded && !_startJump)
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
        
        _isGrounded = _controller.isGrounded;
    }

    [System.Serializable]
    public struct SaveData
    {
        public Vector3      position;
        public Quaternion   rotation;
        public Vector3      velocity;
        public float        stamina;
    }

    public SaveData GetSaveData()
    {
        SaveData saveData;

        saveData.position   = transform.position;
        saveData.rotation   = transform.rotation;
        saveData.velocity   = _velocity;
        saveData.stamina    = _stamina;

        return saveData;
    }

    public void LoadSaveData(SaveData saveData)
    {
        _controller.enabled = false;

        transform.position  = saveData.position;
        transform.rotation  = saveData.rotation;
        _velocity           = saveData.velocity;
        _stamina            = saveData.stamina;
        _isGrounded         = false;
        _startJump          = false;

        _controller.enabled = true;

        UpdateUI();
    }

}
