using UnityEngine;

namespace BalanceGame
{
    public class DropItemObject
    {
        public int Index;
        public GameObject DropItemGameObject;

        public DropItemObject(int index, GameObject dropItemGameObject)
        {
            Index = index;
            DropItemGameObject = dropItemGameObject;
        }
    }
}