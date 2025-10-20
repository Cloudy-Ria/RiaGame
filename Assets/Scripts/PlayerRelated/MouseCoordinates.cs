using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCoordinates : MonoBehaviour
{
    private InputAction pointAction;
    [SerializeField] private Vector2 whereIsMyMouse;
    void Start()
    {
        pointAction = InputSystem.actions.FindAction("Point");
    }
    void Update()
    {
        whereIsMyMouse = pointAction.ReadValue<Vector2>();
    }

    public Vector2 showMouse()
    {
        return whereIsMyMouse;
    }
}
