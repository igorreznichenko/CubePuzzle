using UnityEngine;

namespace CubePuzzle.UI
{
	public class SafeAreaSizer : MonoBehaviour
	{
		[SerializeField] private RectTransform[] _panels;

		private void Start()
		{
			CropToSafeArea();
		}

		private void CropToSafeArea()
		{
			Rect safeArea = Screen.safeArea;
			Vector2 anchorMin = safeArea.position;
			Vector2 anchorMax = anchorMin + safeArea.size;

			anchorMin.x /= Screen.width;
			anchorMin.y /= Screen.height;
			anchorMax.x /= Screen.width;
			anchorMax.y /= Screen.height;

			for (int i = 0; i < _panels.Length; i++)
			{
				_panels[i].anchorMin = anchorMin;
				_panels[i].anchorMax = anchorMax;
			}
		}
	}
}