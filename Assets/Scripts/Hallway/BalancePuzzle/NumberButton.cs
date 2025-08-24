using UnityEngine;
using UnityEngine.UI;

public class NumberButton : MonoBehaviour
{
    public int value;

    private Button button;
    private BalancePuzzle puzzle;

    private void Awake()
    {
        button = GetComponent<Button>();
        puzzle = FindObjectOfType<BalancePuzzle>();

        if (button != null)
            button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if (puzzle != null)
            puzzle.SelectNumber(this);
    }

    public void DisableButton()
    {
        if (button == null) return;

        button.interactable = false;
        Highlight(false);
    }

    public void Highlight(bool isSelected)
    {
        if (button == null || button.targetGraphic == null) return;

        button.targetGraphic.color = isSelected ? Color.yellow : Color.white;
    }

    public void ResetButton()
    {
        if (button == null) return;

        button.interactable = true;

        // Reset color after interactable to avoid Unity overriding
        if (button.targetGraphic != null)
            button.targetGraphic.color = Color.white;
    }


    public bool IsInteractable()
    {
        return button != null && button.interactable;
    }
}
