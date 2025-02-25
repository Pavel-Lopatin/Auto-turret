using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Auto_turret.Code.ObjectPoolingSystem
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        public T prefab { get; }
        public bool autoExpand { get; set; }
        public Transform container { get; }

        private List<T> pool;

        public ObjectPool(T prefab, int count, Transform container)
        {
            this.prefab = prefab;
            this.container = container;

            CreatePool(count);
        }

        private void CreatePool(int count)
        {
            pool = new List<T>();

            for (int i = 0; i < count; i++)
                CreateObject();
        }

        private T CreateObject(bool isActiveByDefault = false)
        {
            var createdObject = Object.Instantiate(prefab, container);
            createdObject.gameObject.SetActive(isActiveByDefault);
            pool.Add(createdObject);
            return createdObject;
        }

        public bool HasFreeElement(out T element)
        {
            foreach (var poolElement in pool)
            {
                if (!poolElement.gameObject.activeInHierarchy)
                {
                    element = poolElement;
                    poolElement.gameObject.SetActive(true);
                    return true;
                }
            }

            element = null;
            return false;
        }

        public T GetFreeElement()
        {
            if (HasFreeElement(out var element))
                return element;

            if (autoExpand)
                return CreateObject(true);

            throw new Exception($"There is now free elements if pool of type {typeof(T)}");
        }
    }
}