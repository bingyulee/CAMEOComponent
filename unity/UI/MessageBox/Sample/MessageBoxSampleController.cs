using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cameo;

public class MessageBoxSampleController : MonoBehaviour {

	public void OnBtnClicked()
	{
		MessageBoxManager.Instance.ShowMessageBox ("Simple");
	}
}
