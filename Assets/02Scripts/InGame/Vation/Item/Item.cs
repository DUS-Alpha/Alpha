using NUnit.Framework.Interfaces;
using UnityEngine;

namespace alpha
{
    public abstract class Item : MonoBehaviour
    {
        public ItemDataSO Data { get; private set; }
        public Item(ItemDataSO data) => Data = data;    // 생성자
    }
}