using System.Linq;
using UnityEngine;

namespace CubePuzzle.Cube
{
	public abstract class RotationModeStrategy
	{
		public abstract RotationMode RotationMode
		{
			get;
		}

		public abstract bool IsStartRotation
		{
			get;
		}

		protected MonoBehaviour _target;

		protected CubePlane[] _cubePlanes;

		public RotationModeStrategy(MonoBehaviour target, CubePlane[] cubePlanes)
		{
			_target = target;
			_cubePlanes = cubePlanes;
		}

		protected Vector3 GetRotateAroundAxis(Vector3 interactionPoint, Vector3 direction)
		{
			CubePlane cubePlane = _cubePlanes.First(x => x.ContainsPoint(interactionPoint));

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


		public abstract void Rotate(Vector3 interactionPoint, Vector3 direction);
	}
}