using CubePuzzle.Constants;
using CubePuzzle.Cube;
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
		private Toggle _sideRotationMode;

		[SerializeField]
		private Toggle _cubeRotationMode;

		[SerializeField]
		private Button _exitButton;

		[SerializeField]
		private Fader _fader;

		[SerializeField]
		private CameraMovementController _cameraMovementController;

		[SerializeField]
		private SwipeInteractor _swipeInteractor;

		[SerializeField]
		private Cube.Cube _cube;

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
			_sideRotationMode.onValueChanged.AddListener(OnSideRotationModeToggleValueChangedEventHandler);
			_cubeRotationMode.onValueChanged.AddListener(OnCubeRotationModeToggleValueChangedEventHandler);
			_exitButton.onClick.AddListener(OnExitButtonClickEventHandler);
		}

		private void UnsubscribeEvents()
		{
			_viewModeToggle.onValueChanged.RemoveListener(OnViewModeToggleValueChangedEventHandler);
			_sideRotationMode.onValueChanged.RemoveListener(OnSideRotationModeToggleValueChangedEventHandler);
			_cubeRotationMode.onValueChanged.RemoveListener(OnCubeRotationModeToggleValueChangedEventHandler);
			_exitButton.onClick.RemoveListener(OnExitButtonClickEventHandler);
		}

		private void OnExitButtonClickEventHandler()
		{
			_fader.FadeIn(() => SceneManager.LoadScene(SceneNames.MAIN_MENU_SCENE_NAME));
		}

		private void OnCubeRotationModeToggleValueChangedEventHandler(bool isOn)
		{
			SetActiveSwipeInteractor(isOn);

			if (isOn)
			{
				_cube.SetRotationMode(RotationMode.CubeRotation);
			}
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

		private void OnSideRotationModeToggleValueChangedEventHandler(bool isOn)
		{
			SetActiveSwipeInteractor(isOn);

			if (isOn)
			{
				_cube.SetRotationMode(RotationMode.SideRotation);
			}
		}

		private void SetActiveSwipeInteractor(bool isOn)
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