using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JailToHallway : MonoBehaviour
{
    public GameObject jumpscareObject;
    public float jumpscareSpeed;
    public AudioClip jumpscareSound;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger: " + other.name);

        if (other.CompareTag("Player"))
        {
            Jumpscare(other);
        }
    }

    private void Jumpscare(Collider player)
    {
        StartCoroutine(JumpscareRoutine(player.transform));
    }

    IEnumerator JumpscareRoutine(Transform player)
    {
        float speed = jumpscareSpeed;
        float stopDistance = 0.005f;

        AudioSource.PlayClipAtPoint(jumpscareSound, player.position, 1f);

        while (true)
        {
            Vector3 direction = (player.position - jumpscareObject.transform.position).normalized;
            jumpscareObject.transform.position += direction * speed * Time.deltaTime;

            jumpscareObject.transform.LookAt(player);

            float distance = Vector3.Distance(jumpscareObject.transform.position, player.position);

            if (distance <= stopDistance)
            {
                jumpscareObject.SetActive(false);
                yield break; // stop the coroutine
            }

            gameObject.SetActive(false);
            yield return null;
        }
    }


}
