using CubePuzzle.Constants;
using CubePuzzle.VFXs;
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

		[SerializeField]
		private Fader _fader;

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
			_fader.FadeIn(() => SceneManager.LoadScene(SceneNames.CUBE_SCENE_NAME));
		}
	}
}