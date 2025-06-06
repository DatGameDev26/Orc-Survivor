using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator animator;
    protected AudioPlayer audioPlayer;
    protected Rigidbody2D rb;
    public bool isFacingRight { get; set; }

    protected virtual void Awake()
    {
        isFacingRight = true;

        rb = GetComponent<Rigidbody2D>();
        audioPlayer = GetComponent<AudioPlayer>();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        handleFlip();
    }

    #region Flip Functions
    protected virtual void handleFlip()
    {
        if (rb.velocity.x > 0 && !isFacingRight) Flip();
        else if (rb.velocity.x < 0 && isFacingRight) Flip();
    }

    public void Flip()
    {
        Vector3 flipVector = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        transform.localScale = flipVector;
        isFacingRight = !isFacingRight;
    }

    #endregion
}
