using CubePuzzle.VFXs;
using UnityEngine;

namespace CubePuzzle.Entry
{
	public class CubeEntryPoint : MonoBehaviour
	{
		[SerializeField]
		private Fader _fader;

		private void Awake()
		{
			Initialize();
		}

		private void Initialize()
		{
			_fader.FadeOut();
		}
	}
}