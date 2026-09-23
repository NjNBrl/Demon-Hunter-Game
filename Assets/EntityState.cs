using UnityEngine;
using Unity.VisualScripting;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected Player player;
    protected string animBooLName;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected PlayerInputSet input;
    protected float stateTimer;
    protected bool triggerCalled;

    public EntityState(StateMachine stateMachine, Player player, string stateName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBooLName = stateName;
        anim = player.anim;
        rb = player.rb;
        input = player.input;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBooLName,true);
        triggerCalled = false;
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            stateMachine.ChangeState(player.playerDashState);
        }
    }
    public virtual void Exit()
    {
        player.anim.SetBool(animBooLName, false);
    }
    public void CallAnimationTrigger()
    {
        triggerCalled = true;
    }
    protected void Jump()
    {
        Debug.Log("World cup everybody jump");
    }
    private bool CanDash()
    {
        if (player.wallDetected)
        {
            return false;
        }
        if (stateMachine.currentState == player.playerDashState)
        {
            return false;
        }
        return true;
    }
}
