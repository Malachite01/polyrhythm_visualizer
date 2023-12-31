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
    [SerializeField] private GameObject _sinePrefab;
    private SineObject _sineObject;
    private float _elapsedTime = 0f;
    private float _nextImpactTime;
    [NonSerialized]public AudioClip sfx;
    private AudioSource _audioSource;


    private void Awake() {
        _sinePrefab = Instantiate(_sinePrefab, transform.position, Quaternion.identity);
    }

    private void Start() {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = resolution;
        PlotArc();
        _ballPrefab = Instantiate(_ballPrefab, arcPoints[0], Quaternion.identity);
        _sineObject = _sinePrefab.GetComponent<SineObject>();
        _audioSource = GetComponent<AudioSource>();
        _nextImpactTime = CalculateNextImpactTime(_elapsedTime, 2*Mathf.PI * numberOfLoops / timeTofullCycle);
    }

    private void Update() {
        MoveBallAlongArc();
    }

    private void MoveBallAlongArc() {
        _elapsedTime += Time.deltaTime;
        if(_elapsedTime >= _nextImpactTime) {
            GenerateImpact();
           _nextImpactTime = CalculateNextImpactTime(_nextImpactTime, _velocity);
        }
        // Vector2 to store the next position of the ball
        Vector2 ballPosition = _ballPrefab.transform.position;

        float oneFullLoop = 2*Mathf.PI;
        _velocity = oneFullLoop * numberOfLoops / timeTofullCycle;
        float maxAngle = 2*Mathf.PI;
        float distance = Mathf.PI + (_elapsedTime * _velocity);
        float modDistance = distance % maxAngle;      
        float adjustedDistance = modDistance >= Mathf.PI ? modDistance : maxAngle - modDistance;

        // Move the ball along the arc
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
        _audioSource.clip = sfx;
        _audioSource.Play();
    }
}
