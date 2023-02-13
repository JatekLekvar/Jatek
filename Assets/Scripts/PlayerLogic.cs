using UnityEngine;
using System.Collections.Generic;


public class PlayerLogic : MonoBehaviour
{
    public List<string> heldAbilities = new List<string>();

    private float gravity = -64f;
    private float runSpeed = 10f;
    private float groundDamping = 28f;
    private float inAirDamping = 8f;
    private float jumpHeight = 12f;
    private float jumpLength = 0.2f;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private PlayerPhysics _controller;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;
    private float _jumpTimer;

    void Awake()
    {
        _controller = GetComponent<PlayerPhysics>();

        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        // _controller.onTriggerExitEvent += onTriggerExitEvent;
    }


    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update()
    {
        if (_controller.isGrounded)
        {
            _velocity.y = 0;
        }

        if (Input.GetKey(KeyCode.D))
        {
            normalizedHorizontalSpeed = 1;
            if (transform.localScale.x < 0f)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }

            if (_controller.isGrounded)
            {
                // _animator.Play(Animator.StringToHash("Run"));
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            normalizedHorizontalSpeed = -1;
            if (transform.localScale.x > 0f)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }

            if (_controller.isGrounded)
            {
                // _animator.Play(Animator.StringToHash("Run"));
            }
        }
        else
        {
            normalizedHorizontalSpeed = 0;

            if (_controller.isGrounded)
            {
                // _animator.Play(Animator.StringToHash("Idle"));
            }
        }


        if (_controller.isGrounded && Input.GetKey(KeyCode.W))
        {
            _jumpTimer = jumpLength;
            // _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            // _animator.Play(Animator.StringToHash("Jump"));
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


        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping;
        _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);
        _velocity.y += gravity * Time.deltaTime;

        _controller.move(_velocity * Time.deltaTime);
        _velocity = _controller.velocity;
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
