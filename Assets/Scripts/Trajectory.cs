using UnityEngine;

public class Trajectory : MonoBehaviour
{
	[SerializeField] private int amountOfPoints;
	[SerializeField] private GameObject pointsParent;
	[SerializeField] private GameObject pointPrefab;
	[SerializeField] float pointSpacing;
	[SerializeField] [Range(0.01f, 0.2f)] float pointsMinScale;
	[SerializeField] [Range(0.2f, 0.5f)] float pointsMaxScale;

	private Transform[] _points;
	private Vector2 _pointPosition;
	private float _timeStamp;

	private void Start()
	{
		Hide();
		PrepareDots();
	}

	private void PrepareDots()
	{
		_points = new Transform[amountOfPoints];
		pointPrefab.transform.localScale = Vector3.one * pointsMaxScale;

		float scale = pointsMaxScale;
		float scaleFactor = scale / amountOfPoints;

		for (int i = 0; i < amountOfPoints; i++)
		{
			_points[i] = Instantiate(pointPrefab, null).transform;
			_points[i].parent = pointsParent.transform;

			_points[i].localScale = Vector3.one * scale;
			if (scale > pointsMinScale)
				scale -= scaleFactor;
		}
	}

	public void UpdateDots(Vector3 ballPos, Vector2 forceApplied)
	{
		_timeStamp = pointSpacing;
		for (int i = 0; i < amountOfPoints; i++)
		{
			_pointPosition.x = (ballPos.x + forceApplied.x * _timeStamp);
			_pointPosition.y = (ballPos.y + forceApplied.y * _timeStamp) - (Physics2D.gravity.magnitude * _timeStamp * _timeStamp) / 2f;

			_points[i].position = _pointPosition;
			_timeStamp += pointSpacing;
		}
	}

	public void Show()
	{
		pointsParent.SetActive(true);
	}

	public void Hide()
	{
		pointsParent.SetActive(false);
	}
}