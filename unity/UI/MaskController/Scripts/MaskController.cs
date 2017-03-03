using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cameo
{
	public enum MaskType
	{
		FOREGROUND,
		BACKGROUND
	}

	public class MaskController : MonoBehaviour
	{
		public static Dictionary<MaskType, MaskController> _dicController = new Dictionary<MaskType, MaskController> ();

		public MaskType Type;
		public Image ImgMask;
		public float FDefaultFadeTime = 0.2f;

		private string _strOnCompleteFunc;
		private GameObject _objOnCompleteTarget;
		private Dictionary<string, object> _dicOnCompleteParams;

		void Start()
		{
			_dicController.Add (Type, this);
			gameObject.SetActive (false);
		}

		public static void Open(MaskType type, Color maskColor, float fFadeTime = -1, string strOnCompleteFunc = "", GameObject objOnCompleteTarget = null, Dictionary<string, object> dicOnCompleteParams = null)
		{
			if (_dicController.ContainsKey (type))
			{
				_dicController [type].open (maskColor, fFadeTime, strOnCompleteFunc, objOnCompleteTarget, dicOnCompleteParams);
			}
		}

		public static void Close(MaskType type, float fFadeTime = -1, string strOnCompleteFunc = "", GameObject objOnCompleteTarget = null, Dictionary<string, object> dicOnCompleteParams = null)
		{
			if (_dicController.ContainsKey (type))
			{
				_dicController [type].close (fFadeTime, strOnCompleteFunc, objOnCompleteTarget, dicOnCompleteParams);
			}
		}

		public static void FadeTo(MaskType type, Color colorTo, float fFadeTime = -1, string strOnCompleteFunc = "", GameObject objOnCompleteTarget = null, Dictionary<string, object> dicOnCompleteParams = null)
		{
			if (_dicController.ContainsKey (type))
			{
				_dicController [type].fadeTo (colorTo, fFadeTime, strOnCompleteFunc, objOnCompleteTarget, dicOnCompleteParams);
			}
		}

		private void open(Color color, float fFadeTime = -1, string strOnCompleteFunc = "", GameObject objOnCompleteTarget = null, Dictionary<string, object> dicOnCompleteParams = null)
		{
			gameObject.SetActive (true);
			fFadeTime = (fFadeTime == -1) ? FDefaultFadeTime : fFadeTime;
			Color colorFrom = new Color (color.r, color.g, color.b, 0);
			iTweenRectTweener.FadeImageColor (ImgMask, colorFrom, color, fFadeTime, strOnCompleteFunc, objOnCompleteTarget, dicOnCompleteParams);
		}

		public void close(float fFadeTime = -1, string strOnCompleteFunc = "", GameObject objOnCompleteTarget = null, Dictionary<string, object> dicOnCompleteParams = null)
		{
			_strOnCompleteFunc = strOnCompleteFunc;
			_objOnCompleteTarget = objOnCompleteTarget;
			_dicOnCompleteParams = dicOnCompleteParams;
			fFadeTime = (fFadeTime == -1) ? FDefaultFadeTime : fFadeTime;
			Color colorTo = new Color (ImgMask.color.r, ImgMask.color.g, ImgMask.color.b, 0);
			iTweenRectTweener.FadeImageColorTo (ImgMask, colorTo, fFadeTime, "onClosed", gameObject);
		}

		public void fadeTo(Color colorTo, float fFadeTime, string strOnCompleteFunc = "", GameObject objOnCompleteTarget = null, Dictionary<string, object> dicOnCompleteParams = null)
		{
			gameObject.SetActive (true);
			fFadeTime = (fFadeTime == -1) ? FDefaultFadeTime : fFadeTime;
			iTweenRectTweener.FadeImageColor (ImgMask, ImgMask.color, colorTo, fFadeTime, strOnCompleteFunc, objOnCompleteTarget, dicOnCompleteParams);
		}

		private void onClosed()
		{
			if (!string.IsNullOrEmpty (_strOnCompleteFunc) && _objOnCompleteTarget != null)
			{
				_objOnCompleteTarget.SendMessage (_strOnCompleteFunc, _dicOnCompleteParams);
				_strOnCompleteFunc = "";
				_objOnCompleteTarget = null;
			}
			gameObject.SetActive (false);
		}
	}
}