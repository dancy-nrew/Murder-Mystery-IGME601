using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementController : MonoBehaviour
{
    /*
        This class can be used to animate all kinds of movement for game pieces. Extend this class to add new movement.

        To Do:
        - Still need to add rotations
        - Add a staging movement to build anticipation before turn reveal.
    */
    private int _elapsedFrames;
    private Vector3 _origin;
    private Quaternion _startRotation;
    private List<MovementDefinition> _movements;
    private int _activeMovementIndex;
    private bool _isMoving;
    private float _journeyDistance;

    private struct MovementDefinition
    {
        public Vector3 destination;
        public Quaternion endRotation;
        public int durationInFrames;
        public bool isFlip;

        public MovementDefinition(Vector3 dest, int duration)
        {
            destination = dest;
            endRotation = Quaternion.identity;
            durationInFrames = duration;
            isFlip = false;
        }

        public MovementDefinition(Quaternion rotation, int duration)
        {
            destination = new Vector3(0,0,0);
            endRotation = rotation;
            durationInFrames = duration;
            isFlip = true;
        }
    }

    void Awake(){
        _isMoving = false;
        _activeMovementIndex = 0;
        _movements = new List<MovementDefinition>();
        _elapsedFrames = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isMoving || _movements.Count == 0){
            return;
        }
        MovementDefinition activeMovement = _movements[_activeMovementIndex];
        
        _elapsedFrames += 1;
        float interpolationRatio = (float) _elapsedFrames / activeMovement.durationInFrames;
        if (activeMovement.isFlip) {
            // Interpolate the rotation
            transform.rotation = Quaternion.Slerp(_startRotation, activeMovement.endRotation, interpolationRatio);
            // Move up and down to give that flip feel
            float flipHeight = Mathf.Sin(interpolationRatio * Mathf.PI) * ConstantParameters.FLIP_HEIGHT;
            transform.position = new Vector3(_origin.x, _origin.y + flipHeight, _origin.z); 
        } else
        {
            transform.position = Vector3.Lerp(_origin, activeMovement.destination, interpolationRatio);
        }

        if (interpolationRatio == 1){
            // Move to the next movement in the list;
            _activeMovementIndex++;
            
            if (_activeMovementIndex == _movements.Count)
            {
                // We have reached the end of the list
                _activeMovementIndex = 0;
                _movements.Clear();
                ToggleMovement();
            }

            // Update the starting parameters for the next move
            _elapsedFrames = 0;
            _origin = transform.position;
            _startRotation = transform.rotation;
        }
    }

    public void AddMovement(Vector3 destination, int duration)
    {
        MovementDefinition def = new MovementDefinition(destination, duration);
        _movements.Add(def);
    }

    public void AddFlip(int duration)
    {
        MovementDefinition def = new MovementDefinition(transform.rotation * Quaternion.Euler(0,0, 180f), duration);
        _movements.Add(def);
    }

    public void ToggleMovement(){
        // Interrupts or starts movement.
        _isMoving = !_isMoving;
    }



}
