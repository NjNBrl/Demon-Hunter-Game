using UnityEngine;

public class WallJumpState : EntityState
{
    public WallJumpState(StateMachine stateMachine, Player player, string stateName) : base(stateMachine, player, stateName) { }
    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(player.wallJumpDirection.x * -player.facingDir, player.jumpForce);
    }

    public override void Update() 
    {
        base.Update();
        if (rb.linearVelocity.y < 0)
        {
            stateMachine.ChangeState(player.fallState);
        }
        if (player.wallDetected)
        {
            stateMachine.ChangeState(player.wallSlide);
        }
    }
}

