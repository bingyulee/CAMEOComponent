using UnityEngine;
using System.Collections;

namespace Cameo
{
	public class BasePage : MonoBehaviour 
	{
		public enum PageAnimType
		{
			PLAY,
			SKIP,
			NONE,
		}

		public int Depth = 0;
		public int ITitleBarIndex = -1;
		public string PageName = "BasePage";
		public string Title = "BasePage";
		public BasePageManager Manager;

		private BasePageAnimation[] _pageAnimators;
		private RectTransform _rectTran;
		private string _strCustomPrevPageName;

		public RectTransform RectTran
		{
			get
			{
				if (_rectTran == null)
				{
					_rectTran = GetComponent<RectTransform> ();
				}
				return _rectTran;
			}
		}

		public virtual void Init()
		{
			_pageAnimators = GetComponents<BasePageAnimation> ();
		}

		public virtual void Open(PageAnimType animType) 
		{
			if (animType == PageAnimType.PLAY)
			{
				for (int i = 0; i < _pageAnimators.Length; ++i)
				{
					_pageAnimators [i].Play (Manager.FPageSwitchAnimationDuring, false);
				}
			}

			if (animType == PageAnimType.SKIP)
			{
				for (int i = 0; i < _pageAnimators.Length; ++i)
				{
					_pageAnimators [i].Play (0, false);
				}
			}
		}

		public virtual void Close(PageAnimType animType)
		{
			if (animType == PageAnimType.PLAY)
			{
				for (int i = 0; i < _pageAnimators.Length; ++i)
				{
					_pageAnimators [i].Play (Manager.FPageSwitchAnimationDuring, true);
				}
			}

			if (animType == PageAnimType.SKIP)
			{
				for (int i = 0; i < _pageAnimators.Length; ++i)
				{
					_pageAnimators [i].Play (0, true);
				}
			}
		}

		public virtual void OnOpen()
		{
			//開啟剛開始時BasePageManager呼叫
		}

		public virtual void OnClose()
		{
			//關閉剛開始時BasePageManager呼叫
		}

		public virtual void OnOpened()
		{
			//開啟動畫結束時BasePageManager呼叫
		}

		public virtual void OnClosed()
		{
			//關閉動畫結束時BasePageManager呼叫
		}
	}
}