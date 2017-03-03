using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cameo;

public class MaskSampleController : MonoBehaviour 
{
	public void OnBlackClicked()
	{
		MaskController.Open(MaskType.FOREGROUND, Color.black, strOnCompleteFunc:"onOpened", objOnCompleteTarget:gameObject);
		Invoke ("closeForegroundMask", 0.5f);
	}

	public void OnRedClicked()
	{
		MaskController.Open(MaskType.BACKGROUND, Color.red, strOnCompleteFunc:"onOpened", objOnCompleteTarget:gameObject);
		Invoke ("closeBackgroundMask", 0.5f);
	}

	private void closeForegroundMask()
	{
		MaskController.Close (MaskType.FOREGROUND, strOnCompleteFunc:"onClosed", objOnCompleteTarget:gameObject);
	}

	private void closeBackgroundMask()
	{
		MaskController.Close (MaskType.BACKGROUND, strOnCompleteFunc:"onClosed", objOnCompleteTarget:gameObject);
	}

	private void onOpened()
	{
		Debug.Log ("mask opened!");
	}

	private void onClosed()
	{
		Debug.Log ("mask closed!");
	}
}
