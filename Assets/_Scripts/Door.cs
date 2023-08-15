using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator animator;
    public static Door i;

    private void Awake()
    {
        i = this;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    internal void Open()
    {
        animator.SetBool("open", true);
    }

    internal void Close()
    {
        animator.SetBool("open", false);
    }

}
