using System.Collections;
using UnityEngine;

public class ArcSpawner : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    public float initialHalfCircleRadius = 5f;
    public float lineWeight = 0.05f;
    public int resolution = 100;
    private Vector3[] arcPoints;
    [SerializeField] private GameObject _ballPrefab;
    public float movementSpeed = 1;
    private float direction = 1;
    public int currentArcPointIndex = 0;
    [SerializeField] private SineObject _sineWave;

    private void Start() {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = resolution;
        PlotArc();
        _ballPrefab = Instantiate(_ballPrefab, arcPoints[0], Quaternion.identity);
        _sineWave = _sineWave.GetComponent<SineObject>();
    }

    private void Update() {
        MoveBallAlongArc();
    }

    private void MoveBallAlongArc() {
        if (direction == 1) {
            if (currentArcPointIndex < arcPoints.Length) {
                Vector3 currentPoint = arcPoints[currentArcPointIndex];

                _ballPrefab.transform.position = Vector3.MoveTowards(_ballPrefab.transform.position, currentPoint, movementSpeed * Time.fixedDeltaTime);

                // Check if the _ballPrefab is close to the currentPoint
                if (Vector3.Distance(_ballPrefab.transform.position, currentPoint) < 0.001f) {
                    currentArcPointIndex++;
                }
            }
            if(currentArcPointIndex == arcPoints.Length) {
                direction = -1;
                GenerateImpact();
            }
        } else {
            if (currentArcPointIndex >= 0) {
                Vector3 currentPoint = arcPoints[currentArcPointIndex-1];

                _ballPrefab.transform.position = Vector3.MoveTowards(_ballPrefab.transform.position, currentPoint, movementSpeed * Time.fixedDeltaTime);

                // Check if the _ballPrefab is close to the currentPoint
                if (Vector3.Distance(_ballPrefab.transform.position, currentPoint) < 0.001f) {
                    currentArcPointIndex--;
                }
            }
            if(currentArcPointIndex == 0) {
                direction = 1;
                GenerateImpact();
            }
        }
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
        _sineWave.StartBounce();
    }
}
