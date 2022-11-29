using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region - Singleton -

	public static GameManager Instance;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	#endregion

	[SerializeField] private Ball ball;
	[SerializeField] private Trajectory trajectory;
	[SerializeField] private float pushForce = 4f;
	[SerializeField] private float maxLength = 13f;

	private Camera _mainCamera;
	private bool _isDragging = false;
	private Vector2 _startPoint, _endPoint, _direction, _force;
	private float _distance;

	private void Start()
	{
		_mainCamera = Camera.main;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0)) //Enter drag
		{
			_isDragging = true;
			OnDragEnter();
		}

		if (_isDragging) //Drag
		{
			OnDrag();
		}

		if (Input.GetMouseButtonUp(0)) //Exit drag
		{
			_isDragging = false;
			OnDragExit();
		}
	}

	#region - Drag -

	private void OnDragEnter()
	{
		ball.DisablePhysics();
		_startPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

		trajectory.Show();
	}

	private void OnDrag()
	{
		_endPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
		_distance = Vector2.Distance(_startPoint, _endPoint);
		_direction = (_startPoint - _endPoint).normalized;
		_force = _distance * pushForce * _direction;
		_force = Vector2.ClampMagnitude(_force, maxLength);

		trajectory.UpdateDots(ball.BallPosition, _force);
	}

	private void OnDragExit()
	{
		ball.EnablePhysics();
		ball.Push(_force);
		trajectory.Hide();
	}

    #endregion
}