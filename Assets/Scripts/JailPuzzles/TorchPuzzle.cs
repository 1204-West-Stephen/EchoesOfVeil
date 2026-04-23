using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class TorchPuzzle : MonoBehaviour, i_Interactable
{
    public ItemData item;
    public bool itemPickedUp;
    private AudioSource source;
    public AudioClip clip;

    public string HallwaySceneName = "Hallway";
    private bool isLoaded = false;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        itemPickedUp = false;
    }
    public void Interact()
    {
        if (!itemPickedUp) { 
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                Inventory inventory = player.GetComponent<Inventory>();
                if (inventory != null)
                {
                    if (inventory.CheckInventory())
                    {
                        if (clip != null)
                        {
                            AudioSource.PlayClipAtPoint(clip, transform.position, 0.05f);
                        }

                        inventory.AddItem(item);
                        itemPickedUp = true;
                        StartCoroutine(LoadHallway());
                    }
                }
                else
                {
                    Debug.LogWarning("Player has no Inventory component.");
                }
            }
        }
    }
    private IEnumerator LoadHallway()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(HallwaySceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        // Wait until loading is complete
        while (!asyncLoad.isDone)
        {
            // Unity considers scene ready when progress hits 0.9
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        isLoaded = true;
        gameObject.SetActive(false);
    }

    public InputType GetRequiredInputType()
    {
        return InputType.None;
    }
}
