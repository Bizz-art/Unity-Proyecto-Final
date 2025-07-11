using UnityEngine;
using System.Collections;

public class BPM : MonoBehaviour
{
    [Header("BPM Settings")]
    public float bpm = 120f;
    private float beatInterval;

    [Header("Visual Pulse")]
    public Transform beatIndicator; // Asigna una imagen (UI) o GameObject con Transform
    public Vector3 minScale = new Vector3(0.5f, 0.5f, 0.5f);
    public Vector3 maxScale = Vector3.one;
    public float scaleDuration = 0.2f;

    [Header("Beat Window")]
    public float beatWindow = 0.20f;
    public float lastBeatTime;

    void Start()
    {
        beatInterval = 60f / bpm;
        //StartCoroutine(BeatLoop());
    }
    private void OnEnable()
    {
        StartCoroutine(BeatLoop());
    }
    IEnumerator BeatLoop()
    {
        while (true)
        {
            OnBeat(); // Marca el latido
            yield return new WaitForSeconds(beatInterval);
        }
    }

    void OnBeat()
    {
        Debug.Log("¡Beat!");
        //lastBeatTime = Time.time;

        if (beatIndicator != null)
        {
            StopCoroutine(nameof(PulseIndicator));
            StartCoroutine(PulseIndicator());
        }
    }

    IEnumerator PulseIndicator()
    {
        float t = 0f;
        beatIndicator.localScale = minScale;

        while (t < scaleDuration)
        {
            beatIndicator.localScale = Vector3.Lerp(minScale, maxScale, t / scaleDuration);
            t += Time.deltaTime;
            yield return null;
        }

        beatIndicator.localScale = maxScale;
        // Se alcanza el punto máximo → aquí se marca el beat efectivo
        lastBeatTime = Time.time;
        Debug.Log("✔ Beat efectivo registrado en máxima escala");
    }
}
