using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class WallSlideState : EntityState
{
    public WallSlideState(StateMachine stateMachine, Player player, string stateName) : base(stateMachine, player, stateName) { }
    public override void Update()
    {
        base.Update();

        HandleWallSlide();
        if (input.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.wallJumpState);
        }
 
        if (player.groundDetected)
        {
            
            stateMachine.ChangeState(player.idleState);
            player.Flip();
            
        }
        if (!player.wallDetected)
        {
            stateMachine.ChangeState(player.fallState);
        }
    }

    private void HandleWallSlide()
    {
        if (player.moveInput.y < 0)
        {
            player.SetVelocity(player.moveInput.x, rb.linearVelocity.y);
        }
        else
        {
            player.SetVelocity(player.moveInput.x, rb.linearVelocity.y * .9f);
        }
    }
}
