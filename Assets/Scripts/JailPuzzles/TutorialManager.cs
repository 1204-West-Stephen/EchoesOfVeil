using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class TutorialManager : MonoBehaviour
{
    private PlayerControls controls;
    public GameObject journalUI;
    public GameObject skipCanvas;

    public GameObject keysCanvas;
    public GameObject jControlUI;
    
    private CanvasGroup wasdCG;
    public CanvasGroup jCG;


    void Start()
    {
        controls = FindFirstObjectByType<PlayerControls>();
        Cursor.lockState = CursorLockMode.Confined;
        controls.DisableMovementOnly();
        Cursor.lockState = CursorLockMode.Confined;

        wasdCG = keysCanvas.GetComponent<CanvasGroup>();
        jCG = jControlUI.GetComponent<CanvasGroup>();

        journalUI.SetActive(false);
        skipCanvas.SetActive(true);
        keysCanvas.SetActive(false);
        jControlUI.SetActive(false);

        StartCoroutine(Tutorial());
    }

    private IEnumerator Tutorial()
    {
        Cursor.lockState = CursorLockMode.Confined;
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

        JournalTutorialUI();
        StartCoroutine(ShowKeyUI(keysCanvas, wasdCG));
        controls.EnableControls();
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void JournalTutorialUI()
    {
        journalUI.SetActive(true);
        skipCanvas.SetActive(false);
    }

    public IEnumerator ShowKeyUI(GameObject keyControlUI, CanvasGroup cg)
    {
        keyControlUI.SetActive(true);

        cg.alpha = 0f;

        float duration = 1f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, t / duration);
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1f, 0f, t / duration);
            yield return null;
        }

        cg.alpha = 0f;
        keyControlUI.SetActive(false);
    }
}
