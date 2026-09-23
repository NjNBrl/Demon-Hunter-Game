using UnityEngine;

public class PlayerIdleState : GroundedState
{
    public PlayerIdleState(StateMachine stateMachine,Player player, string stateName) : base(stateMachine,player,stateName) { }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0,rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();
        if (player.moveInput.x!=0)
        {
            stateMachine.ChangeState(player.moveState);
        }
        if (player.input.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.jumpState);
            
        }
    }
}
