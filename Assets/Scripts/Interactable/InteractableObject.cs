using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour, i_Interactable
{
    private Vector3 pos;

    private void Start()
    {
        StartCoroutine(Move());
    }

    public void Interact() 
    {
        Debug.Log("Interacted");
    }

    private IEnumerator Move()
    {
        while (true)
        {
            pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            pos.x = transform.position.x + 3;
            transform.position = pos;

            yield return new WaitForSeconds(3f);

            pos.x = transform.position.x - 3;
            transform.position = pos;

            yield return new WaitForSeconds(3f);
        }
    }
}
