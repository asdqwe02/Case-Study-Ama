using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFramework.Runner
{
    public class GRunner : MonoBehaviour, IRunner
    {
        private readonly List<Action> _mainThreadActionQueue = new();
        private readonly List<Action> _updateActionQueue = new();
        private readonly List<Action> _lateUpdateActionQueue = new();
        private readonly object _lockCall = new();

        private void Start()
        {
            StartCoroutine(MainThreadUpdater());
        }
        public void StopCoroutine(IEnumerator routine)
        {
            StopCoroutine(routine);
        }

        private void Update()
        {
            foreach (var action in _updateActionQueue)
            {
                action.Invoke();
            }
        }

        private void LateUpdate()
        {
            foreach (var action in _lateUpdateActionQueue)
            {
                action.Invoke();
            }
        }

        private IEnumerator MainThreadUpdater()
        {
            while (true)
            {
                lock (_lockCall)
                {
                    if (_mainThreadActionQueue.Count > 0)
                    {
                        foreach (var action in _mainThreadActionQueue)
                        {
                            action.Invoke();
                        }

                        _mainThreadActionQueue.Clear();
                    }
                }

                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }


        public void ScheduleUpdate(Action action)
        {
            _updateActionQueue.Add(action);
        }

        public void UnscheduleUpdate(Action action)
        {
            _updateActionQueue.Remove(action);
        }

        public void ScheduleLateUpdate(Action action)
        {
            _lateUpdateActionQueue.Add(action);
        }

        public void UnscheduleLateUpdate(Action action)
        {
            _lateUpdateActionQueue.Remove(action);
        }

        public void CallOnMainThread(Action action)
        {
            if (action == null)
            {
                throw new Exception("Function can not be null");
            }

            lock (_lockCall)
            {
                _mainThreadActionQueue.Add(action);
            }
        }
    }
}