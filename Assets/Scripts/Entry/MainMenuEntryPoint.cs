using CubePuzzle.VFXs;
using UnityEngine;

namespace CubePuzzle.Entry
{
	public class MainMenuEntryPoint : MonoBehaviour
	{
		[SerializeField]
		private Fader _fader;

		private void Awake()
		{
			_fader.FadeOut();
		}
	}
}