using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed;

    private Camera mainCamera;
    private NavMeshAgent agent;
    private CustomInput input = null;
    private int groundLayer;

    private void Awake()
    {
        input = new CustomInput();
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        groundLayer = LayerMask.NameToLayer("Ground");
        Debug.Log(groundLayer);
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
    }

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        Debug.Log(Mouse.current.position.ReadValue());
        RaycastHit raycastHit;
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        bool bHit = Physics.Raycast(ray: ray, hitInfo: out raycastHit, maxDistance: mainCamera.farClipPlane, layerMask:(1<<groundLayer));
        if(bHit)
        {

            agent.SetDestination(raycastHit.point);
        }
    }

}
