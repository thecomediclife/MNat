﻿using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SmartNodeScript))]
[CanEditMultipleObjects]
public class SmartNodeEditor : Editor {
	private string[] options = new string[] {"Indefinite Pause", "Continue", "Pause with Timer", "Choose Direction", "Do Nothing"};

	SerializedObject smartNode;
	SerializedProperty indexProp;
	SerializedProperty channelProp;
	SerializedProperty snapToProp;
	SerializedProperty lookAtTargetProp;
	SerializedProperty lookDirProp;
	SerializedProperty chooseDirProp;
	SerializedProperty chosenDirProp;
	SerializedProperty delayProp;
	SerializedProperty deactivateProp;
	SerializedProperty playTimesProp;
	SerializedProperty playNextActProp;
	SerializedProperty nextActioninSeqProp;
	SerializedProperty triggerEnableProp;
	SerializedProperty ignorableProp;
	SerializedProperty enabled1Prop;
	SerializedProperty enabled2Prop;
	SerializedProperty enabled3Prop;
	SerializedProperty enabled4Prop;

	private SmartNodeScript _evCtrl = null;

	void OnEnable() {
		_evCtrl = (SmartNodeScript)target;

//		indexProp = serializedObject.FindProperty ("Index");
		smartNode = new SerializedObject(target);
		indexProp = smartNode.FindProperty ("index");
		channelProp = smartNode.FindProperty ("channel");
		snapToProp = smartNode.FindProperty ("snapToTarget");
		lookAtTargetProp = smartNode.FindProperty ("lookAtTarget");
		lookDirProp = smartNode.FindProperty ("lookDirection");
		chooseDirProp = smartNode.FindProperty ("chooseDirection");
		chosenDirProp = smartNode.FindProperty ("chosenDirection");
		delayProp = smartNode.FindProperty ("delay");
		deactivateProp = smartNode.FindProperty ("deactivateAfterPlay");
		playTimesProp = smartNode.FindProperty ("playTimes");
		playNextActProp = smartNode.FindProperty ("playNextAction");
		nextActioninSeqProp = smartNode.FindProperty ("nextActionInSequence");
		triggerEnableProp = smartNode.FindProperty ("triggerEnabled");
		ignorableProp = smartNode.FindProperty ("ignorable");
		enabled1Prop = smartNode.FindProperty ("enabled1");
		enabled2Prop = smartNode.FindProperty ("enabled2");
		enabled3Prop = smartNode.FindProperty ("enabled3");
		enabled4Prop = smartNode.FindProperty ("enabled4");
	}

	public override void OnInspectorGUI() {
		smartNode.Update ();

		GUILayout.Space (5);
		GUILayout.BeginHorizontal();
		GUILayout.Label ("Channel", GUILayout.Width (150));
		channelProp.intValue = EditorGUILayout.IntField(channelProp.intValue);
		GUILayout.EndHorizontal();

		GUILayout.Space (5);
		triggerEnableProp.boolValue = EditorGUILayout.ToggleLeft (" Enable SmartNode", triggerEnableProp.boolValue);
		ignorableProp.boolValue = EditorGUILayout.ToggleLeft (" Ignorable?", ignorableProp.boolValue);

		GUILayout.Space (5);
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("List of available actions", GUILayout.Width (200));
		GUILayout.EndHorizontal ();
//		_evCtrl.index = EditorGUILayout.Popup (indexProp, options);
//		EditorGUILayout.PropertyField (indexProp);
		indexProp.intValue = EditorGUILayout.Popup (indexProp.intValue, options);

		if (indexProp.intValue == 0) {
			//Indefinite Pause
//			GUILayout.Space (5);
//			GUILayout.BeginHorizontal ();
//			_evCtrl.enabled1 = EditorGUILayout.ToggleLeft (" Enable", _evCtrl.enabled1);
//			GUILayout.EndHorizontal ();
			enabled1Prop.boolValue = true;
			enabled2Prop.boolValue = false;
			enabled3Prop.boolValue = false;
			enabled4Prop.boolValue = false;

			GUILayout.Space (5);
			GUILayout.Label ("Snap to Transform", GUILayout.Width (140));
			snapToProp.objectReferenceValue = EditorGUILayout.ObjectField (snapToProp.objectReferenceValue, typeof(Transform), true) as Transform;

			GUILayout.Space (5);
			GUILayout.BeginHorizontal ();
			lookAtTargetProp.boolValue = EditorGUILayout.ToggleLeft (" Look at Target", lookAtTargetProp.boolValue);
			GUILayout.EndHorizontal ();

			if (_evCtrl.lookAtTarget) {
				GUILayout.Label ("Look at Transform", GUILayout.Width (140));
				lookDirProp.objectReferenceValue = EditorGUILayout.ObjectField (lookDirProp.objectReferenceValue, typeof(Transform), true) as Transform;
			}
		} else if (indexProp.intValue == 1) {
			//Continue
//			GUILayout.Space (5);
//			GUILayout.BeginHorizontal ();
//			_evCtrl.enabled2 = EditorGUILayout.ToggleLeft (" Enable", _evCtrl.enabled2);
//			GUILayout.EndHorizontal ();
			enabled1Prop.boolValue = false;
			enabled2Prop.boolValue = true;
			enabled3Prop.boolValue = false;
			enabled4Prop.boolValue = false;

			GUILayout.Space (5);
			GUILayout.BeginHorizontal ();
			chooseDirProp.boolValue = EditorGUILayout.ToggleLeft (" Choose a Direction", chooseDirProp.boolValue);
			GUILayout.EndHorizontal ();

			if (_evCtrl.chooseDirection) {
				GUILayout.Label ("Next Transform in Direction", GUILayout.Width (200));
				chosenDirProp.objectReferenceValue = EditorGUILayout.ObjectField (chosenDirProp.objectReferenceValue, typeof(Transform), true) as Transform;
			}
		} else if (indexProp.intValue == 2) {
			//Timed Pause
//			GUILayout.Space (5);
//			GUILayout.BeginHorizontal ();
//			_evCtrl.enabled3 = EditorGUILayout.ToggleLeft (" Enable", _evCtrl.enabled3);
//			GUILayout.EndHorizontal ();
			enabled1Prop.boolValue = false;
			enabled2Prop.boolValue = false;
			enabled3Prop.boolValue = true;
			enabled4Prop.boolValue = false;

			GUILayout.Space (5);
			GUILayout.Label ("Snap to Transform", GUILayout.Width (140));
			snapToProp.objectReferenceValue = EditorGUILayout.ObjectField (snapToProp.objectReferenceValue, typeof(Transform), true) as Transform;
			
			GUILayout.Space (5);
			GUILayout.BeginHorizontal ();
			lookAtTargetProp.boolValue = EditorGUILayout.ToggleLeft (" Look at Target", lookAtTargetProp.boolValue);
			GUILayout.EndHorizontal ();

			if (_evCtrl.lookAtTarget) {
				GUILayout.Label ("Look at Transform", GUILayout.Width (140));
				lookDirProp.objectReferenceValue = EditorGUILayout.ObjectField (lookDirProp.objectReferenceValue, typeof(Transform), true) as Transform;
			}

			GUILayout.Space (5);
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Delay Seconds", GUILayout.Width (100));
			delayProp.floatValue = EditorGUILayout.FloatField (delayProp.floatValue);
			GUILayout.EndHorizontal ();

			GUILayout.Space (5);
			GUILayout.BeginHorizontal ();
			chooseDirProp.boolValue = EditorGUILayout.ToggleLeft (" Choose a Direction", chooseDirProp.boolValue);
			GUILayout.EndHorizontal ();
			
			if (_evCtrl.chooseDirection) {
				GUILayout.Label ("Next Transform in Direction", GUILayout.Width (200));
				chosenDirProp.objectReferenceValue = EditorGUILayout.ObjectField (chosenDirProp.objectReferenceValue, typeof(Transform), true) as Transform;
			}
		} else if (indexProp.intValue == 3) {
			//Choose Direction
//			GUILayout.Space (5);
//			GUILayout.BeginHorizontal ();
//			_evCtrl.enabled4 = EditorGUILayout.Toggle (" Enable", _evCtrl.enabled4);
//			GUILayout.EndHorizontal ();

			enabled1Prop.boolValue = false;
			enabled2Prop.boolValue = false;
			enabled3Prop.boolValue = false;
			enabled4Prop.boolValue = true;

			GUILayout.Space (5);
			GUILayout.Label ("Snap to Transform", GUILayout.Width (140));
			snapToProp.objectReferenceValue = EditorGUILayout.ObjectField (snapToProp.objectReferenceValue, typeof(Transform), true) as Transform;

			GUILayout.Space (5);
			GUILayout.Label ("Next Transform in Direction", GUILayout.Width (200));
			chosenDirProp.objectReferenceValue = EditorGUILayout.ObjectField (chosenDirProp.objectReferenceValue, typeof(Transform), true) as Transform;
		} else if (indexProp.intValue == 4) {
			enabled1Prop.boolValue = false;
			enabled2Prop.boolValue = false;
			enabled3Prop.boolValue = false;
			enabled4Prop.boolValue = false;
		}

		GUILayout.Space (15);
		deactivateProp.boolValue = EditorGUILayout.ToggleLeft (" Deactivate after finish", deactivateProp.boolValue);

		if (_evCtrl.deactivateAfterPlay) {
			GUILayout.BeginHorizontal();
			GUILayout.Label ("Play number of times", GUILayout.Width (150));
			playTimesProp.intValue = EditorGUILayout.IntField(playTimesProp.intValue);
			GUILayout.EndHorizontal();

			playNextActProp.boolValue = EditorGUILayout.ToggleLeft(" Play next action", playNextActProp.boolValue);

			if (_evCtrl.playNextAction) {
				GUILayout.Label("Next Action in Sequence", GUILayout.Width(200));
				nextActioninSeqProp.objectReferenceValue = EditorGUILayout.ObjectField (nextActioninSeqProp.objectReferenceValue, typeof(GameObject), true) as GameObject;
			}
		}

		smartNode.ApplyModifiedProperties ();
	}


}
