using CubePuzzle.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CubePuzzle.UI.Panels.MainMenu
{
	public class StartPanel : MonoBehaviour
	{
		[SerializeField]
		private Button _startButton;

		[SerializeField]
		private Button _exitButton;

		private void OnEnable()
		{
			SubscribeEvents();
		}

		private void OnDisable()
		{
			UnsubscribeEvents();
		}

		private void SubscribeEvents()
		{
			_startButton.onClick.AddListener(OnStartButtonClickEventHandler);
			_exitButton.onClick.AddListener(OnExitButtonClickEventHandler);
		}

		private void UnsubscribeEvents()
		{
			_startButton.onClick.AddListener(OnStartButtonClickEventHandler);
			_exitButton.onClick.AddListener(OnExitButtonClickEventHandler);
		}

		private void OnExitButtonClickEventHandler()
		{
			Application.Quit();
		}

		private void OnStartButtonClickEventHandler()
		{
			SceneManager.LoadScene(SceneNames.GAME_SCENE_NAME);
		}
	}
}