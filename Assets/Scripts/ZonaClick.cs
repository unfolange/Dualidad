using UnityEngine;
using System.Collections;

public class ZonaClick : MonoBehaviour
{
    public EstadoNino estadoAlClic = EstadoNino.Playing;
    public NinoController nino;
    [Tooltip("Retraso al soltar antes de volver a Idle")]
    public float graciaAlSoltar = 0.2f;

    bool presionadoDentro = false;
    Coroutine coGracia;

    void OnMouseDown()
    {
        presionadoDentro = true;
        if (coGracia != null) { StopCoroutine(coGracia); coGracia = null; }
        if (nino) nino.SetEstado(estadoAlClic);
    }

    void Update()
    {
        // Si empezamos el clic en esta zona, mantener el estado mientras el botón esté abajo
        if (presionadoDentro)
        {
            if (!Input.GetMouseButton(0))  // se soltó en cualquier parte de la pantalla
            {
                presionadoDentro = false;
                if (coGracia != null) StopCoroutine(coGracia);
                coGracia = StartCoroutine(VolverAIdleTrasGracia());
            }
            else
            {
                // (opcional) si quieres “refrescar” Playing por si el clip terminó:
                // if (estadoAlClic == EstadoNino.Playing && nino) nino.SetEstado(EstadoNino.Playing);
            }
        }
    }

    void OnMouseUp()
    {
        // Si sueltas encima, también vuelve a Idle con gracia
        if (presionadoDentro)
        {
            presionadoDentro = false;
            if (coGracia != null) StopCoroutine(coGracia);
            coGracia = StartCoroutine(VolverAIdleTrasGracia());
        }
    }

    IEnumerator VolverAIdleTrasGracia()
    {
        yield return new WaitForSeconds(graciaAlSoltar);
        if (nino) nino.SetEstado(EstadoNino.Idle);
        coGracia = null;
    }
}
