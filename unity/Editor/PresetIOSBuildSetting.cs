#region Header
/**
 * Created by CAMEO on 2017-03-07.
 *
 * PresetIOSBuildSetting.cs
 *   This script is for unity build iOS Xcode project.
 *   You can pre-set some Xcode settings in this script rather than set these settings in Xcode project.
 *
 * The authors disclaim copyright to this source code.
 **/
#endregion

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Collections;
using System.IO;

public class PresetIOSBuildSetting {

	[PostProcessBuild]
	public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject) {

		if (buildTarget == BuildTarget.iOS) {

			// Get plist
			string plistPath = pathToBuiltProject + "/Info.plist";
			PlistDocument plist = new PlistDocument();
			plist.ReadFromString(File.ReadAllText(plistPath));

			// Get Info.plist root
			PlistElementDict rootDict = plist.root;

			// Set info.plist key-value parameters
			// And the example is for some usage description
			rootDict.SetString("NSCameraUsageDescription","需要使用您的相機");
			rootDict.SetString("NSPhotoLibraryUsageDescription","需要將相片存入您的相簿");
			rootDict.SetString("NSLocationWhenInUseUsageDescription", "需要使用您的位置資訊");
			rootDict.SetString("NSBluetoothPeripheralUsageDescription", "需要使用您的藍芽");

			// Set info.plist key-array parameters
			// The example is for LSApplicationQueriesSchemes settings
			// If an app's CFBundleURLSchemes is set to 'OtherApp'
			// And your app use OtherApp:// url scheme to launch it
			// You need set this setting in Xcode project or the launch function will fail.
			PlistElementArray applicationQueriesSchemes = rootDict.CreateArray("LSApplicationQueriesSchemes");
			applicationQueriesSchemes.AddString("OtherApp");
			
			// The example is for CFBundleURLTypes setting
			// If other app need to launch your app
			// You need to set CFBundleURLTypes and CFBundleURLSchemes in Xcode project
			// Then other app can use WhatEverNameYouWant:// to launch your app
			PlistElementArray bundleUrlTypes = rootDict.CreateArray("CFBundleURLTypes");
			PlistElementDict bundleDic = bundleUrlTypes.AddDict ();
			bundleDic.SetString ("CFBundleURLName", "your.app.bundle.id");
			PlistElementArray urlSchemesArray = bundleDic.CreateArray ("CFBundleURLSchemes");
			urlSchemesArray.AddString ("WhatEverNameYouWant");

			// If your app use push notification and need to receive notify in background
			// You need to enable Background Mode -> remote-notification
			PlistElementArray bgModes = rootDict.CreateArray("UIBackgroundModes");
			bgModes.AddString("remote-notification");

			// Write back to Info.plist
			File.WriteAllText(plistPath, plist.WriteToString());

			// Example for set project.pbxproj
			// If your project include some third party plugin that not use bitcode
			// You can disable bitcode setting here.
			string projPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";

			PBXProject proj = new PBXProject();
			proj.ReadFromString(File.ReadAllText(projPath));

			string target = proj.TargetGuidByName("Unity-iPhone");
			proj.SetBuildProperty(target, "ENABLE_BITCODE", "false");

			// If your project include some third party plugin that not use ARC (Automatic Reference Counting)
			// You can set Compile Flags -fno-objc-arc here.		
			var guid = proj.FindFileGuidByProjectPath("Libraries/Plugins/iOS/BonjourClientImpl.mm");
			var flags = proj.GetCompileFlagsForFile(target, guid);
			flags.Add("-fno-objc-arc");
			proj.SetCompileFlagsForFile(target, guid, flags);

			// Write back to project.pbxproj
			File.WriteAllText(projPath, proj.WriteToString());
		
		}
	}
}
