using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 10f;
    public float attackRange = 5f;
    public float chargeDuration = 2f;

    [Header("Tiempo aleatorio entre arremetidas")]
    public float minWaitTime = 1f;
    public float maxWaitTime = 4f;

    private NavMeshAgent agent;
    private bool inRange = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            GameObject pObj = GameObject.FindGameObjectWithTag("Player");
            if (pObj != null)
                player = pObj.transform;
            else
                Debug.LogError(" No se encontró un objeto con el tag 'Player'");
        }

        StartCoroutine(ChaseCycle());
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        inRange = distance <= chaseRange;
    }

    IEnumerator ChaseCycle()
    {
        while (true)
        {
            if (inRange)
            {
                //Guardar la posición del jugador al comenzar la arremetida
                Vector3 targetPosition = player.position;

                agent.isStopped = false;
                agent.SetDestination(targetPosition);

                float timer = 0f;

                // Correr durante X segundos hacia esa posición, sin actualizarla
                while (timer < chargeDuration && agent.remainingDistance > agent.stoppingDistance)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }

                // Parar después de cargar
                agent.ResetPath();
                agent.isStopped = true;

                // Esperar antes de próxima arremetida
                float waitTime = Random.Range(minWaitTime, maxWaitTime);
                yield return new WaitForSeconds(waitTime);
            }
            else
            {
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}

