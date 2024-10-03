using CubePuzzle.Constants;
using CubePuzzle.VFXs;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CubePuzzle.UI.Panels.Game
{
	public class WinPanel : MonoBehaviour
	{
		[SerializeField]
		private Button _restart;

		[SerializeField]
		private Button _mainMenu;

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
			_restart.onClick.AddListener(OnRestartButtonClickEventHandler);
			_mainMenu.onClick.AddListener(OnMainMenuButtonClickEventHandler);
		}

		private void UnsubscribeEvents()
		{
			_restart.onClick.RemoveListener(OnRestartButtonClickEventHandler);
			_mainMenu.onClick.RemoveListener(OnMainMenuButtonClickEventHandler);
		}

		private void OnRestartButtonClickEventHandler()
		{
			_fader.FadeIn(() =>
			{
				SceneManager.LoadScene(SceneNames.CUBE_SCENE_NAME);
			});
		}

		private void OnMainMenuButtonClickEventHandler()
		{
			_fader.FadeIn(() =>
			{
				SceneManager.LoadScene(SceneNames.MAIN_MENU_SCENE_NAME);
			});
		}
	}
}