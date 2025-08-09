using UnityEngine;

public class NinoController : MonoBehaviour
{
    public Animator animator; // Debe tener parámetro int "estado"
    public EstadoNino estadoActual = EstadoNino.Playing;

    public void SetEstado(EstadoNino nuevoEstado)
    {
        estadoActual = nuevoEstado;
        if (animator != null)
        {
            animator.SetInteger("estado", (int)estadoActual);
            Debug.Log($"Estado cambiado a: {estadoActual} ({(int)estadoActual})");
        }
    }
}