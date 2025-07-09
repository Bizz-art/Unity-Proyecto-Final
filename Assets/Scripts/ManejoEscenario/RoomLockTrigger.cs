using UnityEngine;

public class RoomLockTrigger : MonoBehaviour
{
    [Header("Paredes a bloquear")]
    public Collider[] paredes;

    [Header("Opciones de desactivaci�n")]
    public bool desactivarSpawnerDespues = true;
    public bool activarUnaSolaVez = true;

    private bool yaActivado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (yaActivado) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log(" Player entr� a la zona, activando bloqueo de salida.");

            foreach (var pared in paredes)
            {
                pared.isTrigger = false;
            }

            if (desactivarSpawnerDespues)
            {
                Debug.Log(" Desactivando spawner");
                gameObject.SetActive(false); // Desactiva el spawner (y todo su movimiento/l�gica)
            }

            if (activarUnaSolaVez)
                yaActivado = true;
        }
    }
}

