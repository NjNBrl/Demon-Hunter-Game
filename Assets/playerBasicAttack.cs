using UnityEngine;

public class PlayerBasicAttack : EntityState
{
    private float attackVelocityTimer;
    private const int FirstComboIndex = 1;
    private int comboIndex = 1;
    private int comboLimit = 3;
    private float lastTimeAttack;
    private bool comboAttackQueued;
    private int attackDirection;
    public PlayerBasicAttack(StateMachine stateMachine, Player player, string stateName) : base(stateMachine, player, stateName) 
    { 
        if (comboLimit != player.attackVelocity.Length)
        {
            comboLimit = player.attackVelocity.Length;
        }
    }
    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false;
        if (player.moveInput.x != 0)
        {
            attackDirection = ((int)player.moveInput.x);
        }
        else
        {
            attackDirection = player.facingDir;
        }
        // attackDirection = player.moveInput.x !=0 ? ((int)player.moveInput.x) : player.facingDir
        ResetComboIndexIfNeeded();
        anim.SetInteger("basicAttackIndex", comboIndex);
        attackVelocityTimer = player.attackVelocityDuration;
        GenerateAttackVelocity();
    }
    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if (input.Player.Attack.WasPerformedThisFrame())
        {
            QueueNextAttack();
        }

        if (triggerCalled)
        {
            if (comboAttackQueued)
            {
                anim.SetBool(animBooLName, false);
                player.EnterAttackStateWithDelay();
            }
            else
            {
                stateMachine.ChangeState(player.idleState);
            }
        }
    }
    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;
        if (attackVelocityTimer < 0)
        {
            player.SetVelocity(0, rb.linearVelocity.y);
        }
    }
    private void GenerateAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[comboIndex-1];
        attackVelocityTimer = player.attackVelocityDuration;
        player.SetVelocity(attackVelocity.x * attackDirection, attackVelocity.y);
    }
    public override void Exit()
    {
        base.Exit();
        comboIndex++;
        lastTimeAttack = Time.time; // in game time
    }
    private void ResetComboIndexIfNeeded()
    {
        // if time we attacked was long ago, we reset the index

        if (Time.time > lastTimeAttack + player.comboResetTime)
        {
            comboIndex = FirstComboIndex;
        }
        if (comboIndex > comboLimit)
        {
            comboIndex = FirstComboIndex;
        }

    }
    private void QueueNextAttack()
    {
        if(comboIndex < comboLimit)
        {
            comboAttackQueued = true;
        }
    }
}
