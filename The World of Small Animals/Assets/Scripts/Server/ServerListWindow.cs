using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Server
{
    public class ServerListWindow : MonoBehaviour
    {
        private const string PATH_PREFAB_BUTTON_SELECT_SERVER = "Prefabs/UI/buttonServerSlot";

        private const string PATH_DATA_SLOTS_SERVERS = "Data/ServerSlots";

        [Header("Задержка перед появлением списка кнопок выбора сервера")]
        [SerializeField] float timeoutSpawnButtons = 2.1f;

        [Header("Задержка перед появлением кнопки выбора сервера")]
        [SerializeField] float timeoutSpawnButton = 0.5f;

        [Header("Контент для кнопок")]
        [SerializeField] Transform contentButtons;

        private ServerSlotData[] slots;

        private ServerSlot slotServerPrefab;
        // Use this for initialization
        void Start()
        {
            if (timeoutSpawnButton <= 0)
            {
                throw new ServerListWindowException("time out spawn button not valid");
            }

            if (timeoutSpawnButtons <= 0)
            {
                throw new ServerListWindowException("time out spawn buttons not valid");
            }

            slotServerPrefab = Resources.Load<ServerSlot>(PATH_PREFAB_BUTTON_SELECT_SERVER);

            if (slotServerPrefab == null)
            {
                throw new ServerListWindowException("slot server prefab not found");
            }

            slots = Resources.LoadAll<ServerSlotData>(PATH_DATA_SLOTS_SERVERS);

            if (slots.Length == 0)
            {
                throw new ServerListWindowException("slots not found");
            }

#if UNITY_EDITOR
            ShoeServerSlots(serverUsers => serverUsers.DevelopServer);
#endif


            ShoeServerSlots(serverUsers => serverUsers.DevelopServer == false);
        }

        private void ShoeServerSlots (Func<ServerSlotData, bool> predicate)
        {
            ServerSlotData[] array = slots.Where(predicate).ToArray();

            StartCoroutine(CreatingAsyncSlots(array));
        }

        private IEnumerator CreatingAsyncSlots (ServerSlotData[] slots)
        {
              yield return new WaitForSeconds(timeoutSpawnButtons);

            int i = 0;


            while (i < slots.Length)
            {
                yield return new WaitForSeconds(timeoutSpawnButton);
                ServerSlot slot = Instantiate(slotServerPrefab, contentButtons);
                slot.SetData(slots[i]);
                slot.onClick += SelectServer;
                i++;
                yield return null;
            }
        }

        private void SelectServer(ServerSlotData data)
        {
           
        }
    }
}
