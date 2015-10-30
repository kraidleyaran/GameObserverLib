using System;
using System.Collections.Generic;
using GameObjectLib;

namespace GameObserverLib
{
    [Serializable]
    public class ObserverData
    {
        public ObserverData(Dictionary<string, GameObject> objectList )
        {
            ObjectList = objectList;
        }

        public Dictionary<string, GameObject> ObjectList { get; private set; }
    }
}