using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMonster : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.z -= 0.0001f;
        transform.position = pos;
    }

}
