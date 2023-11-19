using UnityEngine;

public class SceneData : MonoBehaviour
{
    public Vector3 Lane1Transform;
    public Vector3 Lane2Transform;
    public Vector3 Lane3Transform;

    public Vector3 GetLaneTransform(int lane){
        // Provide the relevant transform information for the lane objects
        // Input is the lane as an integer
        Vector3 transform;

        if (lane == ConstantParameters.LANE_1){
            transform = Lane1Transform;
        } else if (lane == ConstantParameters.LANE_2){
            transform = Lane2Transform;
        } else {
            transform = Lane3Transform;
        }
        return transform;
    }
}
