using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Cameo;

public class iTweenRectTweener {
	public static void ScaleRectTransform(RectTransform rectTran, Vector2 v2ScaleFrom, Vector2 v2ScaleTo, float fScaleTime, iTween.EaseType easeType = iTween.EaseType.easeOutBounce,string strOnCompleteFunc = "", GameObject objOnCompleteTarget = null) {
		Hashtable htScale = new Hashtable ();
		htScale.Add ("from", v2ScaleFrom);
		htScale.Add ("to", v2ScaleTo);
		htScale.Add ("time", fScaleTime);
		htScale.Add ("easeType", easeType);
		htScale.Add ("onupdate", (System.Action<object>) (newScale => rectTran.localScale = (Vector2) newScale));

		if (strOnCompleteFunc != "") {
			htScale.Add ("oncomplete", strOnCompleteFunc);
			htScale.Add ("oncompletetarget", objOnCompleteTarget);
		}

		iTween.ValueTo (rectTran.gameObject, htScale);
	}

	public static void FadeCanvasGroupAlpha (CanvasGroup canvasGroup, float fFadeFrom, float fFadeTo, float fFadeTime, string strOnCompleteFunc = "", GameObject objOnCompleteTarget = null, Dictionary<string, object> dicOnCompleteParams = null) {
		canvasGroup.alpha = fFadeFrom;

		Hashtable alphaHt = new Hashtable();
		alphaHt.Add ("from", fFadeFrom);
		alphaHt.Add ("to", fFadeTo);
		alphaHt.Add ("time",fFadeTime);
		alphaHt.Add ("onupdate", (System.Action<object>) (floatAlpha => canvasGroup.alpha = (float) floatAlpha));

		if (strOnCompleteFunc != "") {
			alphaHt.Add ("oncomplete", strOnCompleteFunc);
			alphaHt.Add ("oncompletetarget", objOnCompleteTarget);

			if (dicOnCompleteParams != null) {
				alphaHt.Add ("oncompleteparams", dicOnCompleteParams);
			}
		}

		iTween.ValueTo (canvasGroup.gameObject, alphaHt);
	}

	public static void FadeCanvasGroupAlphaTo (CanvasGroup canvasGroup, float fFadeTo, float fFadeTime, string strOnCompleteFunc = "", GameObject objOnCompleteTarget = null, Dictionary<string, object> dicOnCompleteParams = null) {
		FadeCanvasGroupAlpha (canvasGroup, canvasGroup.alpha, fFadeTo, fFadeTime, strOnCompleteFunc, objOnCompleteTarget, dicOnCompleteParams);
	}

	public static void FadeImageColor(Image img, Color colorFrom, Color colorTo, float fFadeTime, string strOnCompleteFunc = "", GameObject objOnCompleteTarget = null, Dictionary<string, object> dicOnCompleteParams = null) {
		img.color = colorFrom;

		Hashtable colorHt = new Hashtable();
		colorHt.Add ("from", colorFrom);
		colorHt.Add ("to", colorTo);
		colorHt.Add ("time",fFadeTime);
		colorHt.Add ("onupdate", (System.Action<object>) (color => img.color = (Color) color));

		if (strOnCompleteFunc != "") {
			colorHt.Add ("oncomplete", strOnCompleteFunc);
			colorHt.Add ("oncompletetarget", objOnCompleteTarget);

			if (dicOnCompleteParams != null) {
				colorHt.Add ("oncompleteparams", dicOnCompleteParams);
			}
		}

		iTween.ValueTo (img.gameObject, colorHt);
	}

	public static void FadeImageColorTo (Image img, Color colorTo, float fFadeTime, string strOnCompleteFunc = "", GameObject objOnCompleteTarget = null, Dictionary<string, object> dicOnCompleteParams = null) {
		FadeImageColor (img, img.color, colorTo, fFadeTime, strOnCompleteFunc, objOnCompleteTarget, dicOnCompleteParams);
	}

	public static void MoveRectTranform(RectTransform rectTran, Vector2 v2MoveFrom, Vector2 v2MoveTo, float fMoveTime, string strOnCompleteFunc = "", GameObject objOnCompleteTarget = null) {
		rectTran.anchoredPosition = v2MoveFrom;

		Hashtable htMove = new Hashtable ();
		htMove.Add ("from", v2MoveFrom);
		htMove.Add ("to", v2MoveTo);
		htMove.Add ("time", fMoveTime);
		htMove.Add ("easeType", iTween.EaseType.easeOutQuad);
		htMove.Add ("onupdate", (System.Action<object>) (newPos => rectTran.anchoredPosition = (Vector2) newPos));

		if (strOnCompleteFunc != "") {
			htMove.Add ("oncomplete", strOnCompleteFunc);
			htMove.Add ("oncompletetarget", objOnCompleteTarget);
		}

		iTween.ValueTo (rectTran.gameObject, htMove);
	}

	public static void MoveRectTranformTo(RectTransform rectTran, Vector2 v2MoveTo, float fMoveTime, string strOnCompleteFunc = "", GameObject objOnCompleteTarget = null) {
		MoveRectTranform (rectTran, rectTran.anchoredPosition, v2MoveTo, fMoveTime, strOnCompleteFunc, objOnCompleteTarget);
	}
}
