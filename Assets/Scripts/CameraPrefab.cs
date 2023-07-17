using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPrefab : MonoBehaviour
{
    [SerializeField] private Transform target;

    private float zOffset = -10f;

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    public void SetTarget(Vector3 targetPosition)
    {
        target = null;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, target.position.y, zOffset);
        }
    }
}
