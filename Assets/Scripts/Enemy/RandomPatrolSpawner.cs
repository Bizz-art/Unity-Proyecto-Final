using UnityEngine;
using System.Collections;

public class RandomPatrolSpawner : MonoBehaviour
{
    [Header("Área de patrulla")]
    public Vector3 areaCenter;
    public Vector3 areaSize = new Vector3(4f, 0.81f, 4f);

    [Header("Movimiento")]
    public float moveSpeed = 2f;
    public float waitTime = 2f;

    private Vector3 targetPosition;

    void Start()
    {
        StartCoroutine(MoveRoutine());
    }

    void Update()
    {
        // Movimiento hacia el objetivo
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    IEnumerator MoveRoutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => Vector3.Distance(transform.position, targetPosition) < 0.1f);

            yield return new WaitForSeconds(waitTime);

            targetPosition = GetRandomPosition();
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 randomOffset = new Vector3(
      Random.Range(-areaSize.x / 2, areaSize.x / 2),
      0.81f,
      Random.Range(-areaSize.z / 2, areaSize.z / 2)
      );

        Vector3 newPos = areaCenter + randomOffset;
        newPos.y = 0.81f; // Mantiene la altura actual
        return newPos;
    }

    // Visualiza el área en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}
