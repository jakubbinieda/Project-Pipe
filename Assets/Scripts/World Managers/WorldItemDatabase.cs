using System.Collections.Generic;
using UnityEngine;

namespace ProjectPipe
{
    public class WorldItemDatabase : MonoBehaviour
    {
        [field: SerializeField] public WeaponItem NoWeapon { get; set; }
        [field: SerializeField] public List<WeaponItem> Weapons { get; set; } = new();

        private readonly List<Item> _items = new();

        public static WorldItemDatabase Instance { get; private set; }

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (var weapon in Weapons) _items.Add(weapon);
            foreach (var item in _items) item.ItemID = _items.IndexOf(item);
        }
    }
}