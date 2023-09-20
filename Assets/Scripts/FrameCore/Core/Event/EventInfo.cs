using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public class EventInfo : IEventInfo
    {
        public event Action Actions;

        public EventInfo(Action ActionMethod)
        {
            Actions += ActionMethod;
        }

        public void AddEvent(Action ActionMethod)
        {
            Actions += ActionMethod;
        }

        public void RemoveEvent(Action ActionMethod)
        {
            Actions -= ActionMethod;
        }

        public void TriggerEvent()
        {
            Actions?.Invoke();
        }
    }

    public class EventInfo<T> : IEventInfo
    {
        public event Action<T> Actions;

        public EventInfo(Action<T> MethodInfo)
        {
            Actions += MethodInfo;
        }

        public void AddEvent(Action<T> MethodInfo)
        {
            Actions += MethodInfo;
        }

        public void RemoveEvent(Action<T> MethodInfo)
        {
            Actions -= MethodInfo;
        }

        public void TriggerEvent(T info)
        {
            Actions?.Invoke(info);
        }
    }


}


