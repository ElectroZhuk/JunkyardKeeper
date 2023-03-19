using UnityEngine;
using System.Linq;

public class JunkPointsSpawner : MonoBehaviour
{
    [Header("Junk points spawn configuration")]
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private JunkPointsLayer _layerTemplate;
    [SerializeField] private JunkPoint _junkPointTemplate;
    [SerializeField] private bool _needDrawGizmos;
    [SerializeField] [Min(0.1f)] private float _horisontalSpawnDistance;
    [SerializeField] [Min(0.1f)] private float _verticalSpawnDistance;

    private void OnDrawGizmos()
    {
        if (_needDrawGizmos == false)
            return;

        Gizmos.color = Color.red;

        foreach (JunkPoint junkPoint in GetComponentsInChildren<JunkPoint>())
            Gizmos.DrawSphere(junkPoint.transform.position, 0.1f);
    }

    [ExecuteAlways]
    [ContextMenu("Spawn junk points")]
    private void SpawnJunkFromCenter()
    {
        ClearJunk();
        _collider.enabled = true;
        Vector3 startPoint = transform.localToWorldMatrix.MultiplyPoint(_collider.center);
        Vector3 zSpawnDirection = (transform.localToWorldMatrix.MultiplyPoint(_collider.center + new Vector3(0, 0, _collider.radius)) - startPoint).normalized;
        Vector3 xSpawnDirection = (transform.localToWorldMatrix.MultiplyPoint(_collider.center + new Vector3(_collider.height / 2, 0, 0)) - startPoint).normalized;
        Vector3 ySpawnDirection = (transform.localToWorldMatrix.MultiplyPoint(_collider.center + new Vector3(0, _collider.radius, 0)) - startPoint).normalized;
        Vector3 currentPoint = startPoint;
        int verticalIterations = 0;

        while (_collider.bounds.Contains(currentPoint))
        {
            Vector3 localYStartPosition = currentPoint;
            JunkPointsLayer currentLayer = Instantiate(_layerTemplate, transform);
            currentLayer.name = $"Layer {verticalIterations}";

            while (_collider.bounds.Contains(currentPoint))
            {
                Vector3 localXStartPosition = currentPoint;

                while (_collider.bounds.Contains(currentPoint))
                {
                    if (Physics.OverlapSphere(currentPoint, 0.01f).Contains(_collider))
                    {
                        JunkPoint spawnedJunk = Instantiate(_junkPointTemplate, currentLayer.transform);
                        spawnedJunk.transform.position = currentPoint;
                    }

                    currentPoint += _horisontalSpawnDistance * zSpawnDirection;
                }

                currentPoint = localXStartPosition - _horisontalSpawnDistance * zSpawnDirection;

                while (_collider.bounds.Contains(currentPoint))
                {
                    if (Physics.OverlapSphere(currentPoint, 0.01f).Contains(_collider))
                    {
                        JunkPoint spawnedJunk = Instantiate(_junkPointTemplate, currentLayer.transform);
                        spawnedJunk.transform.position = currentPoint;
                    }

                    currentPoint -= _horisontalSpawnDistance * zSpawnDirection;
                }

                currentPoint = localXStartPosition + xSpawnDirection * _horisontalSpawnDistance;
            }

            currentPoint = localYStartPosition;

            while (_collider.bounds.Contains(currentPoint))
            {
                Vector3 localXStartPosition = currentPoint;

                while (_collider.bounds.Contains(currentPoint))
                {
                    if (Physics.OverlapSphere(currentPoint, 0.01f).Contains(_collider))
                    {
                        JunkPoint spawnedJunk = Instantiate(_junkPointTemplate, currentLayer.transform);
                        spawnedJunk.transform.position = currentPoint;
                    }

                    currentPoint += _horisontalSpawnDistance * zSpawnDirection;
                }

                currentPoint = localXStartPosition - _horisontalSpawnDistance * zSpawnDirection;

                while (_collider.bounds.Contains(currentPoint))
                {
                    if (Physics.OverlapSphere(currentPoint, 0.01f).Contains(_collider))
                    {
                        JunkPoint spawnedJunk = Instantiate(_junkPointTemplate, currentLayer.transform);
                        spawnedJunk.transform.position = currentPoint;
                    }

                    currentPoint -= _horisontalSpawnDistance * zSpawnDirection;
                }

                currentPoint = localXStartPosition - xSpawnDirection * _horisontalSpawnDistance;
            }

            currentPoint = localYStartPosition + ySpawnDirection * _verticalSpawnDistance;
            verticalIterations++;
        }

        JunkPoint[] junkPoints = GetComponentsInChildren<JunkPoint>();

        _collider.enabled = false;
    }

    [ExecuteAlways]
    [ContextMenu("Clear junk points")]
    private void ClearJunk()
    {
        foreach (JunkPointsLayer junkLayer in GetComponentsInChildren<JunkPointsLayer>())
        {
            DestroyImmediate(junkLayer.gameObject);
        }
    }
}
