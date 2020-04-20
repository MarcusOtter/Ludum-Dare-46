using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOffset : MonoBehaviour
{
    private float offsetTime;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        offsetTime = Time.time + Random.Range(0.0f, 1f);
    }
    public void Update()
    {
        if(Time.time > offsetTime)
        {
            animator.enabled = true;
            Destroy(this);
        }
    }
}
