using UnityEngine;

public class ConstantParameters
{
    #region Card Battle Parameters
    public const int MAX_HAND_SIZE = 9;
    public const int MIN_FACE_VALUE = 3;
    public const int MAX_FACE_VALUE = 10;
    public const int MAX_TURNS = 9;

    public const int PLAYER_1 = 1;
    public const int PLAYER_2 = 2;

    public const int LANE_1 = 1;
    public const int LANE_2 = 2;
    public const int LANE_3 = 3;

    public const int RETURN_TO_HAND_DURATION = 1;

    public const float FLIP_HEIGHT = 0.5f;
    public static Vector3 AI_STAGING_DESTINATION = new Vector3(-18,30,5);
    #endregion
}
