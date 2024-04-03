using System.Collections;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [SerializeField]
    float walkSpeed;

    [SerializeField]
    float jumpForce;

    [SerializeField]
    float gravityMultiplier = 1.5F;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    Vector2 groundCheckSize;

    [SerializeField]
    LayerMask groundMask;

    [SerializeField]
    bool isFacingRight = true;

    Rigidbody2D _rigidbody;

    Animator _animator;

    float _inputX;
    float _velocityY;
    float _gravityY;

    bool _isJumping;
    bool _isJumpPressed;
    bool _isGrounded;

    [Header("Ataques")]
    private float xAxis, yAxis;
    private bool attack = false;
    private float tiempoEntreAttack;
    private float tiempoDesdeAttack;

    [SerializeField]
    Transform transformAtaqueLateral;
    [SerializeField]
    Vector2 areaAtaqueLateral;

    [SerializeField]
    Transform transformAtaqueUp;
    [SerializeField]
    Vector2 areaAtaqueUp;

    [SerializeField]
    Transform transformAtaqueDown;
    [SerializeField]
    Vector2 areaAtaqueDown;

    [SerializeField]
    private float damage;


    [SerializeField]
    LayerMask attackableLayer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _gravityY = Physics2D.gravity.y;
    }

    private void Start()
    {
        _isGrounded = IsGrounded();
        if (!_isGrounded)
        {
            StartCoroutine(WaitForGroundedCoroutine());
        }
    }

    private void Update()
    {
        GetInputs();
        HandleGravity();
        HandleMovement();
        Attack();
    }

    private void FixedUpdate()
    {
        Jump();
        Rotate();
        Move();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transformAtaqueLateral.position, areaAtaqueLateral);
        Gizmos.DrawCube(transformAtaqueUp.position, areaAtaqueUp);
        Gizmos.DrawCube(transformAtaqueDown.position, areaAtaqueDown);


        if (groundCheck == null)
        {
            return;
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }

    private void Rotate()
    {
        if (_inputX == 0.0F)
        {
            return;
        }

        bool facingRight = _inputX > 0.0F;
        if (facingRight != isFacingRight)
        {
            isFacingRight = facingRight;
            transform.Rotate(0.0F, 180.0F, 0.0F);
        }
    }

    private void Move()
    {
        float speed = _inputX != 0.0F ? 1.0F : 0.0F;
        if (speed != _animator.GetFloat("speed"))
        {
            _animator.SetFloat("speed", speed);
        }

        Vector2 velocity = new Vector2(_inputX, 0.0F) * walkSpeed * Time.fixedDeltaTime;
        velocity.y = _velocityY;

      

        _rigidbody.velocity = velocity;
    }

    private void Jump()
    {
        if (_isJumpPressed)
        {
            _animator.SetTrigger("jump");

            _velocityY = jumpForce;
            _isJumpPressed = false;
            _isJumping = true;

            _isGrounded = false;
            StartCoroutine(WaitForGroundedCoroutine());
        }
        else if (!_isGrounded)
        {
            _velocityY += gravityMultiplier * _gravityY * Time.fixedDeltaTime;
            if (!_isJumping)
            {
                _animator.SetTrigger("fall");
            }

            _animator.SetFloat("velocityY", _velocityY);
        }
        else if (_isGrounded)
        {
            if (_velocityY > 0.0F)
            {
                _velocityY = -1;
            }
            else
            {
                _isGrounded = IsGrounded();
                if (!_isGrounded)
                { 
                    StartCoroutine(WaitForGroundedCoroutine());
                }
            }

            if (_isGrounded && _animator.GetFloat("velocityY") != 0.0F)
            {
                _isJumping = false;
                _animator.SetFloat("velocityY", 0.0F);
            }
        }

    }

    private void HandleGravity()
    {
        if (_isGrounded)
        {
            if (_velocityY < -1.0F)
            {
                _velocityY = -1.0F;
            }

            HandleJump();
        }
    }

    private void HandleJump()
    {
        _isJumpPressed = Input.GetButton("Jump");
    }

    private void HandleMovement()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
    }

    private IEnumerator WaitForGroundedCoroutine()
    {
        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(() => IsGrounded());
        _isGrounded = true;
    }

    private bool IsGrounded()
    {
        Collider2D collider2D =
            Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0.0F, groundMask);
        return collider2D != null;
    }

    public IEnumerator Die()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5F);

        // Reiniciar la escena
    }
    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
    }
    void Attack()
    {
        attack = Input.GetMouseButtonDown(0);

        tiempoDesdeAttack += Time.deltaTime;
        if (attack && tiempoDesdeAttack >= tiempoEntreAttack)
        {
            tiempoDesdeAttack = 0;
            _animator.SetTrigger("Attacking");

            if(yAxis == 0 || yAxis < 0 && IsGrounded())
            {
                Hit(transformAtaqueLateral,  areaAtaqueLateral);
            }
            else if (yAxis > 0)
            {
                Hit(transformAtaqueUp, areaAtaqueUp);
            }
            else if (yAxis < 0 && !IsGrounded())
            {
                Hit(transformAtaqueDown, areaAtaqueDown);
            }
        }
    }

    private void Hit(Transform _attacktransform, Vector2 _attackArea)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attacktransform.position, _attackArea,0,attackableLayer);
        Debug.Log("Objetos detectados: " + objectsToHit.Length);

        if (objectsToHit.Length > 0)
        {
            Debug.Log("Danio");
        }

        for (int i = 0; i < objectsToHit.Length; i++)
        {
            if (objectsToHit[i].GetComponent<PatrolController>() != null)
            {
                objectsToHit[i].GetComponent<PatrolController>().EnemyHit(damage);
            }
        }
    }


}
