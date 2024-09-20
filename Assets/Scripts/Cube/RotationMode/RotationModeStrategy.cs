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

		public RotationModeStrategy(MonoBehaviour target)
		{
			_target = target;
		}

		protected Vector3 GetRotateAroundAxis(CubePlane plane, Vector3 direction)
		{
			Vector3[] axises = plane.GetAxises();

			Vector3 result;

			float firstAxisDot = Vector3.Dot(axises[0], direction);
			float secondAxisDot = Vector3.Dot(axises[1], direction);

			Vector3 offsetVector = plane.GetOffsetAxis() * plane.GetOffsetSighn();

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


		public abstract void Rotate(CubePlane plane, Vector3 interactionPoint, Vector3 direction);
	}
}