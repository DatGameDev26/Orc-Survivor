using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Orc
{
    [Header("Boss")]
    [Header("Teleport Info")]
    [SerializeField] GameObject teleportVFX;
    [SerializeField] float teleportTime;
    [SerializeField] Vector2 maxPointTeleport;
    [SerializeField] Vector2 minPointTeleport;
    [SerializeField] AudioClip teleportSFX;
    [Header("Clone Info")]
    [SerializeField] bool canUseClone;
    [SerializeField] GameObject bossClone;
    [SerializeField] GameObject cloneDieEffect;

    GameObject currentClone;
    float teleportTimer;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        teleportTimer = 4;
    }

    protected override void Update()
    {
        base.Update();

        if (isDead || playerT == null) return;

        teleportTimer -= Time.deltaTime;

        if (teleportTimer < 0)
        {
            Teleport();
        }
    }

    private void Teleport()
    {
        Vector2 telPos = new Vector2(Random.Range(minPointTeleport.x, maxPointTeleport.x),
            Random.Range(minPointTeleport.y, maxPointTeleport.y));
        audioPlayer.playSFX(teleportSFX);
        transform.position = telPos;

        Instantiate(teleportVFX, transform.position, Quaternion.identity);
        teleportTimer = Random.Range(teleportTime - 1f, teleportTime + 1);

        if (!canUseClone || currentClone != null) return;
        spawnClone();
    }

    private void spawnClone()
    {
        Vector2 spawnPos = new Vector2(Random.Range(minPointTeleport.x, maxPointTeleport.x),
                        Random.Range(minPointTeleport.y, maxPointTeleport.y));
        GameObject clone = Instantiate(bossClone, spawnPos, Quaternion.identity);
        Destroy(clone, 10);
        currentClone = clone;
    }

    public override void doSomethingBeforeDeath()
    {
        if(!canUseClone)
        {
            Instantiate(cloneDieEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        base.doSomethingBeforeDeath();

        if(currentClone != null)
        {
            Destroy(currentClone);
        }
    }
}
