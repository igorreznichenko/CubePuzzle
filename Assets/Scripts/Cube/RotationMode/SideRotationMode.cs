using CubePuzzle.Constants;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace CubePuzzle.Cube
{
	public class SideRotationMode : RotationModeStrategy
	{
		public override RotationMode RotationMode
		{
			get { return RotationMode.SideRotation; }
		}

		private bool _isStartRotation;

		public override bool IsStartRotation
		{
			get { return _isStartRotation; }
		}

		private CubePart[] _parts;

		private float _rotationTime;

		public SideRotationMode(MonoBehaviour target, CubePart[] parts, float rotationTime) : base(target)
		{
			_parts = parts;
			_rotationTime = rotationTime;
		}

		public override void Rotate(CubePlane plane, Vector3 interactionPoint, Vector3 direction, Action callback)
		{
			if (!_isStartRotation)
			{
				_isStartRotation = true;

				interactionPoint = _target.transform.InverseTransformPoint(interactionPoint);

				direction = _target.transform.InverseTransformDirection(direction);

				CubePart part = GetClosestPart(interactionPoint);

				Vector3 rotateAroundAxis = GetRotateAroundAxis(plane, direction);

				CubePart[] rotatableParts = GetRotatableParts(part, rotateAroundAxis);

				_target.StartCoroutine(RotatePartsAroundCenterPointCoroutine(rotatableParts, rotateAroundAxis, () =>
				{
					_isStartRotation = false;
					callback?.Invoke();
				}));
			}
		}

		private IEnumerator RotatePartsAroundCenterPointCoroutine(CubePart[] rotatableParts, Vector3 rotateAroundAxis, Action callback)
		{
			Vector3 centerPoint = GetCenterPointOfCubeParts(rotatableParts);

			float currentRotation = 0;
			float deltaRotation;

			float rotationAmountPerSecond = CubeConstants.ONE_ROTATION_ANGLE / _rotationTime;

			while (currentRotation < CubeConstants.ONE_ROTATION_ANGLE)
			{
				deltaRotation = rotationAmountPerSecond * Time.deltaTime;

				currentRotation += deltaRotation;

				RotateParts(rotatableParts, rotateAroundAxis, centerPoint, deltaRotation);

				yield return null;
			}

			deltaRotation = CubeConstants.ONE_ROTATION_ANGLE - currentRotation;

			RotateParts(rotatableParts, rotateAroundAxis, centerPoint, deltaRotation);

			callback?.Invoke();
		}

		private void RotateParts(CubePart[] rotatableParts, Vector3 rotateAroundAxis, Vector3 centerPoint, float deltaRotation)
		{
			foreach (var part in rotatableParts)
			{
				part.RotateAround(_target.transform.TransformPoint(centerPoint), _target.transform.TransformDirection(rotateAroundAxis), deltaRotation);
			}
		}

		private Vector3 GetCenterPointOfCubeParts(CubePart[] rotatableParts)
		{
			float x = 0, y = 0, z = 0;

			int partsAmount = rotatableParts.Count();

			Vector3 result;

			foreach (var part in rotatableParts)
			{
				x += part.LocalPosition.x;
				y += part.LocalPosition.y;
				z += part.LocalPosition.z;
			}

			result = new Vector3(x / partsAmount, y / partsAmount, z / partsAmount);

			return result;
		}

		private CubePart[] GetRotatableParts(CubePart part, Vector3 rotateAroundAxis)
		{
			rotateAroundAxis.x = Mathf.Abs(rotateAroundAxis.x);
			rotateAroundAxis.y = Mathf.Abs(rotateAroundAxis.y);
			rotateAroundAxis.z = Mathf.Abs(rotateAroundAxis.z);

			float pointValue = Vector3.Dot(rotateAroundAxis, part.LocalPosition);

			CubePart[] cubeParts = _parts.Where(x =>
			{
				float value = Vector3.Dot(x.LocalPosition, rotateAroundAxis);

				if (value > pointValue - CubeConstants.SELECTION_ERRORR_VALUE && value < pointValue + CubeConstants.SELECTION_ERRORR_VALUE)
				{
					return true;
				}

				return false;
			}).ToArray();

			return cubeParts;
		}

		private CubePart GetClosestPart(Vector3 touchPosition)
		{
			float minDistance = float.MaxValue;
			CubePart result = null;

			float distanceVectorMagnitude;

			for (int i = 0; i < _parts.Length; i++)
			{
				distanceVectorMagnitude = (_parts[i].LocalPosition - touchPosition).magnitude;

				if (distanceVectorMagnitude < minDistance)
				{
					minDistance = distanceVectorMagnitude;
					result = _parts[i];
				}
			}

			return result;
		}
	}
}