using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabEconomy : MonoBehaviour
{
    public void GetCatalogItems()
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest
        {
            CatalogVersion = "Test Catalog"
        }, result => {
            Debug.Log("Catalog Items:");
            foreach (var item in result.Catalog)
            {
                Debug.Log(item.DisplayName + " " + item.VirtualCurrencyPrices.First().Key + " " + item.VirtualCurrencyPrices.First().Value);
            }
        }, (error) => {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public void PurchaseItem()
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            CatalogVersion = "Test Catalog",
            ItemId = "apple",
            VirtualCurrency = "GD",
            Price = 5
        }, result => {

            Debug.Log("Purchase Success!");
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result2 =>
            {
                Debug.Log("Player Inventory:");
                foreach (var item in result2.Inventory)
                {
                    Debug.Log(item.DisplayName);
                }
            }, (error) => {
                Debug.Log(error.GenerateErrorReport());
            });

        }, (error) => {
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
