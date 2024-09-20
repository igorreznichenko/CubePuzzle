using CubePuzzle.Constants;
using CubePuzzle.Cube.Enums;
using CubePuzzle.Interaction;
using System;
using System.Linq;
using UnityEngine;

namespace CubePuzzle.Cube
{
	public class Cube : MonoBehaviour
	{
		[SerializeField]
		private CubePart[] _parts;

		[SerializeField]
		private CubePlane[] _planes;

		[SerializeField]
		private Collider _collider;

		[SerializeField]
		private float _rotationTime;

		private RotationModeStrategy _currentRotationMode = null;

		private RotationModeStrategy[] _rotationModes;

		private void Awake()
		{
			Initialize();
		}

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
			foreach (var plane in _planes)
			{
				plane.InteractionEvent += OncubePlaneInteractionEventHandler;
			}
		}

		private void UnsubscribeEvents()
		{
			foreach (var plane in _planes)
			{
				plane.InteractionEvent -= OncubePlaneInteractionEventHandler;
			}
		}

		private void OncubePlaneInteractionEventHandler(CubePlane plane, Vector3 vector1, Vector3 vector2)
		{
			Rotate(plane, vector1, vector2);
		}

		private void Initialize()
		{
			_rotationModes = new RotationModeStrategy[2]
			{
				new SideRotationMode(this, _parts, _rotationTime),
				new CubeRotationMode(this, _rotationTime)
			};

			_currentRotationMode = _rotationModes[1];
		}

		private void Rotate(CubePlane plane, Vector3 touchPosition, Vector3 direction)
		{
			bool isRotating = _rotationModes.FirstOrDefault(x => x.IsStartRotation) != null;

			if (!isRotating)
			{
				_currentRotationMode.Rotate(plane, touchPosition, direction);
			}
		}

		#region Solving Check
		private bool CheckSolve(CubePart[] parts)
		{
			if (IsFacesByDirectionHaveTheSameColor(parts, Vector3.up))
			{
				if (IsFacesByDirectionHaveTheSameColor(parts, Vector3.down))
				{
					if (IsFacesByDirectionHaveTheSameColor(parts, Vector3.left))
					{
						if (IsFacesByDirectionHaveTheSameColor(parts, Vector3.right))
						{
							if (IsFacesByDirectionHaveTheSameColor(parts, Vector3.forward))
							{
								if (IsFacesByDirectionHaveTheSameColor(parts, Vector3.back))
								{
									return true;
								}
							}
						}
					}
				}
			}

			return false;
		}

		private bool IsFacesByDirectionHaveTheSameColor(CubePart[] parts, Vector3 direction)
		{
			Face[] faces = GetFacesByDirection(parts, direction);

			return IsFacesHaveTheSameColor(faces);
		}

		private bool IsFacesHaveTheSameColor(Face[] faces)
		{
			SideColor color = faces[0].Color;

			int i = 0;

			while (i < faces.Length && faces[i].Color == color)
			{
				i++;
			}

			return i == faces.Length;
		}

		private Face[] GetFacesByDirection(CubePart[] parts, Vector3 worldDirection)
		{
			return parts.Select(x => x.GetFaceByDirection(worldDirection)).Where(x => x != null).ToArray();
		}
		#endregion

		public void SetRotationMode(RotationMode rotationMode)
		{
			_currentRotationMode = _rotationModes.First(x => x.RotationMode == rotationMode);
		}
	}
}