using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 200f;
    public float chargeDuration = 1f;

    [Header("Tiempo aleatorio entre arremetidas")]
    public float minWaitTime = 1f;
    public float maxWaitTime = 2f;

    private NavMeshAgent agent;
    private bool inRange = false;
    public bool isPaused = false;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            GameObject pObj = GameObject.FindGameObjectWithTag("Player");
            if (pObj != null)
                player = pObj.transform;
            else
                Debug.LogError("No se encontró un objeto con el tag 'Player'");
        }

        StartCoroutine(ChaseCycle());
    }

    private void Update()
    {
        if (player == null) return;
        if (isPaused) return;
        // Siempre mira al Player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // Para no inclinarse verticalmente
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        // Comprobar si está en rango para atacar
        float distance = Vector3.Distance(transform.position, player.position);
        inRange = distance <= chaseRange;
    }

    IEnumerator ChaseCycle()
    {
        while (true)
        {
            // Esperar si el enemigo está en pausa
            if (isPaused)
            {
                agent.isStopped = true;
                agent.ResetPath();
                yield return new WaitWhile(() => isPaused); // Espera hasta que se despausa
            }

            if (inRange)
            {
                // Guardar la posición actual del jugador
                Vector3 targetPosition = player.position;

                agent.isStopped = false;
                agent.SetDestination(targetPosition);

                float timer = 0f;
                while (timer < chargeDuration)
                {
                    if (isPaused) break; // Cortar la arremetida si se pausa
                    timer += Time.deltaTime;
                    yield return null;
                }

                agent.ResetPath();
                agent.isStopped = true;

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


