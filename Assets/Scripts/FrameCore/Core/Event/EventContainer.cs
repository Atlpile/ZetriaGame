using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameCore
{
    public class EventContainer : IContainer
    {
        private Dictionary<string, IEventInfo> _StructEventContainer = new();

        public Dictionary<string, IEventInfo> StringEventContainer { get; private set; }


        public void Init()
        {
            StringEventContainer = new();
        }


        /// <summary>
        /// 添加监听的事件
        /// </summary>
        /// <param name="EventMethod">带有结构体参数的方法</param>
        /// <typeparam name="TStruct">结构体名称</typeparam>
        public void AddEventListener<TStruct>(Action<TStruct> EventMethod) where TStruct : struct
        {
            string eventArgsKey = typeof(TStruct).Name;
            if (_StructEventContainer.ContainsKey(eventArgsKey))
            {
                var eventInfo = _StructEventContainer[eventArgsKey] as EventInfo<TStruct>;
                eventInfo.AddEvent(EventMethod);
            }
            else
            {
                _StructEventContainer.Add(eventArgsKey, new EventInfo<TStruct>(EventMethod));
            }
        }

        /// <summary>
        /// 移除监听的事件
        /// </summary>
        /// <param name="EventMethod">带有结构体参数的方法</param>
        /// <typeparam name="TStruct">结构体名称</typeparam>
        public void RemoveReventListener<TStruct>(Action<TStruct> EventMethod) where TStruct : struct
        {
            string eventArgsKey = typeof(TStruct).Name;
            if (_StructEventContainer.ContainsKey(eventArgsKey))
            {
                var eventInfo = _StructEventContainer[eventArgsKey] as EventInfo<TStruct>;
                eventInfo.RemoveEvent(EventMethod);
            }
            else
            {
                Debug.LogWarning("EventManager: 事件容器没有名为" + typeof(TStruct).Name + "的事件");
            }
        }

        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="EventMethod">带有结构体参数的方法</param>
        /// <typeparam name="TStruct">结构体名称</typeparam>
        public void EventTrigger<TStruct>(TStruct EventMethod) where TStruct : struct
        {
            string eventArgsKey = typeof(TStruct).Name;
            if (_StructEventContainer.ContainsKey(eventArgsKey))
            {
                var eventInfo = _StructEventContainer[eventArgsKey] as EventInfo<TStruct>;
                eventInfo.TriggerEvent(EventMethod);
            }
            else
            {
                Debug.LogWarning("EventManager:事件容器中没有名为" + eventArgsKey + "的事件");
            }
        }

        public void AddEventListener(string name, Action EventMehtod)
        {
            if (StringEventContainer.ContainsKey(name))
            {
                var eventInfo = StringEventContainer[name] as EventInfo;
                eventInfo.AddEvent(EventMehtod);
            }
            else
            {
                StringEventContainer.Add(name, new EventInfo(EventMehtod));
            }
        }

        public void RemoveEventListener(string name, Action EventMehtod)
        {
            if (StringEventContainer.ContainsKey(name))
            {
                var eventInfo = StringEventContainer[name] as EventInfo;
                eventInfo.RemoveEvent(EventMehtod);
            }
            else
            {
                Debug.LogWarning("EventManager: 事件容器没有名为" + name + "的事件");

            }
        }

        public void EventTrigger(string name)
        {
            if (StringEventContainer.ContainsKey(name))
            {
                var eventInfo = StringEventContainer[name] as EventInfo;
                eventInfo.TriggerEvent();
            }
            else
            {
                Debug.LogWarning("EventManager: 事件容器没有名为" + name + "的事件");

            }
        }

        public void ClearEvent()
        {
            _StructEventContainer.Clear();
            StringEventContainer.Clear();
        }


    }
}
