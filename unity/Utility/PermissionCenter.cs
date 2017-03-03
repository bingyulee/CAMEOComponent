using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Cameo.Permission
{
	public enum PermissionEnum
	{
		BEACON_CAN_TOGGLE_EVENT,
		ENABLE_UI_CONTROLL
	}

	public enum PermissionPriorityEnum : int
	{
		NONE = 0,
		LEVEL1 = 1,
		LEVEL2 = 2,
		LEVEL3 = 3,
		LEVEL4 = 4
	}

	public class PermissionCenter
	{
		private static Dictionary<PermissionEnum, PermissionPriorityEnum> _dicPermissionInfo;

		private static bool _bIsInit = false;

		public static void Init()
		{
			_dicPermissionInfo = new Dictionary<PermissionEnum, PermissionPriorityEnum>();

			foreach(PermissionEnum permission in Enum.GetValues(typeof(PermissionEnum)))
			{
				_dicPermissionInfo.Add (permission, PermissionPriorityEnum.NONE);
			}

			_bIsInit = true;
		}

		public static bool CheckPermit(PermissionEnum permission)
		{
			if (!_bIsInit)
				Init ();
			
			return _dicPermissionInfo [permission] == PermissionPriorityEnum.NONE;
		}

		public static void SetPermit(PermissionEnum permission, PermissionPriorityEnum priority)
		{
			if (!_bIsInit)
				Init ();
			
			if (priority >= _dicPermissionInfo [permission])
			{
				Debug.LogFormat ("[PermissionCenter.SetPermit] Set permit: {0}", permission.ToString ());
				_dicPermissionInfo [permission] = PermissionPriorityEnum.NONE;
			}
		}

		public static void SetForbid(PermissionEnum permission, PermissionPriorityEnum priority)
		{
			if (!_bIsInit)
				Init ();
			
			if (priority > _dicPermissionInfo [permission])
			{
				Debug.LogFormat ("[PermissionCenter.SetPermit] Set forbit: {0}", permission.ToString ());
				_dicPermissionInfo [permission] = priority;
			}
		}
	}
}