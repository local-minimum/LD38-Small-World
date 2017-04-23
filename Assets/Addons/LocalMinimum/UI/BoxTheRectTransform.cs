using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LocalMinimum
{
    public class BoxTheRectTransform : MonoBehaviour
    {

        public void BoxMe()
        {
            RectTransform rt = transform as RectTransform;
            
            Rect r = RectTransformToScreenSpace(rt);
            r.center = rt.anchoredPosition;
            r = GetScreenNormalized(r);

            
            rt.anchorMin = r.min;
            rt.anchorMax = r.max;
            rt.offsetMax = Vector2.zero;
            rt.offsetMin = Vector2.zero;
        }


        public static Rect RectTransformToScreenSpace(RectTransform transform)
        {
            Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
            Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
            rect.x -= (transform.pivot.x * size.x);
            rect.y -= ((1.0f - transform.pivot.y) * size.y);
            return rect;
        }

        public Rect GetScreenNormalized(Rect r)
        {
            Rect rParent = (transform.parent as RectTransform).rect;
            //Debug.Log("Canvas " + rCanvas);
            return new Rect((r.xMin - rParent.xMin) / rParent.width, (r.yMin - rParent.yMin) / rParent.height, r.width / rParent.width, r.height / rParent.height);
            //return new Rect((r.xMin) / rParent.width, (r.yMin) / rParent.height, r.width / rParent.width, r.height / rParent.height);
        }
    }
}