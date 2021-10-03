using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOffset : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        animator.SetFloat("circleOffset", Random.Range(0.0f, 1.0f));
    }
}
