using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWave : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    public int points;
    public float amplitude = 1; 
    public float frequency = 1; 
    public Vector2 xLimits = new Vector2(0, 1);
    public float movementSpeed = 1;

    private void Start() {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Draw() {
        float xStart = xLimits.x;
        float Tau = 2* Mathf.PI;
        float xFinish = xLimits.y;

        _lineRenderer.positionCount = points;
        for(int currentPoint = 0; currentPoint < points;currentPoint++) {
            float progress = (float)currentPoint / (points - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);
            float y = amplitude*Mathf.Sin((Tau*frequency*x)+(Time.timeSinceLevelLoad*movementSpeed))-4.2f;
            _lineRenderer.SetPosition(currentPoint, new Vector3(x, y, 0));
        }
    }

    private void Update() {
        Draw();
    }
}
