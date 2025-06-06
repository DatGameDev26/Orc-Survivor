using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CombatEntity
{
    #region All Player States
    public PlayerIdleState idleState;
    public PlayerRunState runState;
    public PlayerAttackState attackUpState;
    public PlayerAttackState attackDownState;
    public PlayerAttackState chargedAttackState;
    public PlayerBlockState blockState;
    public PlayerDamageState damageState;
    #endregion

    [Header("Charge Info")]
    [SerializeField] int chargeRequire = 3;
    [SerializeField] float chargedForce;
    [Header("Block")]
    [SerializeField] float maxBlock;
    [SerializeField] float blockPerUse;
    [SerializeField] AudioClip blockSFX;
    float currentBlock;
    public int chargeCount { get; set; }

    public bool canUseSword { get; set; }
    GameOverManager gameOverManager;

    protected override void Awake()
    {
        base.Awake();

        canUseSword = true;
        idleState = new PlayerIdleState(stateMachine, this, "Idle", this);
        runState = new PlayerRunState(stateMachine, this, "Run", this);
        attackUpState = new PlayerAttackState(stateMachine, this, "attackUp", this);
        attackDownState = new PlayerAttackState(stateMachine, this, "attackDown", this);
        chargedAttackState = new PlayerAttackState(stateMachine, this, "chargedAttack", this);
        blockState = new PlayerBlockState(stateMachine, this, "Block", this);
        damageState = new PlayerDamageState(stateMachine, this, "Damage", this);

    }


    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        currentBlock = maxBlock;
        gameOverManager = GameObject.FindGameObjectWithTag("GameOverManager").GetComponent<GameOverManager>();
    }

    protected override void Update()
    {
        base.Update();
        if (isDead || gameOverManager.isGameEnd()) return;

        stateMachine.UpdateCurrentState();

        currentBlock += Time.deltaTime * 2;
        if (currentBlock > maxBlock) currentBlock = maxBlock;
        else if (currentBlock < 0) currentBlock = 0;

        InputHandler();
    }

    private void InputHandler()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        MoveTo(new Vector2(inputX, inputY));

        if (canUseSword && (Input.GetMouseButton(0) || Input.GetMouseButton(1)))
        {
            bool isMouseUpper = Input.mousePosition.y > Screen.height / 2;
            bool isMouseOnRight = Input.mousePosition.x > Screen.width / 2;
            if ((!isFacingRight && isMouseOnRight) || (isFacingRight && !isMouseOnRight))
            {
                Flip();
            }

            if (Input.GetMouseButton(0))
            {
                if (chargeCount >= chargeRequire)
                {
                    stateMachine.ChangeState(chargedAttackState);
                    Vector2 chargedDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
                    rb.AddForce(chargedDir * chargedForce, ForceMode2D.Impulse);

                    chargeCount = 0;
                }
                else
                {
                    if (isMouseUpper) stateMachine.ChangeState(attackUpState);
                    else stateMachine.ChangeState(attackDownState);
                }
            }
            else if (Input.GetMouseButton(1))
            {
                if (canBlock()) stateMachine.ChangeState(blockState);
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
        if (stateMachine.currentState == blockState)
        {
            audioPlayer.playSFX(blockSFX);
            currentBlock -= blockPerUse;
            chargeCount++;
            if (chargeCount >= chargeRequire) chargeCount = chargeRequire;
            return;
        }
        stateMachine.ChangeState(damageState);
        base.Damage(damage);
    }
    public override void doSomethingBeforeDeath()
    {
        base.doSomethingBeforeDeath();
        gameOverManager.EndGame("Lose");
    }
    public float getCurrentBlock() { return currentBlock; }
    public float getMaxBlock() { return maxBlock; }
    public bool canBlock() { return currentBlock >= blockPerUse; }
    public int getCurrentCharge() { return chargeCount; }
    public int getChargeRequire() { return chargeRequire; }
}

public class PlayerState : State
{
    protected Player player;
    public PlayerState(StateMachine stateMachine, Entity entity, string animBoolName, Player player)
        : base(stateMachine, entity, animBoolName)
    {
        this.player = player;
    }

    public override void Enter()
    {
        base.Enter();
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
public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(StateMachine stateMachine, Entity entity, string animBoolName, Player player) : base(stateMachine, entity, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (!player.isIdle()) stateMachine.ChangeState(player.runState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
public class PlayerRunState : PlayerState
{
    public PlayerRunState(StateMachine stateMachine, Entity entity, string animBoolName, Player player) : base(stateMachine, entity, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (player.isIdle()) stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(StateMachine stateMachine, Entity entity, string animBoolName, Player player) : base(stateMachine, entity, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.stopMoving();
        player.canUseSword = false;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
        player.canUseSword = true;
    }
}
public class PlayerBlockState : PlayerState
{
    public PlayerBlockState(StateMachine stateMachine, Entity entity, string animBoolName, Player player) : base(stateMachine, entity, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.stopMoving();
        player.canUseSword = false;
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonUp(1) || !player.canBlock())
        {
            player.allowToMove();
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.canUseSword = true;
    }
}
public class PlayerDamageState : PlayerState
{
    public PlayerDamageState(StateMachine stateMachine, Entity entity, string animBoolName, Player player) : base(stateMachine, entity, animBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.canUseSword = false;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
        player.canUseSword = true;
    }
}

