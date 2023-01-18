using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventInfo
{

}

public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;

    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}

public class EventInfo : IEventInfo
{
    public UnityAction actions;

    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}

public class EventManager
{
    private Dictionary<string, IEventInfo> eventDic;

    public EventManager()
    {
        eventDic = new Dictionary<string, IEventInfo>();
    }

    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo<T>).actions += action;
        else
            eventDic.Add(name, new EventInfo<T>(action));
    }

    public void AddEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo).actions += action;
        else
            eventDic.Add(name, new EventInfo(action));
    }

    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo<T>).actions -= action;
        else
            Debug.LogWarning("EventManager:事件中没有名为" + name + "的事件");
    }

    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo).actions -= action;
        else
            Debug.LogWarning("EventManager:事件中没有名为" + name + "的事件");
    }

    public void EventTrigger<T>(string name, T eventInfo)
    {
        if (eventDic.ContainsKey(name))
            if ((eventDic[name] as EventInfo<T>).actions != null)
                (eventDic[name] as EventInfo<T>).actions.Invoke(eventInfo);
            else
                Debug.LogWarning("EventManager:事件中名为" + name + "的事件为空");
        else
            Debug.LogWarning("EventManager:事件中没有名为" + name + "的事件");
    }

    public void EventTrigger(string name)
    {
        if (eventDic.ContainsKey(name))
            if ((eventDic[name] as EventInfo).actions != null)
                (eventDic[name] as EventInfo).actions.Invoke();
            else
                Debug.LogWarning("EventManager:事件中名为" + name + "的事件为空");
        else
            Debug.LogWarning("EventManager:事件中没有名为" + name + "的事件");
    }

    public void ClearEvent()
    {
        eventDic.Clear();
    }
}
