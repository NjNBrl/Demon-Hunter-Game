using UnityEngine;

public class PlayerAirState : EntityState
{
    public PlayerAirState(StateMachine stateMachine, Player player, string stateName): base(stateMachine, player, stateName) { }
    public override void Update()
    {
        base.Update();
        if (player.moveInput.x != 0)
        {
            player.SetVelocity(player.moveInput.x *(player.inAirMoveMultiplier* player.moveSpeed), rb.linearVelocity.y);
        }
        if (input.Player.Attack.WasCompletedThisFrame())
        {
            stateMachine.ChangeState(player.jumpAttackState);
        }
    }
}
