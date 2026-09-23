using UnityEngine;

public class PlayerDashState : EntityState
{
    private float originalGravityScale;
    public PlayerDashState(StateMachine stateMachine, Player player, string stateName): base(stateMachine, player, stateName) { }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.dashDuration;
        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;

    }
    public override void Update()
    {
        base.Update();
        CancelDashIfNeeded();
        player.SetVelocity(player.playerDashSpeed * player.facingDir,0);
        if (stateTimer < 0)
        {
            if (player.groundDetected)
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                stateMachine.ChangeState(player.fallState);
            }
        }
    }
    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, 0);
        rb.gravityScale = originalGravityScale;
    }
    private void CancelDashIfNeeded()
    {
        if (player.wallDetected)
        {
            if (player.groundDetected)
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                stateMachine.ChangeState(player.wallSlide);
            }
        }
    }

}
