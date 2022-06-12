using System;
using UnityEngine;
using Zenject;

public class FactoryMonoObject<T> : IFactory<T>
{
    private readonly GameObject _prefab;
    private Transform _parent;
    private readonly SceneContext _currentContainer;

    public FactoryMonoObject(GameObject prefab, Transform parent)
    {
        _currentContainer = ProjectContext.FindObjectOfType<SceneContext>();
        _parent = parent;
        _prefab = prefab;
        var newParent = new GameObject();
        newParent.transform.parent = parent;
        _parent = newParent.transform;
        _parent.name = _prefab.name;
    }

    public T CreatePoolObject()
    {
        var newObject = _currentContainer.Container == null
            ? GameObject.Instantiate(_prefab)
            : _currentContainer.Container.InstantiatePrefab(_prefab, _parent);

        var returnValue = newObject.GetComponent<T>();
        newObject.SetActive(false);
        if (returnValue != null)
        {
            return returnValue;
        }
        else
        {
            throw new InvalidOperationException(
                $"The requested object is missing from the prefab {typeof(T)} >> {_prefab.name}");
        }
    }
}