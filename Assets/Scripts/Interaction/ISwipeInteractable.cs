using UnityEngine;

namespace CubePuzzle.Interaction
{
	public interface ISwipeInteractable
	{
		public void Interact(Vector3 touchPosition, Vector3 direction);
	}
}