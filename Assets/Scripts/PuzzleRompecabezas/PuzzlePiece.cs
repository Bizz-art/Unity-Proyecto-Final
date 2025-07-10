using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public Vector2Int gridPosition;

    public void MoveToPosition(Vector3 targetPos)
    {
        transform.position = targetPos;
    }
}