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
    public class GameObserver
    {
        private Dictionary<string, GameObject> _objectList = new Dictionary<string, GameObject>();
        // ReSharper disable once InconsistentNaming
        public GameObserver()
        {
            
        }

        public GameObserver(List<GameObject> gameObjectList)
        {
            foreach (GameObject gameObject in gameObjectList)
            {
                ObserveGameObject(gameObject);
            }
        }
        


        public bool ObserveGameObject(GameObject newGameObject)
        {
            bool doesObjectExist = _objectList.ContainsKey(newGameObject.Name);
            if (doesObjectExist)
            {
                return false;
            }
            _objectList.Add(newGameObject.Name, newGameObject);
            return true;
        }

        public bool UnobserveGameObject(string gameObjectName)
        {
            bool doesObjectExist = _objectList.ContainsKey(gameObjectName);
            if (!doesObjectExist)
            {
                return false;
            }
            _objectList.Remove(gameObjectName);

            return true;
        }



        public void ClearObserverGameObjects()
        {
            _objectList = new Dictionary<string, GameObject>();
        }

        public Response SendMessage(Message newMessage)
        {
            return !_objectList.ContainsKey(newMessage.Receiver) ? new Response(false, "GameObject is not being observed", "GameObject is not being observed", PropType.Error, "GameObject is not being observed") : _objectList[newMessage.Receiver].ReceiveMessage(newMessage);
        }

        public ObserverData GetObserverData()
        {
            return new ObserverData(_objectList);
        }

        public bool DoesGameObjectNameExist(string name)
        {
            return _objectList.ContainsKey(name);
        }

        public void LoadObjectList(Dictionary<string, GameObject> objectList )
        {
            _objectList = new Dictionary<string, GameObject>(objectList);
        }
    }
}
