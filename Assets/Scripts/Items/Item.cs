using UnityEngine;

namespace ProjectPipe
{
    public class Item : ScriptableObject
    {
        [field: SerializeField] public int ItemID { get; set; }
        [field: SerializeField] public string ItemName { get; set; }
    }
}