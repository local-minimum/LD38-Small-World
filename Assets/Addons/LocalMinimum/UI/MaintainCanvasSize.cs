using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LocalMinimum
{
    [ExecuteInEditMode]
    public class MaintainCanvasSize : MonoBehaviour
    {

        [SerializeField]
        Vector2 screenFraction = Vector2.one;


        void Update()
        {
            Vector2 size = (GetComponentInParent<Canvas>().transform as RectTransform).sizeDelta;
            (transform as RectTransform).sizeDelta = Vector2.Scale(size, screenFraction);
        }
    }
}