using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class Ball : MonoBehaviour
{
	private Rigidbody2D ballRigidbody;
	[HideInInspector] public Vector3 BallPosition { get { return transform.position; } }

	private void Awake()
	{
		ballRigidbody = GetComponent<Rigidbody2D>();
	}

	public void Push(Vector2 force)
	{
		ballRigidbody.AddForce(force, ForceMode2D.Impulse);
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
}
