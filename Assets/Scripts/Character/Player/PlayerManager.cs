using UnityEngine;

namespace ProjectPipe
{
    public class PlayerManager : CharacterManager
    {
        public PlayerAnimatorManager PlayerAnimatorManager { get; private set; }
        public PlayerCombatManager PlayerCombatManager { get; private set; }
        public PlayerEquipmentManager PlayerEquipmentManager { get; private set; }
        public PlayerInventoryManager PlayerInventoryManager { get; private set; }
        public PlayerLocomotionManager PlayerLocomotionManager { get; private set; }
        public PlayerStatsManager PlayerStatsManager { get; private set; }
        public PlayerSoundFXManager PlayerSoundFXManager { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            PlayerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            PlayerCombatManager = GetComponent<PlayerCombatManager>();
            PlayerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            PlayerInventoryManager = GetComponent<PlayerInventoryManager>();
            PlayerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            PlayerStatsManager = GetComponent<PlayerStatsManager>();
            PlayerSoundFXManager = GetComponent<PlayerSoundFXManager>();
        }

        protected override void Start()
        {
            base.Start();

            PlayerInputManager.Instance.PlayerManager = this;
            PlayerCamera.Instance.PlayerManager = this;
            WorldSaveGameManager.Instance.player = this;

            // TODO: Think of something better
            PlayerStatsManager.SetMaxStamina(100);
            PlayerStatsManager.SetMaxHealth(100);
        }

        public void SaveGame(ref GameSaveData gameSaveData)
        {
            gameSaveData.xPosition = transform.position.x;
            gameSaveData.yPosition = transform.position.y;
            gameSaveData.zPosition = transform.position.z;
        }

        public void LoadGame(ref GameSaveData gameSaveData)
        {
            Vector3 myPosition = new Vector3(
                gameSaveData.xPosition, 
                gameSaveData.yPosition, 
                gameSaveData.zPosition
            );
            
            if (TryGetComponent<CharacterController>(out var controller))
            {
                controller.enabled = false;
                transform.position = myPosition;
                controller.enabled = true;
            }
            else
            {
                transform.position = myPosition;
            }
        }
    }
}