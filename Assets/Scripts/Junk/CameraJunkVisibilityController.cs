using System.Collections.Generic;
using UnityEngine;

public class CameraJunkVisibilityController : MonoBehaviour
{
    [SerializeField] private Camera _targetCamera;

    private Vector3 _minVisibleViewportPosition = new Vector3(-0.01f, -0.01f, -0.01f);
    private Vector2 _maxVisibleViewportPosition = new Vector2(1.01f, 1.01f);

    public List<JunkPoint> GetJunkPointsWithNotVisibleJunk(List<JunkPoint> pointsToCheck)
    {
        List<JunkPoint> notVisible = new List<JunkPoint>();

        foreach (JunkPoint junkPoint in pointsToCheck)
        {
            bool isVisible = false;
            IReadOnlyList<Vector3> checkingPoints = junkPoint.GetScheduledJunkBoundingBoxCornersPoints();

            foreach (Vector3 point in checkingPoints)
            {
                Vector3 viewportPoint = _targetCamera.WorldToViewportPoint(point);

                bool isVisibleInAxisX = _minVisibleViewportPosition.x <= viewportPoint.x && viewportPoint.x <= _maxVisibleViewportPosition.x;
                bool isVisibleInAxisY = _minVisibleViewportPosition.y <= viewportPoint.y && viewportPoint.y <= _maxVisibleViewportPosition.y;
                bool isVisibleInAxisZ = -0.01f <= viewportPoint.z;

                if (isVisibleInAxisX && isVisibleInAxisY && isVisibleInAxisZ)
                    isVisible = true;
            }

            if (isVisible == false)
                notVisible.Add(junkPoint);
        }

        return notVisible;
    }
}
