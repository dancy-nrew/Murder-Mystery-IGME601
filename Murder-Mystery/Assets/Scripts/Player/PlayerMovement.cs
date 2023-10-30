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
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float interactionDistance = 2f;

    private Camera mainCamera;
    private NavMeshAgent agent;
    private CustomInput input = null;

    private Interactable currentInteractable = null;
    private bool bIsUIEnabled = false;

    private void Awake()
    {
        input = new CustomInput();
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
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
        Debug.Log("Layer: " + layerMask.value);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentInteractable != null)
        {
            if(Vector3.Distance(transform.position, currentInteractable.transform.position) <= interactionDistance)
            {
                currentInteractable.OnInteraction();
                bIsUIEnabled = true;
                currentInteractable=null;
            }
        }
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        // Checcking to see if some ui is currently being displayed
        if (bIsUIEnabled)
            return;

        RaycastHit raycastHit;
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        bool bHit = Physics.Raycast(ray: ray, hitInfo: out raycastHit, maxDistance: mainCamera.farClipPlane, layerMask:(layerMask.value));
        if(bHit)
        {
            // Checking to see if an interactable has been clicked
            Interactable interactable = raycastHit.collider.gameObject.GetComponentInParent<Interactable>();
            if(interactable != null)
            {
                Debug.Log("hit Interactable");
                agent.SetDestination(raycastHit.point);
                currentInteractable = interactable;
            }
            else
            {
                agent.SetDestination(raycastHit.point);
                currentInteractable = null;
            }
            
        }
    }

    public void SetIsUIEnabled(bool value)
    {
        bIsUIEnabled = value;
    }
}
