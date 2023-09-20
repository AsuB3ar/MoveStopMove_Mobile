using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utilitys.Input
{
    public class JoyStick : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
    {
        private RectTransform joystickTransform;
        [SerializeField]
        private float dragThreshold = 0.6f;
        [SerializeField]
        private int dragMovementDistance = 30;
        [SerializeField]
        private int dragOffsetDistance = 100;

        public event Action<Vector2> OnMove;
        private void Awake()
        {
            joystickTransform = (RectTransform)transform;
        }
        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log("On Drag");
            Vector2 offset;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystickTransform,
                eventData.position,
                null,
                out offset);

            offset = Vector2.ClampMagnitude(offset, dragOffsetDistance) / dragOffsetDistance;
            
            joystickTransform.anchoredPosition = offset * dragMovementDistance;

            Vector2 inputVector = CalculateMovementInput(offset);
            OnMove?.Invoke(inputVector);
        }

        private Vector2 CalculateMovementInput(Vector2 offset)
        {
            float x = Mathf.Abs(offset.x) > dragThreshold ? offset.x : 0;
            float y = Mathf.Abs(offset.y) > dragThreshold ? offset.y : 0;
            return new Vector2(x, y);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            joystickTransform.anchoredPosition = Vector2.zero;
            OnMove?.Invoke(Vector2.zero);
            //Debug.Log("On Pointer Up");
        }

    }
}