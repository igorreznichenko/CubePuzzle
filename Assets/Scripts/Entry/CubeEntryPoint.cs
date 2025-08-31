using CubePuzzle.Cube;
using CubePuzzle.UI.Panels.Game;
using CubePuzzle.VFXs;
using UnityEngine;

namespace CubePuzzle.Entry
{
	public class CubeEntryPoint : MonoBehaviour
	{
		[SerializeField]
		private Fader _fader;

		[SerializeField]
		private GamePanel _gamePanel;

		[SerializeField]
		private WinPanel _winPanel;

		[SerializeField]
		private CubePuzzleCreator _puzzleCreator;

		[SerializeField]
		private Cube.Cube _cube;

		private void Awake()
		{
			Initialize();
		}

		private void Initialize()
		{
			_gamePanel.DisableInteraction();
			_fader.FadeOut(() =>
			{
				_cube.SetRotationMode(RotationMode.SideRotation);
				_puzzleCreator.CreatePuzzle(() =>
				{
					_gamePanel.EnableInteraction();

					_cube.SolvingStateChangedEvent += (isSolved) =>
					{
						if (isSolved)
						{
							_winPanel.gameObject.SetActive(true);
						}
					};
				});
			});
		}
	}
}