using System;
using System.Collections.Generic;
using UnityEngine;

namespace alpha
{
    public enum EEventType { PlayerLevelUp }
    public static class EventBus
    {
        private static Dictionary<EEventType, Action> m_assignedDic = new();

        public static void Raise(EEventType eventType)
        {
            if(m_assignedDic.TryGetValue(eventType, out Action existingAction))
            {
                existingAction?.Invoke();
            }
        }
        public static void Subscribe(EEventType eventType, Action action)
        {
            if(m_assignedDic.ContainsKey(eventType))
            {
                m_assignedDic[eventType] += action;
            }
            else
            {
                m_assignedDic[eventType] = action;
            }
        }

        public static void UnSubscribe(EEventType eventType, Action action)
        {
            if (m_assignedDic.ContainsKey(eventType))
            {
                m_assignedDic[eventType] -= action;
            }
        }
    }
}