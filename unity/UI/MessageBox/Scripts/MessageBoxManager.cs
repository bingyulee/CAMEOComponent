using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Cameo
{
	public class MessageBoxManager : Singleton<MessageBoxManager>
	{
		public UnityEngine.UI.Image Background;
		public MessageBoxInfo[] MessageBoxInfoList;
		public float FadeTime = 0.2f;
		public Color BackgroundColor = Color.black;

		private RectTransform _rectTran;
		private List<BaseMessageBox> _curOpendMessageBoxs = new List<BaseMessageBox> ();
		private BaseMessageBox _curMsgBox = null;
		private Dictionary<string, object> _dicParams;
		private Dictionary<string, BaseMessageBox> _msgBoxInfoMap;

		void Awake()
		{
			_rectTran = GetComponent<RectTransform> ();
			Background.raycastTarget = false;
			Background.color = new Color (0, 0, 0, 0);

			_msgBoxInfoMap = new Dictionary<string, BaseMessageBox> ();
			for (int i = 0; i < MessageBoxInfoList.Length; ++i) 
			{
				_msgBoxInfoMap.Add (MessageBoxInfoList [i].TypeName, MessageBoxInfoList [i].MessageBox);
			}
		}

		public void ShowMessageBox(string TypeName, Dictionary<string, object> dicParams = null)
		{
			Background.raycastTarget = true;
			_curMsgBox = _msgBoxInfoMap [TypeName];
			_dicParams = dicParams;
			if (_curOpendMessageBoxs.Count == 0) {
				CancelInvoke ();
				Background.enabled = true;
				iTweenRectTweener.FadeImageColor (Background, new Color (0, 0, 0, 0), BackgroundColor, FadeTime);
				_rectTran.SetAsLastSibling ();
				Invoke ("onFadeInFinished", FadeTime);
			} 
			else 
			{
				onFadeInFinished ();
			}
		}

		public void OnMessageBoxClosed(BaseMessageBox msgBox)
		{
			_curOpendMessageBoxs.Remove (msgBox);
			GameObject.Destroy (msgBox.gameObject);

			if (_curOpendMessageBoxs.Count == 0) 
			{
				iTweenRectTweener.FadeImageColorTo (Background, new Color (0, 0, 0, 0), FadeTime);
				Invoke ("onFadeOutFinished", FadeTime);
			}
		}

		private void onFadeOutFinished()
		{
			Background.enabled = false;
		}

		private void onFadeInFinished()
		{
			Background.raycastTarget = false;
			GameObject msgBox = GameObject.Instantiate (_curMsgBox.gameObject);
			msgBox.GetComponent<RectTransform> ().SetParent (transform, false);
			msgBox.GetComponent<BaseMessageBox> ().Open (this, _dicParams);
			_curOpendMessageBoxs.Add (msgBox.GetComponent<BaseMessageBox> ());
		}
	}
}
