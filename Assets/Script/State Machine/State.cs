
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class State
{
    protected StateMachine stateMachine;
    protected Entity entity;
    protected float stateTimer;
    protected string animBoolName;
    protected bool triggerCalled;

    public State(StateMachine stateMachine, Entity entity, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.entity = entity;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        if (animBoolName != null) entity.animator.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        entity.animator.SetBool(animBoolName, false);
    }

    public void finishAnimationTrigger()
    {
        triggerCalled = true;
    }

    public bool isAnimationFinish()
    {
        return triggerCalled;
    }

}
