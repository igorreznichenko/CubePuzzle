using CubePuzzle.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CubePuzzle.VFXs
{
	public class Fader : MonoBehaviour
	{
		[SerializeField]
		private Graphic _target;

		[SerializeField]
		private float _time;

		private Coroutine _fadeCoroutine = null;

		[ContextMenu("FadeIn")]
		public void FadeIn()
		{
			this.KillCoroutine(ref _fadeCoroutine);

			_fadeCoroutine = StartCoroutine(FadeCoroutine(0, 1, _time));
		}

		[ContextMenu("FadeOut")]
		public void FadeOut()
		{
			this.KillCoroutine(ref _fadeCoroutine);

			_fadeCoroutine = StartCoroutine(FadeCoroutine(1, 0, _time));
		}

		private IEnumerator FadeCoroutine(float from, float to, float time)
		{
			Color current = _target.color;
			Color targetColor = current;

			targetColor.a = to;

			current.a = from;

			_target.color = current;

			float currentTime = 0;

			while (currentTime < time)
			{
				_target.color = Color.Lerp(current, targetColor, currentTime / time);

				currentTime += Time.deltaTime;

				yield return null;
			}

			_target.color = targetColor;
		}
	}
}