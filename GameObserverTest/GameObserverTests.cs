using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameObjectLib;
using GameObserverLib;
using Messaging;

namespace GameObserverTest
{
    [TestFixture]
    public class GameObserver_Test
    {
        [TearDown]
        public void ClearObserverGameObjects()
        {
            GameObserver observer = GameObserver.Instance;
            observer.ClearObserverGameObjects();
        }
        [TestCase(TestName = "Observing a GameObject using ObserveGameObject() method works correctly")]
        public void ObserveGameObject()
        {
            GameObserver observer = GameObserver.Instance;
            GameObject newGameObject = new GameObject("new game object");
            Assert.IsTrue(observer.ObserveGameObject(newGameObject));
        }

        [TestCase(TestName = "UnObserving a GameObject using UnobserveGameObject method works correctly")]
        public void UnObserverGameObject()
        {
            GameObserver observer = GameObserver.Instance;
            GameObject newGameObject = new GameObject("new game object");
            observer.ObserveGameObject(newGameObject);
            Assert.IsTrue(observer.UnobserveGameObject(newGameObject.Name));
        }


        [TestCase(TestName = "Sending a message to an object works correctly")]
        public void SendMessageToActiveObject()
        {
            GameObserver observer = GameObserver.Instance;
            GameObject newGameObject = new GameObject("new game object");
            observer.ObserveGameObject(newGameObject);
            Message message = new Message(newGameObject.Name, MessageAction.Add, "new property", "a value", PropType.String);
            Response response = observer.SendMessage(message);
            Assert.IsTrue(response.Status);
            Assert.IsTrue(response.Property == "new property");
            Assert.IsTrue(response.Value == "a value");
            Assert.IsTrue(response.PropType == PropType.String);
        }

        [TestCase(TestName = "Sending a message to an object that is not being observed works correctly")]
        public void SendMessageToMissingObject()
        {
            GameObserver observer = GameObserver.Instance;
            GameObject newGameObject = new GameObject("new game object");

            Message message = new Message(newGameObject.Name, MessageAction.Add, "new property", "a value", PropType.String);
            Response response = observer.SendMessage(message);
            Assert.IsFalse(response.Status);
            Assert.IsTrue(response.Value == "GameObject is not being observed");
            Assert.IsTrue(response.PropType == PropType.Error);
        }

    }
}
