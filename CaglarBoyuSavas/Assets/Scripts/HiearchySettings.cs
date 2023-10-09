using UnityEngine;
using UnityEditor;

public static class HiearchySettings
{
   //
   //static HiearchySettings()
   //{
   //    EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItem;
   //}
   //
   //static void HierarchyWindowItem(int instanceID,Rect selectRect)
   //{
   //    var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
   //
   //    if(gameObject != null && gameObject.name.StartsWith("/",System.StringComparison.Ordinal))
   //    {
   //        EditorGUI.DrawRect(selectRect, Color.black);
   //        EditorGUI.DropShadowLabel(selectRect, gameObject.name.Replace("/", "").ToUpperInvariant());
   //    }
   //}
}
