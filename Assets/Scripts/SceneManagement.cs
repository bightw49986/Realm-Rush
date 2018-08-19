using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class SceneManagement : MonoBehaviour
{

    public event Action<Scene> SceneLoaded;
    public event Action<Scene> SceneReLoaded;
    public event Action<Scene> SceneUnLoaded;
    public bool hasLoaded;

    protected virtual void OnSceneLoaded(Scene scene)
    {
        if (SceneLoaded != null)
        {
            SceneLoaded(scene);
        }
    }

    protected virtual void OnSceneReLoaded(Scene scene)
    {
        if (SceneReLoaded != null)
            SceneReLoaded(scene);
    }

    protected virtual void OnSceneUnLoaded(Scene scene)
    {
        if (SceneUnLoaded != null)
            SceneUnLoaded(scene);
    }

    protected void SubscribeToSceneManager()
    {
        SceneManager.sceneLoaded += SceneLoadedEvent;
        SceneManager.sceneUnloaded += SceneUnLoadedEvent;
    }

    void SceneLoadedEvent(Scene scene, LoadSceneMode mode)
    {
        if (hasLoaded == false)
        {
            OnSceneLoaded(scene);
            hasLoaded = true;
            return;
        }
        OnSceneReLoaded(scene);
    }

    void SceneUnLoadedEvent(Scene scene)
    {
        OnSceneUnLoaded(scene);
    }

}