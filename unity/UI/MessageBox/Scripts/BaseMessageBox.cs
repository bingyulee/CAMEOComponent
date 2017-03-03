using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Cameo
{
	[RequireComponent(typeof(CanvasGroup))]
	public class BaseMessageBox : MonoBehaviour 
	{
		public float fAnimationTime = 0.2f;

		protected Dictionary<string, object> _dicParams = null;
		protected RectTransform _rectTran;
		protected CanvasGroup _canvasGroup;
		protected MessageBoxManager _manager;

		public void Open(MessageBoxManager manager, Dictionary<string, object> dicParams)
		{
			_manager = manager;
			_dicParams = dicParams;

			_rectTran = GetComponent<RectTransform> ();
			_canvasGroup = GetComponent<CanvasGroup> ();

			onOpen ();
			iTweenRectTweener.FadeCanvasGroupAlpha (_canvasGroup, 0, 1, fAnimationTime, "onOpened", gameObject);
			iTweenRectTweener.ScaleRectTransform (_rectTran, new Vector2 (0.5f, 0.5f), Vector2.one, fAnimationTime, iTween.EaseType.easeOutBounce); 
		}

		public void Close()
		{
			onClosed ();
			iTweenRectTweener.FadeCanvasGroupAlphaTo (_canvasGroup, 0, fAnimationTime, "onBoxClosed", gameObject);
			iTweenRectTweener.ScaleRectTransform (_rectTran, Vector2.one, new Vector2 (0.5f, 0.5f), fAnimationTime, iTween.EaseType.linear); 
		}

		protected void onBoxClosed()
		{
			onClosed ();
			_manager.OnMessageBoxClosed (this);
		}

		protected virtual void onOpen()
		{

		}

		protected virtual void onClose()
		{

		}

		protected virtual void onOpend()
		{
			
		}

		protected virtual void onClosed()
		{
			
		}
	}

	[System.Serializable]
	public class MessageBoxInfo
	{
		public string TypeName;
		public BaseMessageBox MessageBox;
	}
}
