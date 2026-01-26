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
       if (automatic == true)
        {
            animator.SetBool("Automatic", true);
        }

        if (Input.GetKeyDown("up"))
        {
            animator.SetBool("go_ahead", true);                    
         }

        if (Input.GetKeyUp("up"))
        {
          animator.SetBool("go_ahead", false);
        }

        if (Input.GetKeyDown("down"))
        {
            animator.SetBool("go_back", true);
        }
        if (Input.GetKeyUp("down"))
        {
            animator.SetBool("go_back", false); 
        }
    }
}
