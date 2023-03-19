using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class JunkPoint : MonoBehaviour
{
    public bool IsFilled { get; private set; }
    public bool IsScheduled { get; private set; }

    public event UnityAction<JunkPoint> Released;

    private Junk _junk;

    public void RemoveSpawnedJunk()
    {
        if (IsFilled == false)
        {
            Debug.LogError("Junk point is't filled");
            return;
        }

        _junk.Remove();
        IsFilled = false;
        _junk.JunkCollected -= OnJunkCollected;
    }

    public void SpawnScheduledJunk()
    {
        if (IsScheduled == false)
        {
            Debug.LogError("Junk is not scheduled!");
            return;
        }

        _junk.Activate();
        _junk.JunkCollected += OnJunkCollected;
        IsScheduled = false;
        IsFilled = true;
    }

    public Junk ScheduleSpawn(Junk junk)
    {
        if (IsFilled)
        {
            Debug.LogError("Junk point is already filled");
            return null;
        }

        if (IsScheduled)
        {
            Debug.LogError("Junk point is already scheduled");
            return null;
        }        

        Junk spawnedJunk = Instantiate(junk, transform);
        spawnedJunk.transform.position = transform.position;
        spawnedJunk.transform.rotation = Random.rotation;
        spawnedJunk.Deactivate();
        _junk = spawnedJunk;
        IsScheduled = true;

        return spawnedJunk;
    }

    public IReadOnlyList<Vector3> GetScheduledJunkBoundingBoxCornersPoints()
    {
        if (IsScheduled == false)
        {
            Debug.LogError("Junk is not scheduled!");
            return null;
        }

        Bounds checkingJunkPointsBounds = _junk.AttachedCollider.bounds;
        Vector3[] checkingPoints = new Vector3[8];
        Vector3 colliderSize = Vector3.Scale(_junk.AttachedCollider.size, _junk.transform.lossyScale);
        Vector3 colliderMinPoint = _junk.transform.localToWorldMatrix.MultiplyPoint(_junk.transform.localPosition - _junk.AttachedCollider.size);
        checkingPoints[0] = colliderMinPoint;
        checkingPoints[1] = colliderMinPoint + new Vector3(colliderSize.x, 0, 0);
        checkingPoints[2] = colliderMinPoint + new Vector3(0, colliderSize.y, 0);
        checkingPoints[3] = colliderMinPoint + new Vector3(colliderSize.x, colliderSize.y, 0);
        checkingPoints[4] = colliderMinPoint + new Vector3(0, 0, colliderSize.z);
        checkingPoints[5] = colliderMinPoint + new Vector3(colliderSize.x, 0, colliderSize.z);
        checkingPoints[6] = colliderMinPoint + new Vector3(0, colliderSize.y, colliderSize.z);
        checkingPoints[7] = _junk.transform.localToWorldMatrix.MultiplyPoint(_junk.transform.localPosition + _junk.AttachedCollider.size);

        return checkingPoints.ToList();
    }

    public int GetJunkLevel()
    {
        if (_junk == null)
            Debug.LogError("Junk is null!");

        return _junk.Level;
    }

    public void RemoveScheduleSpawn()
    {
        if (IsScheduled == false)
        {
            Debug.LogError("Junk is not scheduled!");
            return;
        }

        _junk.Remove();
        IsScheduled = false;
    }

    private void OnJunkCollected(Junk collectedJunk)
    {
        collectedJunk.JunkCollected -= OnJunkCollected;
        IsFilled = false;
        Released?.Invoke(this);
    }
}
