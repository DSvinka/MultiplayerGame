using System;
using System.Collections.Generic;
using Code.Utils;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Code
{
    public class CatalogManager: MonoBehaviour
    {
        private void Start()
        {
            var request = new GetCatalogItemsRequest();
            PlayFabClientAPI.GetCatalogItems(request, OnGetCatalog, OnGetCatalogError);
        }
        
        private void ShowCatalog(List<CatalogItem> items)
        {
            foreach (var item in items)
            {
                DLogger.Debug(nameof(CatalogManager), nameof(ShowCatalog), 
                    $"Item: {item.ItemId}"); 
            }
        }

        private void OnGetCatalog(GetCatalogItemsResult result)
        {
            ShowCatalog(result.Catalog);
            
            DLogger.Debug(nameof(CatalogManager), nameof(OnGetCatalog), 
                $"GetCatalog Complete!");
        }

        private void OnGetCatalogError(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            DLogger.Error(nameof(CatalogManager), nameof(OnGetCatalogError), 
                $"GetCatalog Failed: {errorMessage}");
        }
    }
}