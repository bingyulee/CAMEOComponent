using UnityEngine;
using System.Collections;

namespace Cameo
{
	public class MovePageAnimation : BasePageAnimation 
	{
		public Vector3 From;
		public Vector3 To;

		public override void Play (float during, bool isReversed)
		{
			RectTransform rectTran = GetComponent<RectTransform> ();
			if(!isReversed)
				iTweenRectTweener.MoveRectTranform (rectTran, From, To, during);
			else
				iTweenRectTweener.MoveRectTranform (rectTran, To, From, during);
		}
	}
}