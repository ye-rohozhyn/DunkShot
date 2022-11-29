using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool smoothMotion;
    [SerializeField] private float yOffset = 1.25f;
    [SerializeField] private float maxFallDistance = -6f;
    
    private float _smoothSpeed = 5f;
    private Vector3 _targetPosition;
    [SerializeField] private float _maxYPosition = 0f;
    [SerializeField] private float _fallDistance = 0f;

    private void FixedUpdate()
    {
        if (_fallDistance <= maxFallDistance) return;

        CalcTargetPosition();

        if (smoothMotion) 
        { 
            transform.position = Vector3.Lerp(transform.position, _targetPosition, 
                Time.fixedDeltaTime * _smoothSpeed);
        }
        else
        {
            transform.position = _targetPosition;
        }

        if (transform.position.y > _maxYPosition)
        {
            _maxYPosition = transform.position.y;
            _fallDistance = 0;
        }
        else
        {
            _fallDistance = transform.position.y - _maxYPosition;
        } 
    }

    private void CalcTargetPosition()
    {
        _targetPosition = transform.position;
        _targetPosition.y = target.position.y + yOffset;
    }
}
