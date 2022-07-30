using System;
using System.Collections.Generic;
using Code.Utils;
using PlayFab;
using PlayFab.ClientModels;

namespace Code.Managers
{
    public class InventoryManager
    {
        public event Action<List<ItemInstance>> OnInventoryUpdate; 

        /// <summary>
        /// Отправляет запрос получения инвентаря
        /// </summary>
        public void GetInventory()
        {
            var getUserInventoryRequest = new GetUserInventoryRequest();
            PlayFabClientAPI.GetUserInventory(getUserInventoryRequest, OnGetInventorySuccess, OnGetInventoryFailed);
        }
        
        /// <summary>
        /// Покупает предмет
        /// </summary>
        /// <param name="itemId"></param>
        public void PurchaseItem(string itemId)
        {
            var consumeItemRequest = new PurchaseItemRequest()
            {
                CatalogVersion = "MainCatalog",
                ItemId = itemId,
            };

            PlayFabClientAPI.PurchaseItem(consumeItemRequest, OnPurchaseItemSuccess, OnPurchaseItemFailed);
        }

        /// <summary>
        /// Удаляет предмет из инвентаря
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        public void RemoveItem(string itemId, int count)
        {
            var consumeItemRequest = new ConsumeItemRequest()
            {
                ConsumeCount = count,
                ItemInstanceId = itemId
            };

            PlayFabClientAPI.ConsumeItem(consumeItemRequest, OnRemoveItemSuccess, OnRemoveItemFailed);
        }

        #region PurchaseItem Callbacks

        private void OnPurchaseItemSuccess(PurchaseItemResult result)
        {
            DLogger.Debug(GetType(), nameof(OnPurchaseItemSuccess), 
                "PurchaseItem Success");
            
            GetInventory();
        }

        private void OnPurchaseItemFailed(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            DLogger.Error(GetType(), nameof(OnRemoveItemFailed), 
                $"PurchaseItem Failed: {errorMessage}");
        }

        #endregion
        
        #region RemoveItem Callbacks

        private void OnRemoveItemSuccess(ConsumeItemResult obj)
        {
            DLogger.Debug(GetType(), nameof(OnRemoveItemSuccess), 
                $"RemoveItem Success");
            
            GetInventory();
        }
        
        private void OnRemoveItemFailed(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            DLogger.Error(GetType(), nameof(OnRemoveItemFailed), 
                $"RemoveItem Failed: {errorMessage}");
        }

        #endregion

        #region GetInventory Callbacks

        private void OnGetInventorySuccess(GetUserInventoryResult result)
        {
            var items = result.Inventory;
            OnInventoryUpdate?.Invoke(items);
            
            DLogger.Debug(GetType(), nameof(OnGetInventorySuccess), 
                $"GetInventory Success");
        }

        private void OnGetInventoryFailed(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            DLogger.Error(GetType(), nameof(OnGetInventoryFailed), 
                $"GetInventory Failed: {errorMessage}");
        }
        
        #endregion
    }
}