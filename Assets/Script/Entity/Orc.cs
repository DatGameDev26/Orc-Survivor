using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc : CombatEntity
{
    [Header("Orc")]
    [SerializeField] protected float attackRadius;
    [SerializeField] protected float restTime = 2;

    public OrcIdleState idleState;
    public OrcRunState runState;
    public OrcOnceAnimationState attackUpState;
    public OrcOnceAnimationState attackDownState;
    public OrcOnceAnimationState damageState;

    protected Transform playerT;
    protected float attackTimer;
    protected float restTimer;
    protected override void Awake()
    {
        base.Awake();
        idleState = new OrcIdleState(stateMachine, this, "Idle", this);
        runState = new OrcRunState(stateMachine, this, "Run", this);
        attackUpState = new OrcOnceAnimationState(stateMachine, this, "attackUp", this);
        attackDownState = new OrcOnceAnimationState(stateMachine, this, "attackDown", this);
        damageState = new OrcOnceAnimationState(stateMachine, this, "Damage", this);

    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if(playerGO != null) playerT = playerGO.transform;
    }

    protected override void Update()
    {
        base.Update();
        attackTimer -= Time.deltaTime;
        restTimer -= Time.deltaTime;
        checkPlayer();
    }

    private void checkPlayer()
    {
        if (attackTimer > 0) return;

        if (playerT != null)
        {
            if (restTimer > 0) return;

            Vector2 targetDir = (playerT.position - transform.position).normalized;
            MoveTo(targetDir);
            if(Vector2.Distance(transform.position, playerT.position) <= attackRadius && attackTimer <= 0)
            {
                bool isFacingPlayer = (targetDir.x > 0 && isFacingRight) || (targetDir.x < 0 && !isFacingRight);
                if (!isFacingPlayer) Flip();
                if (targetDir.y > 0) stateMachine.ChangeState(attackUpState);
                else stateMachine.ChangeState(attackDownState);
                restTimer = 2;
            }
        }
    }

    public override void allowToMove()
    {
        base.allowToMove();
        stateMachine.ChangeState(idleState);
    }

    public override void Damage(int damage)
    {
        restTimer = 1;
        stateMachine.ChangeState(damageState);
        base.Damage(damage);
    }
}

public class OrcState : State
{
    protected Orc orc;
    public OrcState(StateMachine stateMachine, Entity entity, string animBoolName, Orc orc) : base(stateMachine, entity, animBoolName)
    {
        this.orc = orc;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}

public class OrcIdleState : OrcState
{
    public OrcIdleState(StateMachine stateMachine, Entity entity, string animBoolName, Orc orc) : base(stateMachine, entity, animBoolName, orc)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        if (!orc.isIdle()) stateMachine.ChangeState(orc.runState);
    }

    public override void Exit()
    {
        base.Exit();
    }

}

public class OrcRunState : OrcState
{
    public OrcRunState(StateMachine stateMachine, Entity entity, string animBoolName, Orc orc) : base(stateMachine, entity, animBoolName, orc)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        if (orc.isIdle()) stateMachine.ChangeState(orc.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }

}

public class OrcOnceAnimationState : OrcState
{
    public OrcOnceAnimationState(StateMachine stateMachine, Entity entity, string animBoolName, Orc orc) : base(stateMachine, entity, animBoolName, orc)
    {
    }

    public override void Enter()
    {
        base.Enter();
        orc.stopMoving();
    }
    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }

}
