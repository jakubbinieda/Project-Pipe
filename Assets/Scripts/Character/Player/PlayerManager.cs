using UnityEngine;
using System.Collections;

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

        public override IEnumerator ProcessDeathEvent()
        {
            PlayerUIManager.Instance.PlayerUIPopUpManager.SendYouDiedPopUp();

            yield return base.ProcessDeathEvent();

            yield return new WaitForSeconds(3f);

            WorldSaveGameManager.Instance.BackToMainMenu();
        }

        public void SaveGame(ref GameSaveData gameSaveData)
        {
            gameSaveData.xPosition = transform.position.x;
            gameSaveData.yPosition = transform.position.y;
            gameSaveData.zPosition = transform.position.z;

            gameSaveData.xRotation = transform.rotation.x;
            gameSaveData.yRotation = transform.rotation.y;
            gameSaveData.zRotation = transform.rotation.z;
            gameSaveData.wRotation = transform.rotation.w;

            gameSaveData.currentHealth = PlayerStatsManager.CurrentHealth;
            gameSaveData.currentStamina = PlayerStatsManager.CurrentStamina;
        }

        public void LoadGame(ref GameSaveData gameSaveData, bool isNewGame)
        {
            Vector3 position = new Vector3(
                gameSaveData.xPosition,
                gameSaveData.yPosition,
                gameSaveData.zPosition
            );

            Quaternion rotation = new Quaternion(
                gameSaveData.xRotation,
                gameSaveData.yRotation,
                gameSaveData.zRotation,
                gameSaveData.wRotation
            );

            if (TryGetComponent<CharacterController>(out var controller))
            {
                controller.enabled = false;
                transform.SetPositionAndRotation(position, rotation);
                controller.enabled = true;
            }
            else
            {
                transform.SetPositionAndRotation(position, rotation);
            }

            if (!isNewGame)
            {
                PlayerStatsManager.CurrentHealth = gameSaveData.currentHealth;
                PlayerStatsManager.CurrentStamina = gameSaveData.currentStamina;
            }
        }
    }
}