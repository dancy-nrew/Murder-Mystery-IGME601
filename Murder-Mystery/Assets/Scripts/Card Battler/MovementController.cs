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
    private int _movementDurationInFrames;
    private int _elapsedFrames;
    private Vector3 _origin;
    private Vector3 _destination;
    private bool _isMoving;
    private float _journeyDistance;

    void Awake(){
        _isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isMoving){
            return;
        }

        _elapsedFrames += 1;
        float interpolationRatio = (float) _elapsedFrames / _movementDurationInFrames;
        transform.position = Vector3.Lerp(_origin, _destination, interpolationRatio);

        if (transform.position == _destination){
            ToggleMovement();
        }


    }

    public void SetDestination(Vector3 destination){
        // Set the destination for movement. Input is a Vector3 object that defines where the end should be.
        _origin = transform.position;
        _destination = destination;
        _journeyDistance = Vector3.Distance(_destination, _origin);

        // Because distance has been recalculated, the elapsed frames spent moving need to reset as well
        _elapsedFrames = 0;

    }

    public void SetMovementDuration(int frames){
        // Set how long the movement is meant to last in terms of frames. Few frames means fast movement.
        // Input is the frames as an int.
        _movementDurationInFrames = frames;
    }

    public void ToggleMovement(){
        // Interrupts or starts movement.
        _isMoving = !_isMoving;
    }

}
