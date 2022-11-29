using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform[] hoops;
    [SerializeField] private float minYOffset = 2f;
    [SerializeField] private float maxYOffset = 4f;
    [SerializeField] private float minRotation = 15f;
    [SerializeField] private float maxRotation = 35f;
    [SerializeField] private float leftPos = -1.2f;
    [SerializeField] private float rightPos = 1.2f;
    private float _yOffset;
    private int _currentHoop = 0;

    public void ActivateNextHoop(Vector2 ballPosition)
    {
        _yOffset = Random.Range(minYOffset, maxYOffset);
        ballPosition.y += _yOffset;
        ballPosition.x = ballPosition.x < 0 ? rightPos : leftPos;
        Vector3 rotation = Vector3.zero;
        if (Random.Range(1, 101) <= 25)
        {
            if (ballPosition.x == rightPos) rotation.z = Random.Range(minRotation, maxRotation);
            else rotation.z = Random.Range(-minRotation, -maxRotation);
        }

        hoops[_currentHoop].position = ballPosition;
        hoops[_currentHoop].Rotate(rotation);

        if (_currentHoop == hoops.Length - 1) _currentHoop = 0;
        else _currentHoop++;

        hoops[_currentHoop].rotation = Quaternion.identity;
    }
}