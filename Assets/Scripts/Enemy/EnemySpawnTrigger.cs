using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("Configuración del Spawn")]
    public GameObject enemyPrefab;
    private int cantidad;
    public bool activarSoloUnaVez = true;

    private bool yaActivado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!yaActivado && other.CompareTag("Player"))
        {
            SpawnEnemigos();
            if (activarSoloUnaVez) yaActivado = true;
        }
    }

    void SpawnEnemigos()
    {
        cantidad = Random.Range(1, 3+1);
        for (int i = 0; i < cantidad; i++)
        {
            // Instanciar con un pequeño desplazamiento para evitar solapamiento
            Vector3 offset = new Vector3(Random.Range(-4f, 4f), 0, Random.Range(-4f, 4f));
            Vector3 spawnPosition = transform.position + offset;

            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }

        Debug.Log($" SpawnManager: {cantidad} enemigos instanciados en {transform.position}");
    }
}
