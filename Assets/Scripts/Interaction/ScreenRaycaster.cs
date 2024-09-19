using UnityEngine;

namespace CubePuzzle.Interaction
{
	public class ScreenRaycaster : MonoBehaviour
	{
		[SerializeField]
		private float _rayDistance;

		private Camera _camera;

		private void Awake()
		{
			_camera = Camera.main;
		}

		public bool DoRaycast(Vector2 screenPoint, out RaycastHit hitInfo)
		{
			Ray ray = _camera.ScreenPointToRay(screenPoint);

			if (Physics.Raycast(ray, out hitInfo, _rayDistance))
			{
				return true;
			}

			return false;
		}
	}
}