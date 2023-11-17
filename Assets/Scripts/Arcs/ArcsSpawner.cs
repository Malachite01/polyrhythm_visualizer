using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _arcPrefab;
    private Arc _arc;

    private List<GameObject> _arcs = new List<GameObject>();
    private float _radius;
    public int numberOfArcs = 21;
    public float arcRadiusIncrement = 1f;
    private int _numberOfLoops;

    private void Start() {
        _arc = _arcPrefab.GetComponent<Arc>();
        _radius = _arc.initialHalfCircleRadius;
        _numberOfLoops = _arc.numberOfLoops;
        for (int i = 0; i < numberOfArcs; i++) {
            SpawnArc();
        }
    }

    private void SpawnArc() {
        GameObject arc = Instantiate(_arcPrefab, transform.position, Quaternion.identity);
        arc.GetComponent<Arc>().initialHalfCircleRadius = _radius * arcRadiusIncrement * (_arcs.Count + 1);
        arc.GetComponent<Arc>().numberOfLoops = _numberOfLoops;
        _numberOfLoops -= 1;
        _arcs.Add(arc);
    }
}
