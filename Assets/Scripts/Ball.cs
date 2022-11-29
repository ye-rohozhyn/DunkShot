using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class Ball : MonoBehaviour
{
	private Rigidbody2D ballRigidbody;
	private bool _readyToPush, _collisionObstacle, _rebound, _hitCurrentHoop;
	[SerializeField] private GameObject _currentHoop;

	[HideInInspector] public Vector3 BallPosition { get { return transform.position; } }
	public bool ReadyToPush { get => _readyToPush; }
	public bool CollisionObstacle { get => _collisionObstacle; }
	public bool Rebound { get => _rebound; }
	public bool HitCurrentHoop { get => _hitCurrentHoop; }

	private void Awake()
	{
		ballRigidbody = GetComponent<Rigidbody2D>();
	}

	public void Push(Vector2 force)
	{
		if (!_readyToPush) return;

		ballRigidbody.AddForce(force, ForceMode2D.Impulse);
		_readyToPush = false;
		_collisionObstacle = false;
		_rebound = false;
	}

	public void UnfreezeBall()
	{
		ballRigidbody.constraints = RigidbodyConstraints2D.None;
		ballRigidbody.freezeRotation = false;
	}

	public void FreezeBall()
	{
		ballRigidbody.velocity = Vector3.zero;
		ballRigidbody.angularVelocity = 0f;
		ballRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
		ballRigidbody.freezeRotation = true;
	}

	private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("LoseZone"))
        {
			Debug.Log("Lose");
        }

		if (_readyToPush) return;

        if (collision.CompareTag("BallGettingZone"))
        {
			_hitCurrentHoop = _currentHoop.Equals(collision.gameObject);
			if (!_hitCurrentHoop) _currentHoop = collision.gameObject;

			_readyToPush = true;
		}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
			_collisionObstacle = true;
		}
		else if (collision.gameObject.CompareTag("Wall"))
        {
			_rebound = true;
		}
	}
}
