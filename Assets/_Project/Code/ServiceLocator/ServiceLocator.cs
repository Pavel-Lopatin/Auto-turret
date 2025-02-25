using System;
using System.Collections.Generic;
using UnityEngine;

namespace Auto_turret.Code.ServiceLocatorSystem
{
    public class ServiceLocator
    {
        private readonly Dictionary<string, IServiceLocator> _services = new Dictionary<string, IServiceLocator>();

        public static ServiceLocator Instance { get; private set; }

        public static void Init()
        {
            if (Instance != null) return;

            Instance = new ServiceLocator();
            Debug.Log("SERVICE LOCATOR: Instantiated succesfully!");
        }

        public void Register<T>(T service) where T : IServiceLocator
        {
            string key = typeof(T).Name;

            if (_services.ContainsKey(key))
            {
                Debug.LogError($"Service of type {key} already registered as {GetType().Name}");
                return;
            }

            _services.Add(key, service);
        }

        public void Unregister<T>() where T : IServiceLocator
        {
            string key = typeof(T).Name;

            if (!_services.ContainsKey(key))
            {
                Debug.LogError($"Service of type {key} is not registered");
                return;
            }

            _services.Remove(key);
        }

        public T Get<T>() where T : IServiceLocator
        {
            string key = typeof(T).Name;

            if (!_services.ContainsKey(key))
            {
                Debug.LogError($"Service of type {key} is not registered");
                throw new InvalidOperationException();
            }

            return (T)_services[key];
        }
    }
}