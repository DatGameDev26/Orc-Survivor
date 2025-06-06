using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] EnemyType enemyType;
    [SerializeField] protected int swordDamage;
    [SerializeField] protected float pushForce;
    [SerializeField] protected GameObject hitEffect;
    [SerializeField] protected GameObject shieldEffect;

    [Header("Sword Sound")]
    [SerializeField] AudioClip[] allAttackSounds;
    [SerializeField] AudioClip[] allHitSounds;
    protected AudioPlayer audioPlayer;

    protected virtual void Start()
    {
        audioPlayer = GetComponent<AudioPlayer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == enemyType.ToString())
        {
            CombatEntity combatEntity = collision.GetComponent<CombatEntity>();
            if (combatEntity != null)
            {
                int startHealth = combatEntity.getCurrentHealth();
                combatEntity.Damage(swordDamage);
                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                audioPlayer.playRandomSFXinList(allHitSounds);

                if (rb != null)
                {
                    Vector2 forceDir = (collision.transform.position - transform.position).normalized;
                    rb.AddForce(forceDir * pushForce, ForceMode2D.Impulse);
                }
                if (hitEffect != null)
                {
                    Vector2 hitPoint = (collision.transform.position + transform.position) / 2;
                    if (startHealth != combatEntity.getCurrentHealth())
                    {
                        Instantiate(hitEffect, hitPoint, Quaternion.identity);
                    }
                    else
                    {
                        if(shieldEffect != null) Instantiate(shieldEffect, hitPoint, Quaternion.identity);
                    }
                }

            }

        }
    }

    public void playAttackSound()
    {
        audioPlayer.playRandomSFXinList(allAttackSounds);
    }


}

public enum EnemyType
{
    Orc, Player
}