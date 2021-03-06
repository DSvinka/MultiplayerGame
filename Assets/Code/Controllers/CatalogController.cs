using System.Collections.Generic;
using Code.Utils;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Code.Controllers
{
    public class CatalogController: MonoBehaviour
    {
        #region Unity Events
        
        private void Start()
        {
            var request = new GetCatalogItemsRequest();
            PlayFabClientAPI.GetCatalogItems(request, OnGetCatalog, OnGetCatalogError);
        }
        
        #endregion
        
        #region Show Catalog
        
        private void ShowCatalog(List<CatalogItem> items)
        {
            foreach (var item in items)
            {
                DLogger.Debug(GetType(), nameof(ShowCatalog), 
                    $"Item: {item.ItemId}"); 
            }
        }

        private void OnGetCatalog(GetCatalogItemsResult result)
        {
            ShowCatalog(result.Catalog);
            
            DLogger.Debug(GetType(), nameof(OnGetCatalog), 
                $"GetCatalog Complete!");
        }

        private void OnGetCatalogError(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            DLogger.Error(GetType(), nameof(OnGetCatalogError), 
                $"GetCatalog Failed: {errorMessage}");
        }
        
        #endregion
    }
}