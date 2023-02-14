using UnityEngine;
using System.Collections.Generic;

enum State
{
    Idle,
    Walk,
    Jump,
    Crouch,
    Attack,
}


public class PlayerLogic : MonoBehaviour
{
    public List<string> heldAbilities = new List<string>();
    public GameObject firePrefab;

    private float gravity = -64f;
    private float runSpeed = 10f;
    private float groundDamping = 28f;
    private float inAirDamping = 8f;
    private float jumpHeight = 12f;
    private float jumpLength = 0.2f;
    private float attackLength = 0.3f;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private PlayerPhysics _controller;
    private RaycastHit2D _lastControllerColliderHit;
    private SpriteAnimator _animator;
    private BoxCollider2D _collider;
    private Vector3 _velocity;
    private float _jumpTimer;
    private float _attackTimer;
    private float _attackCooldown;
    private State state = State.Idle;
    private bool left;

    void Awake()
    {
        _controller = GetComponent<PlayerPhysics>();

        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        // _controller.onTriggerExitEvent += onTriggerExitEvent;

        _animator = GetComponent<SpriteAnimator>();
        _collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        State oldState = state;
        state = State.Idle;

        bool crouch = Input.GetKey(KeyCode.S) && _controller.isGrounded;

        if (_controller.isGrounded)
        {
            _velocity.y = 0;
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (!crouch)
            {
                state = State.Walk;
                normalizedHorizontalSpeed = 1;
                if (transform.localScale.x < 0f)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }
            left = false;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (!crouch)
            {
                state = State.Walk;
                normalizedHorizontalSpeed = -1;
                if (transform.localScale.x > 0f)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }
            left = true;
        }
        else
        {
            normalizedHorizontalSpeed = 0;
        }

        if (crouch)
        {
            normalizedHorizontalSpeed = 0;

        }


        if (_controller.isGrounded && Input.GetKey(KeyCode.W))
        {
            _jumpTimer = jumpLength;
        }

        if (_jumpTimer > 0f && Input.GetKey(KeyCode.W))
        {
            _jumpTimer -= Time.deltaTime;
            _velocity.y = jumpHeight;
        }

        if (!Input.GetKey(KeyCode.W))
        {
            _jumpTimer = 0f;
        }

        if (!_controller.isGrounded)
        {
            state = State.Jump;
        }

        bool spit = Input.GetKey(KeyCode.LeftShift);

        if (crouch)
        {
            state = State.Crouch;

            _collider.offset = new Vector2(0f, -1f);
            _collider.size = new Vector2(2f, 1f);
        }
        else
        {
            if (state != State.Attack && _attackCooldown <= 0f && Input.GetKeyDown(KeyCode.Space))
            {
                state = State.Attack;
                _attackTimer = attackLength;

                if (spit)
                {
                    GameObject fire = (GameObject)Instantiate(firePrefab, transform.position, Quaternion.identity);
                    Fire fireComp = fire.GetComponent<Fire>();
                    fireComp.direction = left ? -1f : 1f;
                }
            }

            _collider.offset = new Vector2(0f, -0.5f);
            _collider.size = new Vector2(2f, 2f);
        }

        if (_attackTimer > 0f)
        {
            state = State.Attack;
            _attackTimer -= Time.deltaTime;
            _attackCooldown = 0.5f;

            if (_controller.isGrounded)
            {
                normalizedHorizontalSpeed = 0f;
            }
        }

        if (state != State.Attack && _attackCooldown > 0f)
        {
            _attackCooldown -= Time.deltaTime;
        }

        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping;
        _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);
        _velocity.y += gravity * Time.deltaTime;

        _controller.move(_velocity * Time.deltaTime);
        _velocity = _controller.velocity;

        if (state != oldState)
        {
            switch (state)
            {
                case State.Idle:
                    {
                        _animator.Play("Idle");
                    }
                    break;

                case State.Walk:
                    {
                        _animator.Play("Walk");
                    }
                    break;

                case State.Jump:
                    {
                        _animator.Play("Jump");
                    }
                    break;

                case State.Crouch:
                    {
                        _animator.Play("Crouch");
                    }
                    break;

                case State.Attack:
                    {
                        if (spit)
                        {
                            _animator.Play("Spit");
                        }
                        else
                        {
                            _animator.Play("Punch");
                        }
                    }
                    break;
            }
        }
    }

    void onControllerCollider(RaycastHit2D hit)
    {
        if (hit.normal.y < 0f && _velocity.y > 0f)
        {
            _velocity = new Vector3(_velocity.x, 0f, _velocity.z);
            _jumpTimer = 0f;
        }
    }

    void onTriggerEnterEvent(Collider2D collider)
    {
        GameObject obj = collider.gameObject;
        if (obj.name == "AbilityTrigger")
        {
            obj = obj.transform.parent.gameObject;
            Ability ability = obj.GetComponent<Ability>();
            heldAbilities.Add(ability.identifier);
            Destroy(obj);
        }
    }
}
