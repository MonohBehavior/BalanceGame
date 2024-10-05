using UnityEngine;

namespace BalanceGame
{
    [CreateAssetMenu(fileName = "Items", menuName = "BalanceGame/ItemShop")]
    public class DropItemShopSO : ScriptableObject
    {
        public GameObject[] Items;
    }
}