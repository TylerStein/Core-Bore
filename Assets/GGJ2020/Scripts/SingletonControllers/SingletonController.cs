using System;
using UnityEngine;

public abstract class SingletonController<T> : MonoBehaviour where T : SingletonController<T> {

    private static T Local;


    public static T Inst => GetInst();


    private static T GetInst() {
        if (Local == null) {
            Local = LazyInit();
        }
        return Local;
    }

    private static T LazyInit() {
        var objectOfType = FindOrInstantiateSingleton();
        if(objectOfType == null)
            throw new Exception($"We cant find the singleton {typeof(T).Name}");
        RemoveCloneTag(objectOfType.gameObject);
        objectOfType.Init();
        return objectOfType;
    }


    private static T FindOrInstantiateSingleton()
        => FindObjectOfType<T>() ?? Instantiate(Resources.Load<GameObject>(typeof(T).Name)).GetComponent<T>();


    private static void RemoveCloneTag(GameObject go) {
        go.name = go.name.Replace("(Clone)", "");
    }


    protected virtual void Init() {
        Debug.Log($"Initialized {typeof(T).ToString()}");
    }


}