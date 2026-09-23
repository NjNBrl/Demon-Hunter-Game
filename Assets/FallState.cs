using UnityEngine;

public class FallState : PlayerAirState
{
    public FallState(StateMachine stateMachine, Player player, string stateName) : base(stateMachine, player, stateName) { }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        if (player.groundDetected || (player.rb.linearVelocity.y == 0))
            stateMachine.ChangeState(player.idleState);

        if (player.wallDetected)
        {
            stateMachine.ChangeState(player.wallSlide);
        }
    }
}
