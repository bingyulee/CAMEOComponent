using UnityEngine;
using System.Collections;

namespace Cameo
{
	[RequireComponent(typeof(UnityEngine.CanvasGroup))]
	public class FadePageAnimation : BasePageAnimation 
	{
		public CanvasGroup Group;
		public float From;
		public float To;

		public override void Play (float during, bool isReversed)
		{
			if (!isReversed)
				iTweenRectTweener.FadeCanvasGroupAlpha (Group, From, To, during);
			else
				iTweenRectTweener.FadeCanvasGroupAlpha (Group, To, From, during);
		}
	}
}
