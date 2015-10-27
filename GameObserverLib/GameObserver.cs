using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameObjectLib;
using Messaging;

namespace GameObserverLib
{
    [Serializable]
    public sealed class GameObserver
    {
        // ReSharper disable once InconsistentNaming
        private static readonly GameObserver instance = new GameObserver();

        private List<string> _objectList = new List<string>();
        private Dictionary<string, GameObject> _activeObjects = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject> _inactiveObjects = new Dictionary<string, GameObject>();

        static GameObserver()
        {

        }

        private GameObserver()
        {

        }

        public static GameObserver Instance
        {
            get { return instance; }
        }

        public bool ObserveGameObject(GameObject newGameObject, ObjectStatus objectStatus)
        {
            bool doesObjectExist = _objectList.Contains(newGameObject.Name);
            if (doesObjectExist)
            {
                return false;
            }
            _objectList.Add(newGameObject.Name);
            if (objectStatus == ObjectStatus.Inactive)
            {
                _inactiveObjects.Add(newGameObject.Name, newGameObject.CloneGameObject());
            }
            else
            {
                _activeObjects.Add(newGameObject.Name, newGameObject.CloneGameObject());
            }
            return true;
        }

        public bool UnobserveGameObject(string gameObjectName)
        {
            bool doesObjectExist = _objectList.Contains(gameObjectName);
            if (!doesObjectExist)
            {
                return false;
            }
            _objectList.Remove(gameObjectName);
            if (_inactiveObjects.ContainsKey(gameObjectName))
            {
                _inactiveObjects.Remove(gameObjectName);
            }

            if (_activeObjects.ContainsKey(gameObjectName))
            {
                _activeObjects.Remove(gameObjectName);
            }

            return true;
        }

        public bool SetGameObjectActive(string gameObjectName)
        {
            bool doesObjectExist = _objectList.Contains(gameObjectName);
            if (!doesObjectExist)
            {
                return false;
            }
            GameObject gameObject;
            if (!_inactiveObjects.TryGetValue(gameObjectName, out gameObject))
            {
                return false;
            }
            _activeObjects.Add(gameObject.Name, gameObject);
            _inactiveObjects.Remove(gameObject.Name);
            return true;
        }

        public bool SetGameObject_InActive(string gameObjectName)
        {
            bool doesObjectExist = _objectList.Contains(gameObjectName);
            if (!doesObjectExist)
            {
                return false;
            }
            GameObject gameObject;
            if (!_activeObjects.TryGetValue(gameObjectName, out gameObject))
            {
                return false;
            }
            _inactiveObjects.Add(gameObject.Name, gameObject);
            _activeObjects.Remove(gameObject.Name);
            return true;
        }

        public ObjectStatus GetGameObjectStatus(string gameObjectName)
        {
            bool doesObjectExist = _objectList.Contains(gameObjectName);
            if (!doesObjectExist)
            {
                return ObjectStatus.Error;
            }
            return _inactiveObjects.ContainsKey(gameObjectName) ? ObjectStatus.Inactive : ObjectStatus.Active;
        }

        public void ClearObserverGameObjects()
        {
            _inactiveObjects.Clear();
            _activeObjects.Clear();
            _objectList.Clear();
        }

        public Response SendMessage(Message newMessage)
        {
            ObjectStatus gameObjectStatus = GetGameObjectStatus(newMessage.Receiver);
            switch (gameObjectStatus)
            {
                case ObjectStatus.Active:
                    return _activeObjects[newMessage.Receiver].ReceiveMessage(newMessage);
                                        
                case ObjectStatus.Inactive:
                    return new Response(false, newMessage.Property, "GameObject is inactive", PropType.Error, newMessage.Receiver);
                    
                case ObjectStatus.Error:
                    return new Response(false, newMessage.Property, "GameObject is not being observed", PropType.Error, newMessage.Receiver);

            }

            return new Response(false, "Error", "Error", PropType.Error, "Error");
        }

        public ObserverData GetObserverData()
        {
            return new ObserverData(_activeObjects, _inactiveObjects);
        }

        public bool DoesGameObjectNameExist(string name)
        {
            return _objectList.Contains(name);
        }
    }
}
