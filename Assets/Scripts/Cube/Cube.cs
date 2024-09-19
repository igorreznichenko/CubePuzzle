using CubePuzzle.Constants;
using CubePuzzle.Cube.Enums;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace CubePuzzle.Cube
{
	public class Cube : MonoBehaviour
	{
		[SerializeField]
		private CubePart[] _parts;

		[SerializeField]
		private Collider _collider;

		private CubePlane[] _cubePlanes;

		[SerializeField]
		private float _rotationTime;

		private bool _isStartRotation = false;

		[Header("Debug")]
		[SerializeField]
		private Vector3 _touchPosition;

		[SerializeField]
		private Vector3 _touchDirection;

		private void Start()
		{
			Initialize();
		}

		private void Initialize()
		{

			float offset = _collider.bounds.size.x / 2;

			_cubePlanes = new CubePlane[CubeConstants.CUBE_SIDES]
			{
				new CubePlane(Enums.CubePlaneType.XZ, offset),
				new CubePlane(Enums.CubePlaneType.XZ, -offset),
				new CubePlane(Enums.CubePlaneType.XY, offset),
				new CubePlane(Enums.CubePlaneType.XY, -offset),
				new CubePlane(Enums.CubePlaneType.YZ, offset),
				new CubePlane(Enums.CubePlaneType.YZ, -offset),
			};
		}

		[ContextMenu("Debug rotate")]
		private void Rotate()
		{
			Rotate(_touchPosition, _touchDirection);
		}

		[ContextMenu("CheckSolving")]
		private void CheckSolving()
		{
			bool isSolved = CheckSolve(_parts);
			print("IsSolved:" + isSolved);
		}

		public void Rotate(Vector3 touchPosition, Vector3 direction)
		{
			if (!_isStartRotation)
			{
				_isStartRotation = true;

				touchPosition = transform.InverseTransformPoint(touchPosition);
				direction = transform.InverseTransformDirection(direction);

				CubePart part = GetClosestPart(touchPosition);

				Vector3 rotateAroundAxis = GetRotateAroundAxis(touchPosition, direction);

				CubePart[] rotatableParts = GetRotatableParts(part, rotateAroundAxis);

				StartCoroutine(RotatePartsAroundCenterPointCoroutine(rotatableParts, rotateAroundAxis, () => _isStartRotation = false));
			}
		}

		private bool CheckSolve(CubePart[] parts)
		{
			if (IsFacesByDirectionHaveTheSameColor(parts, Vector3.up))
			{
				if (IsFacesByDirectionHaveTheSameColor(parts, Vector3.down))
				{
					if (IsFacesByDirectionHaveTheSameColor(parts, Vector3.left))
					{
						if (IsFacesByDirectionHaveTheSameColor(parts, Vector3.right))
						{
							if (IsFacesByDirectionHaveTheSameColor(parts, Vector3.forward))
							{
								if (IsFacesByDirectionHaveTheSameColor(parts, Vector3.back))
								{
									return true;
								}
							}
						}
					}
				}
			}

			return false;
		}

		private bool IsFacesByDirectionHaveTheSameColor(CubePart[] parts, Vector3 direction)
		{
			Face[] faces = GetFacesByDirection(parts, direction);

			return IsFacesHaveTheSameColor(faces);
		}

		private bool IsFacesHaveTheSameColor(Face[] faces)
		{
			SideColor color = faces[0].Color;

			int i = 0;

			while (i < faces.Length && faces[i].Color == color)
			{
				i++;
			}

			return i == faces.Length;
		}

		private Face[] GetFacesByDirection(CubePart[] parts, Vector3 worldDirection)
		{
			return parts.Select(x => x.GetFaceByDirection(worldDirection)).Where(x => x != null).ToArray();
		}

		private IEnumerator RotatePartsAroundCenterPointCoroutine(CubePart[] rotatableParts, Vector3 rotateAroundAxis, Action callback)
		{
			Vector3 centerPoint = GetCenterPointOfCubeParts(rotatableParts);

			float currentRotation = 0;
			float deltaRotation;

			float rotationAmountPerSecond = CubeConstants.ONE_ROTATION_ANGLE / _rotationTime;

			while(currentRotation < CubeConstants.ONE_ROTATION_ANGLE)
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

		private static void RotateParts(CubePart[] rotatableParts, Vector3 rotateAroundAxis, Vector3 centerPoint, float deltaRotation)
		{
			foreach (var part in rotatableParts)
			{
				part.RotateAround(centerPoint, rotateAroundAxis, deltaRotation);
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

		private Vector3 GetRotateAroundAxis(Vector3 touchPosition, Vector3 direction)
		{
			CubePlane cubePlane = _cubePlanes.First(x => x.ContainsPoint(touchPosition));

			Vector3[] axises = cubePlane.GetAxises();

			Vector3 result;

			float firstAxisDot = Vector3.Dot(axises[0], direction);
			float secondAxisDot = Vector3.Dot(axises[1], direction);

			Vector3 offsetVector = cubePlane.GetOffsetAxis() * cubePlane.GetOffsetSighn();

			float angle;

			float projectionSighn;

			if (Mathf.Abs(firstAxisDot) > Mathf.Abs(secondAxisDot))
			{
				result = axises[1];
				angle = Vector3.SignedAngle(axises[0], axises[1], offsetVector);
				projectionSighn = firstAxisDot / Mathf.Abs(firstAxisDot);
			}
			else
			{
				result = axises[0];
				angle = Vector3.SignedAngle(axises[1], axises[0], offsetVector);
				projectionSighn = secondAxisDot / Mathf.Abs(secondAxisDot);
			}

			result *= projectionSighn;
			result *= (angle / Mathf.Abs(angle));

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