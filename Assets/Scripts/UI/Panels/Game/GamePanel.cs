using CubePuzzle.Constants;
using CubePuzzle.Interaction;
using CubePuzzle.Movement;
using CubePuzzle.VFXs;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CubePuzzle.UI.Panels.Game
{
	public class GamePanel : MonoBehaviour
	{
		[SerializeField]
		private Toggle _viewModeToggle;

		[SerializeField]
		private Toggle _cubeInteractionModeToggle;

		[SerializeField]
		private Button _exitButton;

		[SerializeField]
		private Fader _fader;

		[SerializeField]
		private CameraMovementController _cameraMovementController;

		[SerializeField]
		private SwipeInteractor _swipeInteractor;

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
			_viewModeToggle.onValueChanged.AddListener(OnViewModeToggleValueChangedEventHandler);
			_cubeInteractionModeToggle.onValueChanged.AddListener(OnCubeInteractionModeToggleValueChangedEventHandler);
			_exitButton.onClick.AddListener(OnExitButtonClickEventHandler);
		}

		private void UnsubscribeEvents()
		{
			_viewModeToggle.onValueChanged.RemoveListener(OnViewModeToggleValueChangedEventHandler);
			_cubeInteractionModeToggle.onValueChanged.RemoveListener(OnCubeInteractionModeToggleValueChangedEventHandler);
			_exitButton.onClick.RemoveListener(OnExitButtonClickEventHandler);
		}

		private void OnExitButtonClickEventHandler()
		{
			_fader.FadeIn(() => SceneManager.LoadScene(SceneNames.MAIN_MENU_SCENE_NAME));
		}

		private void OnViewModeToggleValueChangedEventHandler(bool isOn)
		{
			if (isOn)
			{
				_cameraMovementController.Enable();
			}
			else
			{
				_cameraMovementController.Disable();
			}
		}

		private void OnCubeInteractionModeToggleValueChangedEventHandler(bool isOn)
		{
			if (isOn)
			{
				_swipeInteractor.Enable();
			}
			else
			{
				_swipeInteractor.Disable();
			}
		}
	}
}