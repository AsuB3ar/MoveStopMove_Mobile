﻿using UnityEngine;

namespace CustomAttribute
{
	/// <summary>
	/// Field will be Read-Only in Playmode
	/// </summary>
	public class InitializationFieldAttribute : PropertyAttribute
	{
	}
}

#if UNITY_EDITOR
namespace CustomAttribute.Internal
{
	using UnityEditor;
	
	[CustomPropertyDrawer(typeof(InitializationFieldAttribute))]
	public class InitializationFieldAttributeDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, label, true);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (Application.isPlaying) GUI.enabled = false;
			EditorGUI.PropertyField(position, property, label, true);
			GUI.enabled = true;
		}
	}
}
#endif