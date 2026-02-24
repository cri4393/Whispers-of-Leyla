using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Gravity")]
    [HideInInspector] public float gravityScale = 2; //Player gravity scale (rb.gravityScale)
    public float gravityFall = 3;
    public float maxFallSpeed = 40;
    public float jumpCutGravityMult = 4;

    [Space(5)]
    [Header("Run")]
    public float runMaxSpeed = 13; //Target speed we want the player to reach
    public float runAcceleration = 1;
    [HideInInspector] public float runAccelAmount = .2f;
    public float runDecceleration = 1;
    [HideInInspector] public float runDeccelAmount = .2f;
    [Space(5)]
    [Range(0f, 1)] public float accelInAir;
    [Range(0f, 1)] public float deccelInAir;
    [Space(5)]
    public bool doConserveMomentum = true;

    [Space(5)]
    [Header("Jump")]
    public float jumpForce = 100;
    [Range(0.01f, 0.5f)] public float coyoteTime = .1f;
    [Range(0.01f, 0.5f)] public float jumpInputBufferTime;
    [Range(.1f, 2f)] public float jumpHangGravityMult;
    public float jumpHangTimeThreshold = .1f;

    [Space(5)]
    [Header("Slide")]
    public float slideSpeed = 5;
    public float slideAccel = 3;

    [Space(5)]
    [Header("WallJump")]
    [Range(0f, 1.5f)] public float wallJumpTime = .7f;
    public Vector2 wallJumpForce;
    [Range(0f, 1f)] public float wallJumpRunLerp;

    private void OnValidate()
    {
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;
    }
}
