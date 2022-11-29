using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class Ball : MonoBehaviour
{
	private Rigidbody2D ballRigidbody;
	[HideInInspector] public Vector3 BallPosition { get { return transform.position; } }
	[SerializeField] private bool _readyToPush = true;
	[SerializeField] private bool _collisionObstacle = false;
	[SerializeField] private bool _rebound = false;

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

	public void EnablePhysics()
	{
		ballRigidbody.isKinematic = false;
	}

	public void DisablePhysics()
	{
		ballRigidbody.velocity = Vector3.zero;
		ballRigidbody.angularVelocity = 0f;
		ballRigidbody.isKinematic = true;
	}

	public void GetBallGettingInfo(ref bool readyToPush, ref bool collisionObstacle, ref bool rebound)
    {
		readyToPush = _readyToPush;
		collisionObstacle = _collisionObstacle;
		rebound = _rebound;
	}

	private void OnTriggerStay2D(Collider2D collision)
    {
		if (_readyToPush) return;

        if (collision.CompareTag("BallGettingZone"))
        {
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
