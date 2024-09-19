using CubePuzzle.Cube.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CubePuzzle.Cube
{
	public class CubePlane
	{
		private CubePlaneType _cubePlaneType;

		public CubePlaneType CubePlaneType
		{
			get { return _cubePlaneType; }
		}

		private float _offset;

		public float Offset
		{
			get { return _offset; }
		}

		public bool ContainsPoint(Vector3 point)
		{
			float offsetPoint = 0;

			switch (CubePlaneType)
			{
				case CubePlaneType.XY:
					offsetPoint = point.z;
					break;
				case CubePlaneType.XZ:
					offsetPoint = point.y;
					break;
				case CubePlaneType.YZ:
					offsetPoint = point.x;
					break;
			}

			return offsetPoint == _offset;
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
			return _offset / Mathf.Abs(_offset);
		}

		public CubePlane(CubePlaneType cubePlaneType, float offset)
		{
			_cubePlaneType = cubePlaneType;
			_offset = offset;
		}
	}
}