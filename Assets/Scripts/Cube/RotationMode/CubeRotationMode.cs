using CubePuzzle.Constants;
using System;
using System.Collections;
using UnityEngine;

namespace CubePuzzle.Cube
{
	public class CubeRotationMode : RotationModeStrategy
	{
		public override RotationMode RotationMode
		{
			get { return RotationMode.CubeRotation; }
		}

		private bool _isStartRotation;

		public override bool IsStartRotation
		{
			get { return _isStartRotation; }
		}

		private float _rotationTime;

		public CubeRotationMode(MonoBehaviour target, CubePlane[] planes, float rotationTime) : base(target, planes)
		{
			_rotationTime = rotationTime;
		}

		public override void Rotate(Vector3 interactionPoint, Vector3 direction)
		{
			if (!_isStartRotation)
			{
				_isStartRotation = true;

				interactionPoint = _target.transform.InverseTransformPoint(interactionPoint);
				direction = _target.transform.InverseTransformDirection(direction);

				Vector3 rotationAxis = GetRotateAroundAxis(interactionPoint, direction);

				_target.StartCoroutine(RotateCoroutine(_target.transform, rotationAxis, _rotationTime, () => _isStartRotation = false));
			}
		}

		private IEnumerator RotateCoroutine(Transform target, Vector3 rotationAxis, float rotationTime, Action callback)
		{
			float currentRotation = 0;
			float deltaRotation;

			float rotationAmountPerSecond = CubeConstants.ONE_ROTATION_ANGLE / rotationTime;

			while (currentRotation < CubeConstants.ONE_ROTATION_ANGLE)
			{
				deltaRotation = rotationAmountPerSecond * Time.deltaTime;

				currentRotation += deltaRotation;

				target.Rotate(rotationAxis, deltaRotation, Space.Self);

				yield return null;
			}

			deltaRotation = CubeConstants.ONE_ROTATION_ANGLE - currentRotation;
			
			target.Rotate(rotationAxis, deltaRotation, Space.Self);

			callback?.Invoke();
		}
	}
}