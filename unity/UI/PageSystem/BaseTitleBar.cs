using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Cameo.Permission;

namespace Cameo
{
	public class BaseTitleBar : MonoBehaviour 
	{
		public Text TextTitle;
		public Button BtnPrev;
		public BasePageAnimation[] _pageAnimators;
		public float AnimationDuring = 0.5f;
		public System.Action ToPrevPageCallback = delegate { };

		void Start()
		{
			_pageAnimators = GetComponents<BasePageAnimation> ();
		}

		public void ToPrevPage()
		{
			if (PermissionCenter.CheckPermit (PermissionEnum.ENABLE_UI_CONTROLL))
			{
				ToPrevPageCallback ();
			}
		}

		public void Open(string title)
		{
			Open (title, AnimationDuring);
		}

		public virtual void Open(string title, float during)
		{
			if (TextTitle != null)
			{
				TextTitle.text = title;
			}
			if (!isEnable ())
			{
				gameObject.SetActive (true);
				for (int i = 0; i < _pageAnimators.Length; ++i)
				{
					_pageAnimators [i].Play (during, false);
				}
			}
			CancelInvoke ("onCloseFinished");
		}

		public void Close()
		{
			if (isEnable ())
			{
				gameObject.SetActive (true);
				for (int i = 0; i < _pageAnimators.Length; ++i)
				{
					_pageAnimators [i].Play (AnimationDuring, true);
				}
			}
			Invoke ("onCloseFinished", AnimationDuring);
		}

		protected virtual void onCloseFinished()
		{
			gameObject.SetActive (false);
		}

		private bool isEnable()
		{
			return gameObject.activeSelf;
		}
	}
}