using UnityEngine;

public class GroundedState : EntityState
{
    public GroundedState(StateMachine stateMachine, Player player, string stateName): base(stateMachine, player, stateName) { }
    public override void Update()
    {
        base.Update();
        if (rb.linearVelocity.y < 0)
        {
            stateMachine.ChangeState(player.fallState);
        }

        if (input.Player.Jump.WasPerformedThisFrame())
        {
            stateMachine.ChangeState(player.jumpState);
        }
        if (input.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.playerBasicAttack);
        }
    }
}
