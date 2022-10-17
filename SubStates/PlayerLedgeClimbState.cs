
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 detectedPos;
    private Vector2 cornerPos;
    private Vector2 startPos;
    private Vector2 stopPos;

    private bool isHanging;
    private bool isClimbing;

    private int xInput;
    private int yInput;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        player.Anim.SetBool("climbLedge", false);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        player.RB.sharedMaterial = player.FullFriction;

        core.Movement.SetVelocityZero();
        player.transform.position = detectedPos;
        cornerPos = core.CollisionSenses.DetermineCornerPosition();

        startPos.Set(cornerPos.x - (core.Movement.FacingDirection * playerData.startOffset.x), cornerPos.y - playerData.startOffset.y);
        stopPos.Set(cornerPos.x + (core.Movement.FacingDirection * playerData.stopOffset.x), cornerPos.y + playerData.stopOffset.y);

        player.transform.position = startPos;

        isClimbing = false;
        isHanging = true;

        player.Anim.SetBool("climbLedge", false);
    }

    public override void Exit()
    {
        base.Exit();

        player.RB.sharedMaterial = player.NoFriction;

        if(isClimbing)
        {
            player.transform.position = stopPos;
        }

        isHanging = false;
        isClimbing = false;

        player.Anim.SetBool("climbLedge", false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else
        {
            xInput = player.InputHandler.NormInputX;
            yInput = player.InputHandler.NormInputY;

            if (core.CollisionSenses.Wall && !core.CollisionSenses.LedgeHorizontal)
            {
                SetDetectedPosition(player.transform.position);
                cornerPos = core.CollisionSenses.DetermineCornerPosition();
                startPos.Set(cornerPos.x - (core.Movement.FacingDirection * playerData.startOffset.x), cornerPos.y - playerData.startOffset.y);
                stopPos.Set(cornerPos.x + (core.Movement.FacingDirection * playerData.stopOffset.x), cornerPos.y + playerData.stopOffset.y);
                core.Movement.SetVelocityZero();
                player.transform.position = startPos;
            }

            if (xInput == core.Movement.FacingDirection && player.InputHandler.JumpInput && isHanging && !isClimbing && !core.CollisionSenses.Ceiling)
            {
                isClimbing = true;
                player.Anim.SetBool("climbLedge", true);
            }
            else if (yInput == -1 && isHanging && !isClimbing)
            {
                player.transform.position = startPos;
                stateMachine.ChangeState(player.InAirState);
            }
            else if (xInput != core.Movement.FacingDirection && player.InputHandler.JumpInput && isHanging && !isClimbing)
            {
                player.transform.position = startPos;
                stateMachine.ChangeState(player.JumpState);
                player.JumpState.ResetJumps();
            }
        }

        if (!core.CollisionSenses.Wall || core.CollisionSenses.Ground || core.CollisionSenses.LedgeHorizontal)
        {
            stateMachine.ChangeState(player.InAirState);
        }
    }

    public void SetDetectedPosition(Vector2 position) => detectedPos = position;
}
