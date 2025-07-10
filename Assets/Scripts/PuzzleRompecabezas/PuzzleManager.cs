using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public List<Transform> tiles = new List<Transform>(); // Asigna los cubos en el Inspector
    public Transform emptySpot; // El espacio vacío
    public float moveDuration = 0.2f;

    public void TryMoveTile(Transform tile)
    {
        Vector3 localTilePos = tile.localPosition;
        Vector3 localEmptyPos = emptySpot.localPosition;

        if (Vector3.Distance(localTilePos, localEmptyPos) < 1.1f)
        {
            StartCoroutine(MoveTile(tile, localEmptyPos));
            emptySpot.localPosition = localTilePos;
        }
    }

    IEnumerator MoveTile(Transform tile, Vector3 targetLocalPos)
    {
        float elapsed = 0f;
        Vector3 start = tile.localPosition;

        while (elapsed < moveDuration)
        {
            tile.localPosition = Vector3.Lerp(start, targetLocalPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        tile.localPosition = targetLocalPos;
    }
}
