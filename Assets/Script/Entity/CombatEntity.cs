using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEntity : Entity
{
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int speed;

    [Header("Entity Audio")]
    [SerializeField] AudioClip[] allDamageSounds;
    [SerializeField] AudioClip dieSound;

    protected StateMachine stateMachine;
    protected bool canMove;
    protected bool isDead;
    protected int currentHealth;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine();
        currentHealth = maxHealth;
        canMove = true;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (!isDead)
        {
            stateMachine.UpdateCurrentState();
        }
    }


    #region Move Functions
    protected void MoveTo(Vector2 direction)
    {
        if (!canMove || isDead) return;
        rb.velocity = direction * speed;
    }

    public bool isIdle() { return rb.velocity.magnitude < 0.0000001f; }

    public virtual void allowToMove() { canMove = true; }
    public void stopMoving()
    {
        canMove = false;
        rb.velocity = Vector3.zero;
    }
    protected override void handleFlip()
    {
        if (!canMove) return;
        base.handleFlip();
    }
    #endregion

    #region Damage Functions
    public virtual void Damage(int damage)
    {
        audioPlayer.playRandomSFXinList(allDamageSounds);
        currentHealth -= damage;
        canMove = false;

        if (currentHealth <= 0 && !isDead)
        {
            currentHealth = 0;
            doSomethingBeforeDeath();
        }
    }

    public virtual void doSomethingBeforeDeath() {
        isDead = true;
        GetComponent<Collider2D>().enabled = false;
        stateMachine.currentState.Exit();
        animator.SetBool("Die", true);
        audioPlayer.playSFX(dieSound);
    }

    public void Die(){ Destroy(gameObject); }
    #endregion
    public int getCurrentHealth() { return currentHealth; }
    public int getMaxHealth() { return maxHealth; }

}

