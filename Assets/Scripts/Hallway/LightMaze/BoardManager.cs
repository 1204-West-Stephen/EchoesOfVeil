using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Place a piece in the grid world position
    public void PlacePiece(PipePiece piece, Vector2Int pos)
    {
        piece.transform.position = new Vector3(pos.x, pos.y, 0);
    }
}
