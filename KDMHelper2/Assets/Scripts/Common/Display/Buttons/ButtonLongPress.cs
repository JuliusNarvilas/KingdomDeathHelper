using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Common.Display.Buttons
{
    public class ButtonLongPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField]
        [Tooltip("How long must pointer be down on this object to trigger a long press")]
        private float holdTime = 1f;

        //private bool held = false;
        //public UnityEvent onClick = new UnityEvent();

        public UnityEvent onLongPress = new UnityEvent();

        public void OnPointerDown(PointerEventData eventData)
        {
            //held = false;
            Invoke("OnLongPress", holdTime);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            CancelInvoke("OnLongPress");

            //if (!held)
            //    onClick.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CancelInvoke("OnLongPress");
        }

        void OnLongPress()
        {
            //held = true;
            onLongPress.Invoke();
        }
    }
}
