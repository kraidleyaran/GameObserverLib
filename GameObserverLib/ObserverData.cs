using System;
using System.Collections.Generic;
using GameObjectLib;

namespace GameObserverLib
{
    [Serializable]
    public class ObserverData
    {
        public ObserverData(Dictionary<string, GameObject> activeObjects, Dictionary<string, GameObject> inactiveObjects  )
        {
            ActiveObjects = activeObjects;
            InactiveObjects = inactiveObjects;
        }

        public Dictionary<string, GameObject> ActiveObjects;
        public Dictionary<string, GameObject> InactiveObjects;
    }
}