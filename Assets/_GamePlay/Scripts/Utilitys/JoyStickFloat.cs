using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utilitys.Input
{
    public class JoyStickFloat : MonoBehaviour,IPointerUpHandler,IPointerDownHandler,IDragHandler
    {
        RectTransform panelTransform;
        [SerializeField]
        RectTransform joyStickPanelRectTransform;
        [SerializeField]
        JoyStick joyStick;

        private void Awake()
        {
            panelTransform = (RectTransform)transform;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            Vector2 offset;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                panelTransform,
                eventData.position,
                null,
                out offset);

            //Debug.Log(offset);
            joyStickPanelRectTransform.localPosition = offset;
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            joyStick.OnPointerUp(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            joyStick.OnDrag(eventData);
        }
    }
}