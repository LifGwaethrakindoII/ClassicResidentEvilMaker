using System;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomPropertyDrawer(typeof(CollectionIndex))]
public class CollectionIndexDrawer : VPropertyDrawer
{
	private SerializedProperty index;
	private SerializedProperty objectIndex;
	private SerializedProperty collectionIndex;

	/// <summary>Overrides OnGUI method.</summary>
	/// <param name="_position">Rectangle on the screen to use for the property GUI.</param>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The Label of this Property.</param>
	public override void OnGUI (Rect _position, SerializedProperty _property, GUIContent _label)
	{
		BeginPropertyDrawing(_position, _property, _label);

		float fieldWidth = (layoutWidth * 0.5f) - SPACE_HORIZONTAL;
		float horizontalDisplacement = fieldWidth + SPACE_HORIZONTAL;
		string[] array = null;

		positionRect.width = fieldWidth;
		positionRect.x += horizontalDisplacement;
		array = CollectionsIndexer.GetObjectsNames();
		objectIndex.intValue = Mathf.Min(EditorGUI.Popup(positionRect, objectIndex.intValue, array), Mathf.Max(0, array.Length - 1));
		
		AddVerticalSpace();
		positionRect.x -= horizontalDisplacement;
		array = CollectionsIndexer.GetObjectCollectionsNames(objectIndex.intValue);
		collectionIndex.intValue = Mathf.Min(EditorGUI.Popup(positionRect, collectionIndex.intValue, array), Mathf.Max(0, array.Length - 1));
		positionRect.x += horizontalDisplacement;
		array = CollectionsIndexer.GetObjectCollectionsItems(objectIndex.intValue, collectionIndex.intValue);
		index.intValue = Mathf.Min(EditorGUI.Popup(positionRect, index.intValue, array), Mathf.Max(0, array.Length - 1));
		
		AddVerticalSpace();
		positionRect.x -= horizontalDisplacement;
		EditorGUI.LabelField(positionRect, "Index: ");
		positionRect.x += horizontalDisplacement;
		EditorGUI.LabelField(positionRect, index.intValue.ToString());

		EndPropertyDrawing(_property);
	}

	/// <summary>Override this method to specify how tall the GUI for this field is in pixels.</summary>
	/// <param name="_property">The SerializedProperty to make the custom GUI for.</param>
	/// <param name="_label">The label of this property.</param>
	public override float GetPropertyHeight(SerializedProperty _property, GUIContent _label)
	{
		index = _property.FindPropertyRelative("index");
		objectIndex = _property.FindPropertyRelative("objectIndex");
		collectionIndex = _property.FindPropertyRelative("collectionIndex");

		if(!CollectionsIndexer.loaded) CollectionsIndexer.Load();
		CollectionsIndexer.Update();

		return SPACE_VERTICAL * 3.0f;
	}
}
}