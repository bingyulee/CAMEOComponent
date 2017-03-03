using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cameo
{
	public class BasePageManager : MonoBehaviour
	{
		public delegate void SwitchPageDelegate(string from, string to);

		public float FPageSwitchAnimationDuring = 0.35f;
		public string StartPageName;
		public string CustomStartPageName = null;
		public bool OpenNextAfterCloseAnimationFinished = true;
		public SwitchPageDelegate OnStartChangePage = delegate { };
		public SwitchPageDelegate OnEndChangePage = delegate { };

		protected Dictionary<string, BasePage> _pageMap = new Dictionary<string, BasePage>();
		protected bool _isChangingPage = false;
		protected string _curPageName;
		protected string _nextPageName;
		protected bool _isSkipAnimation;

		private Stack<string> _stackPageHistory;
		private static Stack<string> _stackPageHistoryTemp;
		private bool _bIsBack;

		public string CurrentPageName
		{
			get
			{
				return _curPageName;
			}
		}

		protected virtual void Start()
		{
			_bIsBack = false;

			if (_stackPageHistoryTemp != null)
			{
				_stackPageHistory = _stackPageHistoryTemp;
				_stackPageHistoryTemp = null;
			} 
			else
			{
				_stackPageHistory = new Stack<string> ();
			}

			constructPageData (transform);

			foreach (KeyValuePair<string, BasePage> _pageInfo in _pageMap)
			{
				_pageInfo.Value.Init ();
				_pageInfo.Value.gameObject.SetActive (false);
			}

			if (!string.IsNullOrEmpty (CustomStartPageName))
			{
				_bIsBack = true;
				SwitchTo (CustomStartPageName, true);
			}
			else if (string.IsNullOrEmpty (StartPageName))
				Debug.Log ("[BasePageSystem.Start] StartPageName is not defined");
			else
				SwitchTo (StartPageName, false);
		}
			
		public void SwitchTo(string pageName, bool isSkipAnimation)
		{
			if (!_isChangingPage)
			{
				if (!_bIsBack)
				{
					_stackPageHistory.Push (CurrentPageName);
				}
				_bIsBack = false;
				_nextPageName = pageName;
				_isSkipAnimation = isSkipAnimation;
				StartCoroutine ("ChangeToProcess");
			}
		}

		public void TempPageHistory()
		{
			_stackPageHistoryTemp = _stackPageHistory;
		}

		public void ToPrevPage()
		{
			if (_stackPageHistory.Count > 0)
			{
				string strPageName = _stackPageHistory.Pop ();
				_bIsBack = true;
				SwitchTo (strPageName, false);
			}
		}

		private IEnumerator changeToProcess()
		{
			Permission.PermissionCenter.SetForbid (Cameo.Permission.PermissionEnum.ENABLE_UI_CONTROLL, Cameo.Permission.PermissionPriorityEnum.LEVEL1);
			_isChangingPage = true;

			OnStartChangePage (_curPageName, _nextPageName);

			string oldPageName = _curPageName;
			_curPageName = _nextPageName;

			if (string.IsNullOrEmpty (oldPageName))
			{
				_pageMap [_curPageName].gameObject.SetActive (true);
				_pageMap [_curPageName].OnOpen ();

				if (_isSkipAnimation)
				{
					_pageMap [_curPageName].Open (BasePage.PageAnimType.SKIP);
				} 
				else
				{
					_pageMap [_curPageName].Open (BasePage.PageAnimType.PLAY);
					yield return new WaitForSeconds(FPageSwitchAnimationDuring);
				}

				_pageMap [_curPageName].OnOpened ();
			
			} else
			{
				BasePage.PageAnimType curPageAnimType = BasePage.PageAnimType.PLAY;
				BasePage.PageAnimType nextPageAnimType = BasePage.PageAnimType.PLAY;

				nextPageAnimType = (_isSkipAnimation) ? BasePage.PageAnimType.SKIP : BasePage.PageAnimType.PLAY;
				curPageAnimType = (_isSkipAnimation) ? BasePage.PageAnimType.SKIP : BasePage.PageAnimType.PLAY;

				if (OpenNextAfterCloseAnimationFinished)
				{
					_pageMap [oldPageName].OnClose ();
					_pageMap [oldPageName].Close (curPageAnimType);
					yield return new WaitForSeconds (FPageSwitchAnimationDuring);
					_pageMap [oldPageName].OnClosed ();

					_pageMap [_curPageName].gameObject.SetActive (true);
					_pageMap [_curPageName].OnOpen ();
					_pageMap [_curPageName].Open (nextPageAnimType);
					yield return new WaitForSeconds (FPageSwitchAnimationDuring);
					_pageMap [_curPageName].OnOpened ();

					_pageMap [oldPageName].gameObject.SetActive (false);
				} 
				else
				{
					_pageMap [_curPageName].gameObject.SetActive (true);

					_pageMap [oldPageName].OnClose ();
					_pageMap [_curPageName].OnOpen ();

					_pageMap [oldPageName].Close (curPageAnimType);
					_pageMap [_curPageName].Open (nextPageAnimType);

					yield return new WaitForSeconds (FPageSwitchAnimationDuring);

					_pageMap [oldPageName].OnClosed ();
					_pageMap [_curPageName].OnOpened ();

					_pageMap [oldPageName].gameObject.SetActive (false);
				}
			}

			OnEndChangePage (oldPageName, _curPageName);
			Permission.PermissionCenter.SetPermit (Cameo.Permission.PermissionEnum.ENABLE_UI_CONTROLL, Cameo.Permission.PermissionPriorityEnum.LEVEL1);
			_isChangingPage = false;
		}

		private void constructPageData(Transform trans)
		{
			BasePage[] pages = trans.GetComponentsInChildren<BasePage> (true);
			for (int i = 0; i < pages.Length; ++i)
			{
				if (trans != pages [i].transform)
				{
					_pageMap.Add (pages [i].PageName, pages [i]);
					pages [i].Manager = this;
					constructPageData (pages [i].transform);
				}
			}
		}
	}
}