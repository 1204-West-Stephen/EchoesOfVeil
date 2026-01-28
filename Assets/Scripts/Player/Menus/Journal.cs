using UnityEngine;

public class Journal : MonoBehaviour, i_Interactable
{
    public void Interact()
    {
        PlayerControls player = FindFirstObjectByType<PlayerControls>();
        player.AcquireJournal();
        Destroy(gameObject);
    }

    public InputType GetRequiredInputType()
    {
        return InputType.None;
    }
}
