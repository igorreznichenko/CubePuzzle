using CubePuzzle.Extensions;
using System;
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

		public void FadeIn()
		{
			FadeIn(null);
		}

		public void FadeIn(Action callback)
		{
			this.KillCoroutine(ref _fadeCoroutine);

			_fadeCoroutine = StartCoroutine(FadeCoroutine(0, 1, _time, callback));
		}

		public void FadeOut()
		{
			FadeOut(null);
		}

		public void FadeOut(Action callback)
		{
			this.KillCoroutine(ref _fadeCoroutine);

			_fadeCoroutine = StartCoroutine(FadeCoroutine(1, 0, _time, callback));
		}

		private IEnumerator FadeCoroutine(float from, float to, float time, Action callback)
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

			callback?.Invoke();
		}
	}
}