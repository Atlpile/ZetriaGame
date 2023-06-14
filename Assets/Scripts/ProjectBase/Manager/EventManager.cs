using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventInfo
{

}


public class EventInfo : IEventInfo
{
    public UnityAction actions;

    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}

public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;

    public EventInfo(UnityAction<T> action)
    {
        actions += action;
    }
}

public class EventInfo<T, K> : IEventInfo
{
    public UnityAction<T, K> actions;

    public EventInfo(UnityAction<T, K> action)
    {
        actions += action;
    }
}


//FIXME：Event需要在Start时才能添加

public class EventManager
{
    private Dictionary<string, IEventInfo> eventDic;

    public EventManager()
    {
        eventDic = new Dictionary<string, IEventInfo>();
    }

    public void AddEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo).actions += action;
        else
            eventDic.Add(name, new EventInfo(action));
    }

    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo<T>).actions += action;
        else
            eventDic.Add(name, new EventInfo<T>(action));
    }

    public void AddEventListener(E_EventType type, UnityAction action)
    {
        string typeName = type.ToString();
        if (eventDic.ContainsKey(typeName))
            (eventDic[typeName] as EventInfo).actions += action;
        else
            eventDic.Add(typeName, new EventInfo(action));
    }

    public void AddEventListener<T>(E_EventType type, UnityAction<T> action)
    {
        string typeName = type.ToString();
        if (eventDic.ContainsKey(typeName))
            (eventDic[typeName] as EventInfo<T>).actions += action;
        else
            eventDic.Add(typeName, new EventInfo<T>(action));
    }

    public void AddEventListener<T, K>(E_EventType type, UnityAction<T, K> action)
    {
        string typeName = type.ToString();
        if (eventDic.ContainsKey(typeName))
            (eventDic[typeName] as EventInfo<T, K>).actions += action;
        else
            eventDic.Add(typeName, new EventInfo<T, K>(action));
    }


    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo).actions -= action;
        else
            Debug.LogWarning("EventManager:事件中没有名为" + name + "的事件");
    }

    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (eventDic.ContainsKey(name))
            (eventDic[name] as EventInfo<T>).actions -= action;
        else
            Debug.LogWarning("EventManager:事件中没有名为" + name + "的事件");
    }

    public void RemoveEventListener(E_EventType type, UnityAction action)
    {
        string typeName = type.ToString();
        if (eventDic.ContainsKey(typeName))
            (eventDic[typeName] as EventInfo).actions -= action;
        else
            Debug.LogWarning("EventManager:事件中没有名为" + typeName + "的事件");
    }

    public void RemoveEventListener<T>(E_EventType type, UnityAction<T> action)
    {
        string typeName = type.ToString();
        if (eventDic.ContainsKey(typeName))
            (eventDic[typeName] as EventInfo<T>).actions -= action;
        else
            Debug.LogWarning("EventManager:事件中没有名为" + typeName + "的事件");
    }

    public void RemoveEventListener<T, K>(E_EventType type, UnityAction<T, K> action)
    {
        string typeName = type.ToString();
        if (eventDic.ContainsKey(typeName))
            (eventDic[typeName] as EventInfo<T, K>).actions -= action;
        else
            Debug.LogWarning("EventManager:事件中没有名为" + typeName + "的事件");
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

    public void EventTrigger(E_EventType type)
    {
        string typeName = type.ToString();
        if (eventDic.ContainsKey(typeName))
            if ((eventDic[typeName] as EventInfo).actions != null)
                (eventDic[typeName] as EventInfo).actions.Invoke();
            else
                Debug.LogWarning("EventManager:事件中名为" + typeName + "的事件为空");
        else
            Debug.LogWarning("EventManager:事件中没有名为" + typeName + "的事件");
    }

    public void EventTrigger<T>(E_EventType type, T eventInfo)
    {
        string typeName = type.ToString();
        if (eventDic.ContainsKey(typeName))
            if ((eventDic[typeName] as EventInfo<T>).actions != null)
                (eventDic[typeName] as EventInfo<T>).actions.Invoke(eventInfo);
            else
                Debug.LogWarning("EventManager:事件中名为" + typeName + "的事件为空");
        else
            Debug.LogWarning("EventManager:事件中没有名为" + typeName + "的事件");
    }

    public void EventTrigger<T, K>(E_EventType type, T eventInfo1, K eventInfo2)
    {
        string typeName = type.ToString();
        if (eventDic.ContainsKey(typeName))
            if ((eventDic[typeName] as EventInfo<T, K>).actions != null)
                (eventDic[typeName] as EventInfo<T, K>).actions.Invoke(eventInfo1, eventInfo2);
            else
                Debug.LogWarning("EventManager:事件中名为" + typeName + "的事件为空");
        else
            Debug.LogWarning("EventManager:事件中没有名为" + typeName + "的事件");
    }


    public void Clear()
    {
        eventDic.Clear();
    }
}
