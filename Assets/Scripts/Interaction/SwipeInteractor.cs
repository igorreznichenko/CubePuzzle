using CubePuzzle.Constants;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace CubePuzzle.Interaction
{
	public class SwipeInteractor : MonoBehaviour
	{
		[SerializeField]
		private PlayerInput _playerInput;

		[SerializeField]
		private ScreenRaycaster _screenRaycaster;

		[SerializeField]
		private float _swipePinch;

		[SerializeField]
		private bool _isActive;

		private ISwipeInteractable _interactable = null;

		private Vector2 _currentInteractableStartScreenInteractionPoint = Vector2.zero;
		private Vector3 _currentInteractableStartInteractionWorldPoint = Vector3.zero;

		private InputAction _touch1InputAction;

		public void Enable()
		{
			_isActive = true;
		}

		public void Disable()
		{
			_isActive = false;
		}

		private void Awake()
		{
			_touch1InputAction = _playerInput.actions[InputActionName.TOUCH1_ACTION_KEY];
		}

		private void Update()
		{
			if (_isActive && _touch1InputAction.IsInProgress())
			{
				TouchState touch = _touch1InputAction.ReadValue<TouchState>();

				if (_interactable != null)
				{
					if (touch.phase == TouchPhase.Moved)
					{
						Vector2 touchPosition = touch.position;

						if (_screenRaycaster.DoRaycast(touchPosition, out RaycastHit hitInfo) && hitInfo.collider.TryGetComponent(out ISwipeInteractable swipeInteractable))
						{
							if (swipeInteractable == _interactable)
							{
								float swipeVectorDistanse = (touch.position - _currentInteractableStartScreenInteractionPoint).magnitude;

								if (swipeVectorDistanse > _swipePinch)
								{
									_interactable.Interact(_currentInteractableStartInteractionWorldPoint, hitInfo.point - _currentInteractableStartInteractionWorldPoint);

									ResetInteractableTracking();
								}
							}
							else
							{
								SetInteractable(swipeInteractable, touchPosition, hitInfo.point);
							}
						}
						else
						{
							ResetInteractableTracking();
						}
					}
					else if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
					{
						ResetInteractableTracking();
					}
				}
				else
				{
					if (touch.phase == TouchPhase.Began)
					{
						Vector2 touchPosition = touch.position;

						if (_screenRaycaster.DoRaycast(touchPosition, out RaycastHit hitInfo) && hitInfo.collider.TryGetComponent(out ISwipeInteractable swipeInteractable))
						{
							SetInteractable(swipeInteractable, touchPosition, hitInfo.point);
						}
					}
				}
			}
		}

		private void ResetInteractableTracking()
		{
			_interactable = null;
			_currentInteractableStartScreenInteractionPoint = Vector2.zero;
			_currentInteractableStartInteractionWorldPoint = Vector3.zero;
		}

		private void SetInteractable(ISwipeInteractable swipeInteractable, Vector3 screenPoint, Vector3 worldPoint)
		{
			_interactable = swipeInteractable;
			_currentInteractableStartScreenInteractionPoint = screenPoint;
			_currentInteractableStartInteractionWorldPoint = worldPoint;
		}
	}
}