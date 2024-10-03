using System;
using System.Collections;
using UnityEngine;

namespace CubePuzzle.Cube
{
	public class CubePuzzleCreator : MonoBehaviour
	{
		[SerializeField]
		private CubePlane[] _cubePlanes;

		[SerializeField]
		private Cube _cube;

		[SerializeField]
		private int _rotationIterations;

		[SerializeField]
		private float _delayBetweenRotations;

		private bool _isStartCreatePuzzle = false;

		public void CreatePuzzle(Action callback)
		{
			if (!_isStartCreatePuzzle)
			{
				StartCoroutine(CreatePuzzleCoroutine(callback));
			}
		}

		private IEnumerator CreatePuzzleCoroutine(Action callback)
		{
			_isStartCreatePuzzle = true;

			int randomIndex;
			CubePlane randomCubePlane;

			bool isRotationFinished;

			for (int i = 0; i < _rotationIterations || _cube.IsSolved; i++)
			{
				randomIndex = UnityEngine.Random.Range(0, _cubePlanes.Length);
				randomCubePlane = _cubePlanes[randomIndex];

				Vector3 randomCellPoint = randomCubePlane.GetRandomCellPoint();
				Vector3 randomRotationDirection = randomCubePlane.GetRandomRotationDirection();

				isRotationFinished = false;

				_cube.Rotate(randomCubePlane, randomCellPoint, randomRotationDirection, () => isRotationFinished = true);

				yield return new WaitUntil(() => isRotationFinished);

				yield return new WaitForSeconds(_delayBetweenRotations);
			}

			_isStartCreatePuzzle = false;

			callback?.Invoke();
		}
	}
}