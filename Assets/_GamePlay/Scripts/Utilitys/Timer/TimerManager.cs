﻿using UnityEngine.Events;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Utilitys.Timer
{
    [DefaultExecutionOrder(-100)]
    public class TimerManager : MonoBehaviour
    {
        private static TimerManager inst;
        public static TimerManager Inst => inst;

        public enum LoopType
        {
            Update = 0,
            FixedUpdate = 1
        }
        private const int INIT_STIMER = 50;
        [HideInInspector]
        public event Action TimerUpdate;
        [HideInInspector]
        public event Action TimerFixedUpdate;
        [HideInInspector]
        public event Action TimerLateUpdate;
        private Queue<STimer> sTimers = new Queue<STimer>();
        private Queue<STimer> inUseSTimers = new Queue<STimer>();
        private void Awake()
        {
            if (inst != null)
            {
                Destroy(gameObject);
                return;
            }
            inst = this;
            //DontDestroyOnLoad(gameObject);
            AddSTimerToPool();
        }
        private void Update()
        {
            TimerUpdate?.Invoke();
        }
        private void FixedUpdate()
        {
            TimerFixedUpdate?.Invoke();
        }
        private void LateUpdate()
        {
            TimerLateUpdate?.Invoke();
        }
        public STimer PopSTimer()
        {
            AddSTimerToPool();
            STimer timer = sTimers.Dequeue();
            inUseSTimers.Enqueue(timer);
            return timer;
        }
        public void PushSTimer(STimer timer, bool checkDuplicated = true) //DEV: Can using Heap to optimize
        {
            if (timer == null) return;
            if (checkDuplicated)
            {
                if (sTimers.Contains(timer))
                {
                    return;
                }
                else
                {
                    sTimers.Enqueue(timer);
                }
            }
        }   
        public void WaitForFrame(int frame, Action action) 
        {
            STimer timer = PopSTimer();
            Action timerAction = () =>
            {
                action?.Invoke();
                PushSTimer(timer, false);
            };
            timer.Start(frame, timerAction);
        }
        public void WaitForTime(float time, Action action) 
        {
            STimer timer = PopSTimer();
            Action timerAction = () =>
            {
                action?.Invoke();
                PushSTimer(timer, false);
            };
            timer.Start(time, timerAction);
        }
        public STimer WaitForTime(List<float> times, List<Action> events)
        {
            STimer timer = PopSTimer();
            timer.Start(times, events, () => PushSTimer(timer));
            return timer;
        }
        public void TriggerLoopAction(Action action, float time, int num = 0)
        {
            if (action == null || num <= 0) return;
            STimer timer = PopSTimer();

            int i = 1;
            Action timerAction = () =>
            {
                if (i >= num)
                {
                    StopTimer(ref timer);
                }
                action?.Invoke();
                i++;
            };

            timer.Start(time, timerAction, true);
        }
        public void StopTimer(ref STimer timer)
        {
            if (timer == null) return;

            timer.Stop();
            PushSTimer(timer);
            timer = null;
        }
        public void TriggerLoopAction(Action action, int frame, LoopType type)
        {
            STimer timer = PopSTimer();
            if (type == LoopType.Update)
            {
                timer.ClearEvent(STimer.EventType.FrameUpdate);
                timer.FrameUpdate += action;

                Action timerAction = () => PushSTimer(timer, false);
                timer.Start(frame, timerAction);
            }
            else if(type == LoopType.FixedUpdate)
            {
                timer.ClearEvent(STimer.EventType.FrameFixedUpdate);
                timer.FrameFixedUpdate += action;
                Action timerAction = () =>
                {
                    PushSTimer(timer, false);
                };
                timer.Start(frame, timerAction, STimer.EventType.FrameFixedUpdate);
            }


        }

        public void RecallAllSTimer()
        {
            while(inUseSTimers.Count > 0)
            {
                sTimers.Enqueue(inUseSTimers.Dequeue());
            }
        }
        private void AddSTimerToPool()
        {
            if (sTimers.Count == 0)
            {
                for (int i = 0; i < INIT_STIMER; i++)
                {
                    STimer timer = new STimer();
                    sTimers.Enqueue(timer);
                }
            }
        }

    }

        
}
