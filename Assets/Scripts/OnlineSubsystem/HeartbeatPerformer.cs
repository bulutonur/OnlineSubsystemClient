using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace OnlineSubsystem
{
    /// <summary>
    /// Heartbeat is essential to keep server alive.
    /// Add it to network manager
    /// </summary>
    public class HeartbeatPerformer : MonoBehaviour
    {
        [SerializeField] private float travelToleranceTime=0.0f;
        private float interval;
        private Timer timer=new Timer();
        private Server server=null;

        private void Start()
        {
            OnlineSubsystemManager.Instance.OnRegisterServerSuccess += OnRegisterServerSuccess;
            OnlineSubsystemManager.Instance.OnUpdateServerSuccess += OnServerUpdateSuccess;
            OnlineSubsystemManager.Instance.OnUnregisterServer += OnUnregisterServer;
        }

        private void OnRegisterServerSuccess(Server _server,float heartRate)
        {
            server = _server;
            interval = heartRate-travelToleranceTime;
            timer.Start();
        }

        private void OnServerUpdateSuccess(Server _server)
        {
            server = _server;
        }

        private void OnUnregisterServer()
        {
            timer.Stop();
        }

        private void Update()
        {
            if (server == null)
            {
                return;
            }
            
            bool didTimeElapsed = timer.GetTicks() > interval;
            if (didTimeElapsed)
            {
                OnlineSubsystemManager.Instance.PerformHeartbeat(server);
                timer.Start();
            }
        }
        
    }

}