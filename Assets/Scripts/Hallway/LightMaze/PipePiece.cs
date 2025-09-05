using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PipePiece : MonoBehaviour, i_Interactable
{
    [Tooltip("Openings in neutral prefab orientation (rotationSteps = 0).")]
    public List<Direction> baseOpenings;

    private int rotationSteps = 0; // 0=0°, 1=90°, 2=180°, 3=270°

    // Current openings in world space (after rotation)
    public List<Direction> CurrentOpenings
    {
        get
        {
            List<Direction> result = new List<Direction>();
            foreach (Direction d in baseOpenings)
                result.Add(DirectionHelper.RotateCW(d, rotationSteps));
            return result;
        }
    }

    // Called when player interacts
    public void Interact()
    {
        RotateClockwise();
    }

    private void RotateClockwise()
    {
        rotationSteps = (rotationSteps + 1) % 4;
        transform.Rotate(0, 0, -90, Space.Self); // 3D rotation around local Z axis
    }
}
