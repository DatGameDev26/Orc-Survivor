using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEntity : MonoBehaviour
{
    [SerializeField] CombatEntity parentEntity;

    void AllowToMove()
    {
        parentEntity.allowToMove();
    }

    void DestroyParent()
    {
        parentEntity.Die();
    }

}
