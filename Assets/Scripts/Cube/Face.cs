using CubePuzzle.Cube.Enums;
using System;
using UnityEngine;

namespace CubePuzzle.Cube
{
	[Serializable]
	public class Face
	{
		[SerializeField]
		private SideColor _color;

		[SerializeField]
		private Vector3 _localDirection;

		public Vector3 LocalDirection
		{
			get { return _localDirection; }
		}

		public SideColor Color
		{
			get { return _color; }
		}
	}
}