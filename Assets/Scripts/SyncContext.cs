using System;
using System.Threading;
using System.Threading.Tasks;

﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SyncContext : MonoBehaviour {

    /**
     * We use this class in order to queue actions that will be ran
     * on the primary unity thread. The reason for this is because
     * the networking runs on a separate thread, and cross thread
     * communication cannot occur unless we queue it accordingly.
     */

    public static TaskScheduler unityTaskScheduler;
    public static int unityThread;
    public static SynchronizationContext unitySynchronizationContext;
    static public Queue<Action> runInUpdate= new Queue<Action>();

    public void Awake() {
        unitySynchronizationContext = SynchronizationContext.Current;
        unityThread = Thread.CurrentThread.ManagedThreadId;
        unityTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
    }

    public static bool isOnUnityThread => unityThread == Thread.CurrentThread.ManagedThreadId;


    public static void RunOnUnityThread(Action action) {
        // is this right?
        if (unityThread == Thread.CurrentThread.ManagedThreadId)
            action();
        else {
            lock(runInUpdate) {
                runInUpdate.Enqueue(action);
            }
        }
    }

    private void Update() {
        while(runInUpdate.Count > 0) {
            Action action = null;
            lock(runInUpdate) {
                if(runInUpdate.Count > 0)
                    action = runInUpdate.Dequeue();
            }

            action?.Invoke();
        }
    }
}
