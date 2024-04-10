using TopDownController;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    Rigidbody _rb;
    CapsuleCollider _capsuleCollider;

    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _rollForce;
    [SerializeField] private float _rollCooldown;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed;

    private bool _isGrounded;
    private bool _canRoll = true;
    private float _lastRollTime;
    private Transform _cameraTransform;

    private void Awake()
    {

    }

    //private void Roll(Vector2 direction)
    //{
    //    if (_canRoll)
    //    {
    //        Vector3 dashDirection = new Vector3(direction.x, 0, direction.y).normalized;
    //        if (dashDirection == Vector3.zero)
    //        {
    //            return;
    //        }
    //        _rb.AddForce(Vector3.up, ForceMode.Impulse);
    //        _rb.AddForce(dashDirection * _rollForce, ForceMode.Impulse);

    //        _canRoll = false;
    //        _lastRollTime = Time.time;
    //    }
    //}
}
