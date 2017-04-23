using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LocalMinimum
{
    [CustomEditor(typeof(BoxTheRectTransform))]
    public class BoxTheRectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Box Me"))
            {
                (target as BoxTheRectTransform).BoxMe();
                serializedObject.ApplyModifiedProperties();
            }
        }

    }
}
