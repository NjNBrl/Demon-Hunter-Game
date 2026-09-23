using UnityEngine;

public class JumpState : PlayerAirState
{
    public JumpState(StateMachine stateMachine, Player player, string stateName) : base(stateMachine, player, stateName) { }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(rb.linearVelocity.x, player.jumpForce);
    }
    public override void Update()
    {
        base.Update();
        if (rb.linearVelocity.y < -0.1 && stateMachine.currentState != player.jumpAttackState)
        {
            stateMachine.ChangeState(player.fallState);
        }
    }
}
