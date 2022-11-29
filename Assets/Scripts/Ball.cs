using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class Ball : MonoBehaviour
{
	private Rigidbody2D ballRigidbody;
	private bool _readyToPush, _collisionObstacle, _rebound, _hitCurrentHoop, _isFall;
	private Vector3 _startPosition;
	[SerializeField] private GameObject currentHoop;
	[SerializeField] private AudioClip hitObstacle;
	[SerializeField] private AudioClip ballGetting;
	[SerializeField] private AudioSource audioSource;

	[HideInInspector] public Vector3 BallPosition { get { return transform.position; } }
	public bool ReadyToPush { get => _readyToPush; }
	public bool CollisionObstacle { get => _collisionObstacle; }
	public bool Rebound { get => _rebound; }
	public bool HitCurrentHoop { get => _hitCurrentHoop; }
	public bool IsFall { get => _isFall; }

	private void Awake()
	{
		ballRigidbody = GetComponent<Rigidbody2D>();
		_startPosition = transform.position;
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
		ballRigidbody.angularVelocity = 0.01f;
	}

	public void FreezeBall()
	{
		ballRigidbody.velocity = Vector3.zero;
		ballRigidbody.angularVelocity = 0f;
		ballRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
		ballRigidbody.freezeRotation = true;
	}

	public void BallReset()
    {
		transform.position = _startPosition;
		ballRigidbody.velocity = Vector3.zero;
		ballRigidbody.angularVelocity = 0.01f;
		_readyToPush = false;
		_collisionObstacle = false;
		_rebound = false;
		_isFall = false;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.CompareTag("LoseZone"))
		{
			_isFall = true;
		}
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_readyToPush) return;

        if (collision.CompareTag("BallGettingZone"))
        {
			audioSource.PlayOneShot(ballGetting);
			_hitCurrentHoop = currentHoop.Equals(collision.gameObject);
			if (!_hitCurrentHoop) currentHoop = collision.gameObject;

			_readyToPush = true;
		}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.gameObject.CompareTag("Obstacle"))
        {
			audioSource.PlayOneShot(hitObstacle);
			_collisionObstacle = true;
		}
		else if (collision.gameObject.CompareTag("Wall"))
        {
			audioSource.PlayOneShot(hitObstacle);
			_rebound = true;
		}
	}
}
