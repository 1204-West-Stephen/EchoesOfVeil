using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private PlayerControls controls;
    public GameObject journalUI;


    void Start()
    {
        controls = FindFirstObjectByType<PlayerControls>();
        controls.DisableMovementOnly();

        journalUI.SetActive(false);

        StartCoroutine(Tutorial());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(2f);

        controls.StartDialogue("Where... where am I?");
        controls.StartDialogue("The last thing I remember... fighting broke out");
        controls.StartDialogue("The Barbarians from the West and the Mages from the East...");
        controls.StartDialogue("They... they're trying to destroy the Pyramid.");
        controls.StartDialogue("I need to figure out where I am.");
        controls.StartDialogue("It looks like there's a journal of some kind on the table.");
        controls.StartDialogue("I should start there.");

        // WAIT until dialogue finishes
        yield return new WaitUntil(() => !controls.IsDialogueRunning);

        JournalTutorialUI();          // NOW this will actually happen at the right time
        controls.EnableMovementOnly();
    }

    private void JournalTutorialUI()
    {
        journalUI.SetActive(true);
    }
}
