using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace OnlineSubsystem
{
    /// <summary>
    /// Subsystem Manager for Subsytem
    /// Add to the scene, set config values
    /// For results use Actions like OnRegisterServerSuccess or OnRegisterServerFailed,...
    /// </summary>
    public class OnlineSubsystemManager : Utils.Singleton<OnlineSubsystemManager>
    {
        [Header("Config")] [Tooltip("Ip address of the server. For local it is https://127.0.0.1")]
        public string masterIp = "https://127.0.0.1";

        [Tooltip("Port of the server. Default value is 443")]
        public int masterPort = 443;

        public Action<Server,float> OnRegisterServerSuccess;
        public Action OnRegisterServerFailed;

        public Action<GetServerListResult> OnGetServerlistSuccess;
        public Action OnGetServerlistFailed;
        
        public Action<Server> OnUpdateServerSuccess;
        public Action OnUpdateServerFailed;

        public Action OnUnregisterServer;


        private string GetUrl()
        {
            return masterIp + ":" + masterPort;
        }

        private IEnumerator RegisterServerCoroutine(string name, int port, string map, int maxplayers, bool pwprotected,
            string gamemode)
        {
            string uri =
                $"{GetUrl()}/register_server?name={name}&port={port}&map={map}&maxplayers={maxplayers}&pwprotected={pwprotected}&gamemode={gamemode}";
            Debug.Log(uri);
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                webRequest.certificateHandler = new CertificateAcceptor();
                yield return webRequest.SendWebRequest();
                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.Success:
                        RegisterServerResult registerServerResult =
                            JsonConvert.DeserializeObject<RegisterServerResult>(webRequest.downloadHandler.text);
                        if (registerServerResult.error)
                        {
                            Debug.LogError($"Failed to create room: {registerServerResult.message}");
                            OnRegisterServerFailed?.Invoke();
                        }
                        else
                        {
                            Debug.Log($"Successfully created room: {registerServerResult.message}");
                            Server server = new()
                            {
                                name = name,
                                port = port,
                                map = map,
                                playercount = 0,
                                maxplayers = maxplayers,
                                pwprotected = pwprotected,
                                gamemode = gamemode
                            };
                            OnRegisterServerSuccess?.Invoke(server,registerServerResult.heartbeat);
                        }

                        break;
                    default:
                        OnRegisterServerFailed?.Invoke();
                        Debug.LogError($"Something went wrong: {webRequest.error}");
                        break;
                }
            }
        }

        public void RegisterServer(string name, int port, string map, int maxplayers, bool pwprotected, string gamemode)
        {
            StartCoroutine(RegisterServerCoroutine(name, port, map, maxplayers, pwprotected, gamemode));
        }

        private IEnumerator GetServerListCoroutine()
        {
            string uri = $"{GetUrl()}/get_serverlist";
            Debug.Log(uri);
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                webRequest.certificateHandler = new CertificateAcceptor();
                yield return webRequest.SendWebRequest();
                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.Success:
                        GetServerListResult getServerListResult =
                            JsonConvert.DeserializeObject<GetServerListResult>(webRequest.downloadHandler.text);
                        if (getServerListResult.error)
                        {
                            Debug.LogError($"Failed to get serverlist: {getServerListResult.message}");
                            OnGetServerlistFailed?.Invoke();
                        }
                        else
                        {
                            Debug.Log($"Successfully got serverlist: {getServerListResult.message}");
                            OnGetServerlistSuccess?.Invoke(getServerListResult);
                        }

                        break;
                    default:
                        OnGetServerlistFailed?.Invoke();
                        Debug.LogError($"Something went wrong: {webRequest.error}");
                        break;
                }
            }
        }

        public void GetServerList()
        {
            StartCoroutine(GetServerListCoroutine());
        }

        private IEnumerator PerformHeartbeatCoroutine(int port)
        {
            string uri = $"{GetUrl()}/perform_heartbeat?port={port}";
            Debug.Log(uri);
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                webRequest.certificateHandler = new CertificateAcceptor();
                yield return webRequest.SendWebRequest();
                Debug.Log("Performed heartbeat");
                // perform_heartbeat operation does not return result
            }
        }

        public void PerformHeartbeat(Server server)
        {
            if (server == null)
            {
                Debug.LogError("Server is null");
                return;
            }

            StartCoroutine(PerformHeartbeatCoroutine(server.port));
        }

        private IEnumerator UpdateServerCoroutine(string name, int port, string map, int playercount, int maxplayers,
            bool pwprotected,
            string gamemode)
        {
            string uri =
                $"{GetUrl()}/update_server?port={port}&name={name}&map={map}&playercount={playercount}&maxplayers={maxplayers}&pwprotected={pwprotected}&gamemode={gamemode}";
            Debug.Log(uri);
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                webRequest.certificateHandler = new CertificateAcceptor();
                yield return webRequest.SendWebRequest();
                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.Success:
                        UpdateServerResult updateServerResult =
                            JsonConvert.DeserializeObject<UpdateServerResult>(webRequest.downloadHandler.text);
                        if (updateServerResult.error)
                        {
                            Debug.LogError($"Failed to update server: {updateServerResult.message}");
                            OnUpdateServerFailed?.Invoke();
                        }
                        else
                        {
                            Debug.Log($"Successfully updated server: {updateServerResult.message}");
                            Server server = new()
                            {
                                name = name,
                                port = port,
                                map = map,
                                playercount = playercount,
                                maxplayers = maxplayers,
                                pwprotected = pwprotected,
                                gamemode = gamemode
                            };
                            OnUpdateServerSuccess?.Invoke(server);
                        }

                        break;
                    default:
                        OnUpdateServerFailed?.Invoke();
                        Debug.LogError($"Something went wrong: {webRequest.error}");
                        break;
                }
            }
        }

        public void UpdateServer(Server server)
        {
            if (server == null)
            {
                Debug.LogError("Did not create a server");
                OnUpdateServerFailed?.Invoke();
                return;
            }

            StartCoroutine(UpdateServerCoroutine(server.name,
                server.port,
                server.map,
                server.playercount,
                server.maxplayers,
                server.pwprotected,
                server.gamemode));
        }

        private IEnumerator UnregisterServerCoroutine(int port)
        {
            string uri = $"{GetUrl()}/unregister_server?port={port}";
            Debug.Log(uri);
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                webRequest.certificateHandler = new CertificateAcceptor();
                yield return webRequest.SendWebRequest();
                Debug.Log("Performed unregister");
                // unregister_server operation does not return result
                OnUnregisterServer?.Invoke();
            }
        }

        public void UnregisterServer(Server server)
        {
            if (server == null)
            {
                Debug.LogError("Did not create a server");
                return;
            }

            StartCoroutine(UnregisterServerCoroutine(server.port));
        }
    }
}