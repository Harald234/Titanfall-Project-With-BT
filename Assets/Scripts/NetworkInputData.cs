using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector3 direction;
    public NetworkBool isJump;
    public NetworkBool isSprinting;
    public NetworkBool isCrouching;
    public NetworkBool isAiming;
    public NetworkBool isDashing;
}
