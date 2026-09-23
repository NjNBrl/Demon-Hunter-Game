using UnityEngine;

public class PlayerMoveState : PlayerAirState
{
       public PlayerMoveState(StateMachine stateMachine, Player player, string stateName) : base(stateMachine, player,stateName) { }

    public override void Update()
    {
        base.Update();
        if (player.moveInput.x == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
        player.SetVelocity(player.moveInput.x * (player.moveSpeed-0.2f), rb.linearVelocity.y);
    }
}

