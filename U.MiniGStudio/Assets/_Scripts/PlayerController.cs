using UnityEditor.Build;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputManager _controllerMap;
    private bool _isMovingHorizontal = false;
    private bool _isMovingVertical = false;

    public delegate void Menu();
    public event Menu onMenu;
    public delegate void Jump();
    public event Jump onJump;
    public delegate void Roll();
    public event Roll onRoll;

    private void Awake()
    {
        _controllerMap = new InputManager();
    }

    public void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }

    private void OnEnable()
    {
        _controllerMap.Enable();
        _controllerMap.Player.Jump.started += ctx => JumpEvent();
        _controllerMap.Player.Roll.started += ctx => RollEvent();
        _controllerMap.Player.Menu.started += ctx => MenuEvent();
    }

    private void OnDisable()
    {
        _controllerMap.Disable();
    }

    private void JumpEvent()
    {
        onJump?.Invoke();
    }

    private void RollEvent()
    {
        onRoll?.Invoke();
    }

    private void MenuEvent()
    {
        onMenu?.Invoke();
    }

    public Vector2 GetMoveDirection() => _controllerMap.Player.Movement.ReadValue<Vector2>();
}
