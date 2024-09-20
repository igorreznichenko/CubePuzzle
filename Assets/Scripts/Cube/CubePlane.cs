using CubePuzzle.Cube.Enums;
using CubePuzzle.Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CubePuzzle.Cube
{
	public class CubePlane : MonoBehaviour, ISwipeInteractable
	{
		[SerializeField]
		private CubePlaneType _cubePlaneType;

		public event Action<CubePlane, Vector3, Vector3> InteractionEvent;

		public CubePlaneType CubePlaneType
		{
			get { return _cubePlaneType; }
		}

		public void Interact(Vector3 touchPosition, Vector3 direction)
		{
			InteractionEvent?.Invoke(this, touchPosition, direction);
		}

		private float GetOffset()
		{
			switch (_cubePlaneType)
			{
				case CubePlaneType.XY:
					return transform.localPosition.z;
				case CubePlaneType.XZ:
					return transform.localPosition.y;
				case CubePlaneType.YZ:
					return transform.localPosition.x;
			}

			throw new Exception("Not valid cube plane type!");
		}

		public Vector3[] GetAxises()
		{
			List<Vector3> result = new List<Vector3>();

			switch (_cubePlaneType)
			{
				case CubePlaneType.XY:
					result.Add(Vector3.right);
					result.Add(Vector3.up);
					break;
				case CubePlaneType.XZ:
					result.Add(Vector3.right);
					result.Add(Vector3.forward);
					break;
				case CubePlaneType.YZ:
					result.Add(Vector3.up);
					result.Add(Vector3.forward);
					break;
			}

			return result.ToArray();
		}

		public Vector3 GetOffsetAxis()
		{
			switch (_cubePlaneType)
			{
				case CubePlaneType.XY:
					return Vector3.forward;
				case CubePlaneType.XZ:
					return Vector3.up;
				case CubePlaneType.YZ:
					return Vector3.right;
			}

			throw new Exception("Not valid cube plane type!");
		}

		public float GetOffsetSighn()
		{
			float offset = GetOffset();
			return offset / Mathf.Abs(offset);
		}
	}
}