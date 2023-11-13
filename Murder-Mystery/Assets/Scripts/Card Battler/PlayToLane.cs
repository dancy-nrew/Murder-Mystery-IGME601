using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;


public class PlayToLane : MonoBehaviour
{
    private Vector3 _Lane1Transform;
    
    private Vector3 _Lane2Transform;
    private Vector3 _Lane3Transform;
    private Vector3 _handOrigin;

    //Constants for movement
    
    private const int TO_LANE_MOVEMENT_DURATION = 20;
    private const float CARD_TO_LANE_HEIGHT_DISTANCE = 0.5f;
    private const float CARD_LANE_CENTER_X_OFFSET = 2;
    private const int ROW_OFFSET = -6;
    private const string LANE1_NAME = "Lane1";
    private const string LANE2_NAME = "Lane2";
    private const string LANE3_NAME = "Lane3";
    private BoardManager _boardManager;


    void Awake()
    {
        GameObject sceneDataManager = GameObject.Find("SceneDataManager");
        SceneData sceneData = sceneDataManager.GetComponent<SceneData>();
        _boardManager = sceneDataManager.GetComponent<BoardManager>();
        _Lane1Transform = sceneData.GetLaneTransform(ConstantParameters.LANE_1);
        _Lane2Transform = sceneData.GetLaneTransform(ConstantParameters.LANE_2);
        _Lane3Transform = sceneData.GetLaneTransform(ConstantParameters.LANE_3);
        
    }

    public void Release() {
        // Release mechanism for a card held by mouse. Only the human player has access to this functionality, so all logic
        // here corresponds to player 1.

        // Syntactic Sugar
        LayerMask groundLayer = LayerMask.NameToLayer("Ground");

        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        bool bHit = Physics.Raycast(ray: ray, hitInfo: out raycastHit, maxDistance: Camera.main.farClipPlane, layerMask:(1<<groundLayer));
        if (bHit){
            Vector3 destination;
            // I have hit something in the ground layer, which for this scene must be a lane
            
            if (raycastHit.collider.gameObject.name == LANE1_NAME){
                destination = CalculateMoveToLane(ConstantParameters.LANE_1, ConstantParameters.PLAYER_1);
                UpdateBoardStatus(ConstantParameters.LANE_1, ConstantParameters.PLAYER_1);
            } else if (raycastHit.collider.gameObject.name == LANE2_NAME){
                destination = CalculateMoveToLane(ConstantParameters.LANE_2, ConstantParameters.PLAYER_1);
                UpdateBoardStatus(ConstantParameters.LANE_2, ConstantParameters.PLAYER_1);
            } else{
                destination = CalculateMoveToLane(ConstantParameters.LANE_3, ConstantParameters.PLAYER_1);
                UpdateBoardStatus(ConstantParameters.LANE_3, ConstantParameters.PLAYER_1);
            }

            gameObject.GetComponent<FollowMouse>().ToggleInmovable();
            // Send the info to the movement controller
            EngageMovement(destination, TO_LANE_MOVEMENT_DURATION);
        } else {
            //I am over no lane at all
            EngageMovement(_handOrigin, ConstantParameters.RETURN_TO_HAND_DURATION);
        }
    }
    
    private void UpdateBoardStatus(int lane, int player)
    {
        Card card = GetComponent<Card>();
        
        // Add this card's information to the lane container
        _boardManager.PlayCardToLane(player, lane, card.cardData);
    }

    private Vector3 CalculateMoveToLane(int lane, int player){
        /*
            Determine where this physical object will land, informed by which player made the movement.

            Inputs:
            lane - int to indicate which lane this card is being played
            player - int to indicate which player is making the move
            
            Outputs:
            Destination vector, including any relevant offsets
        */ 

        Vector3 destination;

        // Calculate information for physical offset in lane
        int laneCardCount = _boardManager.GetCardsInLaneForPlayer(player, lane);
        float xOffsetForCard = CARD_LANE_CENTER_X_OFFSET*MathUtil.RemapToPosAndNeg(laneCardCount%2);
        int row = (laneCardCount / 2) + 1;
        if (player == ConstantParameters.PLAYER_2){ row *= -1; }

        // Generally determine destination transform
        if (lane == ConstantParameters.LANE_1){
            destination = _Lane1Transform;
        } else if (lane == ConstantParameters.LANE_2){
            destination = _Lane2Transform;
        } else {
            destination = _Lane3Transform;
        }

        // Modify destination transform by offset
        destination += new Vector3(xOffsetForCard, CARD_TO_LANE_HEIGHT_DISTANCE, row*ROW_OFFSET);
        return destination;

    }

    private void EngageMovement(Vector3 destination, int duration)
    {
        
        ConfigureMovement(destination, duration);
        TriggerMovement();
    }
    
    public void ConfigureMovement(Vector3 destination, int duration){
        /* Set up the movement controller to execute the motion to the lane and trigger the movement.

        Inputs:
        destination - Vector3 that dictates the endpoint of the movement for the card
        duration - int that represents, in count of frames, how long this movement will happen for.
        */
        MovementController mc = gameObject.GetComponent<MovementController>();
        mc.AddMovement(destination, duration);
    }

    private void TriggerMovement()
    {
        MovementController mc = gameObject.GetComponent<MovementController>();
        mc.ToggleMovement();
    }

    public void SetHandOrigin(Vector3 location){
        /* Setter for the origin of this card. This origin marks the spot where cards will return to if not played to a lane.
        Inputs:
        location - Vector3 that indicates the position that represents the cards location in the hand
        */
        _handOrigin = location;
    }

    public void AnticipationMove(int lane)
    {
        /*
            Anticipation move for the AI object
            Inputs:
                The lane to move this card to
            
         */
        MovementController mc = gameObject.GetComponent<MovementController>();
        int suspense_duration = 10 * TO_LANE_MOVEMENT_DURATION;

        //ConfigureMovement(ConstantParameters.AI_STAGING_DESTINATION, suspense_duration);
        ConfigureMovement(CalculateMoveToLane(lane, ConstantParameters.PLAYER_2), TO_LANE_MOVEMENT_DURATION);
        mc.AddWait(suspense_duration / 2);
        mc.AddFlip(suspense_duration*2);
        mc.ToggleMovement();

        UpdateBoardStatus(lane, ConstantParameters.PLAYER_2);
    }
}
