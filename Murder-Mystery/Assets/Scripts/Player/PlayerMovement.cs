using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

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

    /*
     * If an interactable has been selected, checking to see if player is close enought to interact with it.
     */
    void Update()
    {
        if(currentInteractable != null)
        {
            Vector2 playerXZPos = new Vector2(transform.position.x, transform.position.z);
            Vector2 interactableXZPos = new Vector2(currentInteractable.transform.position.x, currentInteractable.transform.position.z);
            if (Vector2.Distance( playerXZPos, interactableXZPos) <= interactionDistance)
            {
                currentInteractable.OnInteraction();
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
            if(interactable != null && interactable.bIsInteractable)
            {
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
