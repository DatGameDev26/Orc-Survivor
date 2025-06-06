using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : Sword
{
    [SerializeField] AudioClip[] chargedUpSounds;
    [SerializeField] AudioClip[] specialAttackSounds;
    [SerializeField] GameObject specialHitEffect;
    [SerializeField] Transform swordVisualizer;

    GameObject baseHitEffect;
    int baseDamage;
    float baseForce;
    protected override void Start()
    {
        base.Start();
        baseHitEffect = hitEffect;
        baseDamage = swordDamage;
        baseForce = pushForce;
    }

    private void Update()
    {
        Vector3 swordDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        swordDir.z = 0;
        swordVisualizer.up = swordDir;
    }


    public void playChargedSound()
    {
        audioPlayer.playRandomSFXinList(chargedUpSounds);
    }

    public void playSpecialAttackSound()
    {
        audioPlayer.playRandomSFXinList(specialAttackSounds);
    }

    void setSpecialAttack()
    {
        swordDamage = baseDamage * 4;
        pushForce = baseForce * 2;
        hitEffect = specialHitEffect;
    }

    void setNormalAttack()
    {
        swordDamage = baseDamage;
        pushForce = baseForce;
        hitEffect = baseHitEffect;
    }

}
