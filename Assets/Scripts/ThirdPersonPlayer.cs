using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class ThirdPersonPlayer : MonoBehaviour
{
    //Move speed of character
    public float speed = 4.0f;
    public float sprintSpeed = 8f;
    
    //Character turn to face movement direction
    public float rotationSmoothTime = 0.12f;
    
    //Acceleration/deceleration factor
    public float acceleration = 10.0f;
 
    //Jump parameters
    public float jumpHeight = 100f;
    public float jumpTimeout = 0.50f;
    
    //Custom gravity parameters
    public float gravity = -15.0f;
    public float fallTimeout = 0.15f;
    public bool grounded = true;
    public float groundedOffset = -0.14f;
    public float groundedRadius = 1f;
    public LayerMask groundLayers;
    
    //Camera target
    public GameObject cameraRoot;
    
    //Degree clamps for camera rotation
    public float topClamp = 70.0f;
    public float bottomClamp = -30.0f;
    
    //Extra degrees to override camera. Useful when locked
    public float angleCamOverride = 0.0f;
    
    //For locking the camera position on all axis
    public bool lockCameraPosition = false;
    
    //Cinemachine parameters
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    //Player parameters
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;
    
    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;
    
    private Animator _animator;
    private CharacterController _controller;
    private InputChecker _input;
    public GameObject _mainCamera;
    

    private float deltaTimeMultiplier =  0.1f;
    private const float _threshold = 0.01f;

    private float lastRotation;
    private Vector3 lastRotationV;
    private float rotationBlend;
    private float idleTimeout = 5.0f;
    private float _idleTimeoutDelta;

    private GameObject gameManager;

    public bool isSwimming = false;
    
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        _cinemachineTargetYaw = cameraRoot.transform.rotation.eulerAngles.y;
        gameManager = this.GetComponent<CharacterVariables>().gameManager;    
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _input = gameManager.gameObject.GetComponent<InputChecker>();
    
    }
    
    // Update is called once per frame
    private void Update()
    {
        _animator.SetBool("Swimming", isSwimming);
        JumpAndGravity();
        GroundedCheck();
        lockCameraPosition = _input.inMenu;
        CameraRotation();
        Move();
    }
    
    private void GroundedCheck()
    {
        // set sphere position, with offset
        float posy = transform.position.y - groundedOffset;
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
        grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
        
        _animator.SetBool("Grounded", grounded);
    }
    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_input.look.sqrMagnitude >= _threshold && !lockCameraPosition)
        {
            _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier * 2;
            _cinemachineTargetPitch += (-_input.look.y) * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

        // Cinemachine will follow this target
        cameraRoot.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + angleCamOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    private void Move()
    {

        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = _input.sprint ? sprintSpeed : speed;

        // If no input, set the target speed to 0
        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        // a reference to the players current horizontal velocity
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * acceleration);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * acceleration);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        // normalise input direction
        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
        
        float rotation = 0.0f;
        Vector3 targetDirection;
        if (!_input.aim){
            if (_input.move != Vector2.zero)
            {
                if (!isSwimming)
                {
                  _animator.SetBool("inIdle", false);
                  _animator.SetBool("superIdle", false);  
                }
                
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    rotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            }

            else if (!_animator.GetBool("inIdle"))
            {
                _idleTimeoutDelta = idleTimeout;
                _animator.SetBool("inIdle", true);

                int idle = Random.Range(1, 4);
 
                _animator.SetInteger("Idle", idle);
            }

            if (_animator.GetBool("inIdle"))
            {
                if (_idleTimeoutDelta >= 0.0f)
                {
                    _idleTimeoutDelta -= Time.deltaTime;
                }

                if (_idleTimeoutDelta < 0.0f && !_animator.GetBool("superIdle"))
                {
                    _animator.SetBool("superIdle", true);
                }
            }
        }

        targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        if(_input.aim && grounded)
        {
              //Set the player rotation based on the look transform
              transform.rotation = Quaternion.Euler(0, cameraRoot.transform.rotation.eulerAngles.y, 0);
              //reset the y rotation of the look transform
              //followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
              targetDirection = Quaternion.Euler(0.0f, cameraRoot.transform.rotation.eulerAngles.y, 0.0f) * Vector3.forward;
        }
        else
        {
            // get a numeric angle for each vector, on the X-Z plane (relative to world forward)
                var forwardA = transform.forward;
                var forwardB = lastRotationV;
                
                var rot = Vector3.Angle(forwardA, forwardB)*10;
                var sign = Vector3.Cross(forwardB, forwardA);
                if (sign.y < 0)
                {
                    rot = -rot;
                }

                // get the signed difference in these angles
                //var rotationdiff = Mathf.DeltaAngle( angleA, angleB)*1000;

                lastRotationV = transform.forward;
                
                /*var rotationdiff = rotation - lastRotation;
                lastRotation = rotation;*/
                
                rotationBlend = Mathf.Lerp(rotationBlend, rot, Time.deltaTime);
                if (rotationBlend is > 0 and < 0.01f) rotationBlend = 0f;
                if (rotationBlend is < 0 and > -0.01f) rotationBlend = 0f;
                
                //print(rotationBlend);
                _animator.SetFloat("Rotation",rotationBlend);
                _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }
            
  
        
            
        _animator.SetFloat("Speed", _animationBlend);
    }
    
    private void JumpAndGravity()
        {
            if (grounded)
            {
                
                // reset the fall timeout timer
                _fallTimeoutDelta = fallTimeout;
                
                _animator.SetBool("Swimming", isSwimming);
                _animator.SetBool("Jump", false);
                _animator.SetBool("FreeFall", false);
                
                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity <= 0.0f)
                {
                    if (isSwimming)
                    {
                        _verticalVelocity = 0f;
                    }
                    else
                    { _verticalVelocity = -5f;
                    }
                }
                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f && _animator.GetLayerWeight(1) < 1)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    _animator.SetBool("Jump", true);
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                
                // reset the jump timeout timer
                _jumpTimeoutDelta = jumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    if (!isSwimming)
                    {
                        _animator.SetBool("FreeFall", true); 
                    }


                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (isSwimming)
            {
                _verticalVelocity = 0;
            }

            else if (_verticalVelocity < _terminalVelocity)
            {
              
                _verticalVelocity += gravity * Time.deltaTime;
                
            }
        }

    public void setSwimming(bool swimming)
    {
        isSwimming = swimming;
    }
}
