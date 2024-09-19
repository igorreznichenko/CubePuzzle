using UnityEngine;

namespace CubePuzzle.Cube
{
	public class CubePart : MonoBehaviour
	{
		[SerializeField]
		private Face[] _face;

		private const float PROJECTION_PINCH = 0.8f;

		public Face GetFaceByDirection(Vector3 worldDirection)
		{
			Vector3 localFaceDirection;

			float projection;

			for (int i = 0; i < _face.Length; i++)
			{
				localFaceDirection = transform.TransformDirection(_face[i].LocalDirection);

				projection = Vector3.Dot(localFaceDirection, worldDirection);

				if (projection > PROJECTION_PINCH)
				{
					return _face[i];
				}
			}

			return null;
		}

		public Vector3 LocalPosition
		{
			get { return transform.localPosition; }
		}

		public void RotateAround(Vector3 point, Vector3 axis, float angle)
		{
			transform.RotateAround(point, axis, angle);
		}
	}
}
