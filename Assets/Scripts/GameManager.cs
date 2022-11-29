using TMPro;
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
	[SerializeField] private LevelGenerator levelGenerator;
	[SerializeField] private TMP_Text scoreText;
	[SerializeField] private float pushForce = 4f;
	[SerializeField] private float maxLength = 13f;

	private Camera _mainCamera;
	private bool _isDragging = false, _ballIsGetting = false;
	private Vector2 _startPoint, _endPoint, _direction, _force;
	private float _distance;
	private int _combo = 0, _pushCount, _score = 0;

	private void Start()
	{
		_mainCamera = Camera.main;
	}

	private void Update()
	{
		if (!ball.ReadyToPush)
		{
			return;
		}
		else if(!_ballIsGetting)
		{
			_ballIsGetting = true;

			if (ball.HitCurrentHoop || _pushCount == 0) return;

			levelGenerator.ActivateNextHoop(ball.BallPosition);

			if (ball.CollisionObstacle)
            {
				Debug.Log("+1");
				_score += 1;
				_combo = 0;
			}
            else
            {
                if (ball.Rebound)
                {
					Debug.Log("Rebound! +1");
					_score += 1;
				}

				if (_combo < 3) _combo++;
				Debug.Log($"Perfect! +{_combo * 2}");
				_score += _combo * 2;
			}

			scoreText.text = $"{_score}";
		}

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
			_ballIsGetting = false;
			_pushCount++;
			OnDragExit();
		}
	}

	#region - Drag -

	private void OnDragEnter()
	{
		ball.FreezeBall();
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
		ball.UnfreezeBall();
		ball.Push(_force);
		trajectory.Hide();
	}

    #endregion
}