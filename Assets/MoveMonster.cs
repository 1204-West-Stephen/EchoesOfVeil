using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMonster : MonoBehaviour
{
    private Animator animator;
    public float speed;

    void Start()
    {
        animator = GetComponent<Animator>();
        speed = speed / 10000f;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.z -= speed;
        transform.position = pos;
    }

}
