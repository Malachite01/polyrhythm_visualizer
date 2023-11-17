using System;
using System.Collections;
using UnityEngine;

public class Arc : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    public float initialHalfCircleRadius = 5f;
    public float lineWeight = 0.05f;
    public int resolution = 100;
    private Vector3[] arcPoints;
    [SerializeField] private GameObject _ballPrefab;
    private float _velocity;
    public int numberOfLoops = 50;
    public int timeTofullCycle = 900;
    [SerializeField] private GameObject _sineWave;
    private SineObject _sineObject;
    private float _elapsedTime = 0f;
    private float _nextImpactTime;
    private bool _allowImpact = true;


    private void Start() {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = resolution;
        PlotArc();
        _ballPrefab = Instantiate(_ballPrefab, arcPoints[0], Quaternion.identity);
        _sineObject = _sineWave.GetComponent<SineObject>();
    }

    private void Update() {
        MoveBallAlongArc();
    }

    private void MoveBallAlongArc() {
        _elapsedTime += Time.deltaTime;
        Vector2 ballPosition = _ballPrefab.transform.position;

        float oneFullLoop = 2*Mathf.PI;
        _velocity = oneFullLoop * numberOfLoops / timeTofullCycle;
        float maxAngle = 2*Mathf.PI;
        float distance = Mathf.PI + (_elapsedTime * _velocity);
        float modDistance = distance % maxAngle;      
        float adjustedDistance = modDistance >= Mathf.PI ? modDistance : maxAngle - modDistance;

        if(_allowImpact) {
           _nextImpactTime = CalculateNextImpactTime(_elapsedTime, _velocity);
           _allowImpact = false;
        }
        if(_nextImpactTime < _elapsedTime) {
            GenerateImpact();
        }

        ballPosition.x = transform.position.x + (initialHalfCircleRadius + lineWeight) * Mathf.Cos(adjustedDistance); 
        ballPosition.y = transform.position.y - (initialHalfCircleRadius + lineWeight) * Mathf.Sin(adjustedDistance); 
       _ballPrefab.transform.position = ballPosition;
    }

    private float CalculateNextImpactTime(float currentImpactTime, float _velocity) {
        return currentImpactTime + (Mathf.PI / _velocity);
    }

    private Vector3[] GenerateArcPoints() {
        Vector3[] points = new Vector3[resolution];

        for (int i = 0; i < resolution; i++) {
            float t = i / (float)(resolution - 1);
            float angle = Mathf.Lerp(Mathf.PI, 2 * Mathf.PI, t);

            float x = transform.position.x + (initialHalfCircleRadius + lineWeight) * Mathf.Cos(angle);
            float y = transform.position.y - (initialHalfCircleRadius + lineWeight) * Mathf.Sin(angle);

            points[i] = new Vector3(x, y);
        }

        return points;
    }

    private void PlotArc() {
        arcPoints = GenerateArcPoints();
        _lineRenderer.SetPositions(arcPoints);
    }

    private void GenerateImpact() {
        _sineObject.StartBounce();
        _allowImpact = true;
    }
}
