using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstracts Input signals into usable commands
/// </summary>
public class InputManager : MonoBehaviour
{
    private Vector2 _commandedMovement;
    public Vector2 CommandedMovement 
    { 
        get => _commandedMovement; 
    }
    private bool _fire;
    public bool Fire
    {
        get => _fire;
    }

    // The Axis names are set based on the Unity Input.GetAxis documenation here:
    // https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
    private const string _verticalAxis = "Vertical";
    private const string _horizontalAxis = "Horizontal";

    // Start is called before the first frame update
    void Start()
    {
        _commandedMovement = new Vector2(0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
       // _commandedMovement.x = Input.GetAxis(_horizontalAxis);
        //_commandedMovement.y = Input.GetAxis(_verticalAxis);
        //_fire = Input.GetButtonDown("Fire1");
    }
}

