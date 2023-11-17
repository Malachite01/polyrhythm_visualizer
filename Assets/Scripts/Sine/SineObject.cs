using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineObject : MonoBehaviour
{
    [SerializeField] private float _basicAmplitude = 0.01f;
    [SerializeField] private float _maxAmplitude = 20f;
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private SineWave _sineWave1;
    [SerializeField] private SineWave _sineWave2;
    private void Start() {
        _sineWave1 = _sineWave1.GetComponent<SineWave>();
        _sineWave2 = _sineWave2.GetComponent<SineWave>();
    }

    private void SetAmplitude(float amplitude) {
        _sineWave1.amplitude = amplitude;
        _sineWave2.amplitude = amplitude + 0.1f;
    }

    public void StartBounce() {
        StartCoroutine(Bounce());
    }

    public IEnumerator Bounce() {
        float elapsedTime = 0f;
        
        while (elapsedTime < duration) {
            float t = elapsedTime / duration;
            SetAmplitude(Mathf.Lerp(_maxAmplitude / 4, _basicAmplitude, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        yield return null;

        SetAmplitude(_basicAmplitude);
    }

}
