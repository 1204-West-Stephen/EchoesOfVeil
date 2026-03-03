using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation_book : MonoBehaviour
{
    private Animator animator;
    public bool automatic = false;

    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
