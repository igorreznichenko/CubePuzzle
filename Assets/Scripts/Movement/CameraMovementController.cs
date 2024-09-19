using UnityEngine;

namespace CubePuzzle.Movement
{
	public class CameraMovementController : MonoBehaviour
	{
		[SerializeField]
		private Transform _centerPoint;

		[SerializeField]
		private float _sensitivity;

		[SerializeField]
		private float _smoothTime;

		[SerializeField]
		private float _maxDistance;

		[SerializeField]
		private float _minDistance;

		[SerializeField]
		private float _xAxisDeltaRotation;

		[SerializeField]
		private bool _isEnabled = false;

		private Transform _cameraTransform;
		
		private float _currentDistance;

		private float _rotationY;
		private float _rotationX;

		private Vector3 _currentRotation;
		private Vector3 _nextRotation;
		private Vector3 _smoothVelocity = Vector3.zero;

		private void Awake()
		{
			_cameraTransform = Camera.main.transform;
		}

		private void Start()
		{
			_rotationX = _cameraTransform.localEulerAngles.x;
			_rotationY = _cameraTransform.localEulerAngles.y;
			_currentDistance = _maxDistance;
			_nextRotation = _cameraTransform.localEulerAngles;
		}

		private void Update()
		{
			if (_isEnabled && Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);

				Vector2 touchMovement = touch.deltaPosition;

				float xDelta = touchMovement.x * _sensitivity;

				float yDelta = touchMovement.y * _sensitivity;

				_rotationX -= yDelta;
				_rotationY += xDelta;

				_rotationX = Mathf.Clamp(_rotationX, -_xAxisDeltaRotation, _xAxisDeltaRotation);

				_nextRotation = new Vector3(_rotationX, _rotationY);

			}

			UpdateRotation();
		}

		private void UpdateRotation()
		{
			_currentRotation = Vector3.SmoothDamp(_currentRotation, _nextRotation, ref _smoothVelocity, _smoothTime);

			_cameraTransform.localEulerAngles = _currentRotation;
			_cameraTransform.position = _centerPoint.position - _cameraTransform.forward * _currentDistance;
		}
	}
}