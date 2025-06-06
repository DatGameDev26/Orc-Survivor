using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform targetToFollow;
    [SerializeField] Vector3 offset;
    [SerializeField] float smoothTime = 0.25f;
    Vector3 vel = Vector3.zero;

    void Start()
    {
        offset = new Vector3(0.01f, 0.01f, -10);
    }

    void LateUpdate()
    {
        followTarget();
    }

    void followTarget()
    {
        if (targetToFollow != null)
        {
            Vector3 targetPos = targetToFollow.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref vel, smoothTime);
        }
    }

    public void setOffsetX(float newOffsetX)
    {
        offset.x = newOffsetX;
    }

    public void setOffsetY(float newOffsetY)
    {
        offset.y = newOffsetY;
    }

}
