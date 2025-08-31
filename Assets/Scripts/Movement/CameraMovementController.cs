using CubePuzzle.Constants;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace CubePuzzle.Movement
{
	public class CameraMovementController : MonoBehaviour
	{
		[SerializeField]
		private Vector3 _cameraStartLocalRotation;

		[SerializeField]
		private Transform _centerPoint;

		[SerializeField]
		private float _moveSensitivity;
		
		[SerializeField]
		private float _zoomSensitivity;

		[SerializeField]
		private float _moveSmoothTime;
		
		[SerializeField]
		private float _zoomSmoothTime;

		[SerializeField]
		private float _maxDistance;

		[SerializeField]
		private float _minDistance;

		[SerializeField]
		private float _xAxisDeltaRotation;

		[SerializeField]
		private bool _isEnabled = false;

		[SerializeField]
		private PlayerInput _playerInput;

		private InputAction _touch1InputAction;

		private InputAction _touch2InputAction;

		private Transform _cameraTransform;

		private float _lastZoomGestureDistance = 0;

		private float _rotationY;
		private float _rotationX;

		#region Rotation Smooth
		private Vector3 _currentRotation;
		private Vector3 _nextRotation;
		private Vector3 _smoothMoveVelocity = Vector3.zero;
		#endregion

		#region ZoomSmooth
		private float _currentDistance;
		private float _nextDistance;
		private float _smoothZoomVelocity = 0;
		#endregion

		private void Awake()
		{
			_cameraTransform = Camera.main.transform;

			_touch1InputAction = _playerInput.actions[InputActionName.TOUCH1_ACTION_KEY];
			_touch2InputAction = _playerInput.actions[InputActionName.TOUCH2_ACTION_KEY];
		}

		private void Start()
		{
			_cameraTransform.localEulerAngles = _cameraStartLocalRotation;

			_rotationX = _cameraTransform.localEulerAngles.x;
			_rotationY = _cameraTransform.localEulerAngles.y;
			_currentDistance = _maxDistance;

			_nextRotation = _cameraTransform.localEulerAngles;
			_nextDistance = _currentDistance;
		}

		public void Enable()
		{
			_isEnabled = true;
		}

		public void Disable()
		{
			_isEnabled = false;
		}

		private void Update()
		{
			if (_isEnabled)
			{
				if (_touch1InputAction.IsInProgress())
				{
					if (!_touch2InputAction.IsInProgress())
					{
						_lastZoomGestureDistance = 0;
						CalculateRotation();
					}
					else
					{
						CalculateCameraDistance();
					}
				}
				else
				{
						_lastZoomGestureDistance = 0;
				}
			}

			UpdateRotationAndDistance();
		}

		private void CalculateCameraDistance()
		{
			TouchState first = _touch1InputAction.ReadValue<TouchState>();
			TouchState second = _touch2InputAction.ReadValue<TouchState>();

			float distance = (first.position - second.position).magnitude;

			if(_lastZoomGestureDistance == 0)
			{
				_lastZoomGestureDistance = distance;
			}

			float delta = distance - _lastZoomGestureDistance;

			_lastZoomGestureDistance = distance;

			_nextDistance -= delta * _zoomSensitivity;

			_nextDistance = Mathf.Clamp(_nextDistance, _minDistance, _maxDistance);
		}

		private void CalculateRotation()
		{
			TouchState touch = _touch1InputAction.ReadValue<TouchState>();

			Vector2 touchMovement = touch.delta;

			float xDelta = touchMovement.x * _moveSensitivity;

			float yDelta = touchMovement.y * _moveSensitivity;

			_rotationX -= yDelta;
			_rotationY += xDelta;

			_rotationX = Mathf.Clamp(_rotationX, -_xAxisDeltaRotation, _xAxisDeltaRotation);

			_nextRotation = new Vector3(_rotationX, _rotationY);
		}

		private void UpdateRotationAndDistance()
		{
			_currentDistance = Mathf.SmoothDamp(_currentDistance, _nextDistance, ref _smoothZoomVelocity, _zoomSmoothTime);
			_currentRotation = Vector3.SmoothDamp(_currentRotation, _nextRotation, ref _smoothMoveVelocity, _moveSmoothTime);

			_cameraTransform.localEulerAngles = _currentRotation;
			_cameraTransform.position = _centerPoint.position - _cameraTransform.forward * _currentDistance;
		}
	}
}