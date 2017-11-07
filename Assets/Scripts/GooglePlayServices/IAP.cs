using Assets.Scripts;
using Assets.Scripts.GooglePlayServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAP : MonoBehaviour, IStoreListener
{

    public static IAP Instance { get; set; }

    public IAPListener iapListener;
    private static IStoreController storeController;
    private static IExtensionProvider storeExtensionProvider;

    public List<string> consumableProducts;
    public List<string> nonConsumableProducts;

    private void Start()
    {
        Instance = this;
       
        if (storeController == null)
        {
            InitializedPurchasing();
        }
    }
   
    //IAP Servislerini başlatır.
    private void InitializedPurchasing()
    {
        if (IsInitialized())
            return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Consumable ve NonConsumable olan ürünleri IAP Servisine ekler.
        foreach (string item in consumableProducts)
        {
            builder.AddProduct(item, ProductType.Consumable);
        }

        foreach (string item in nonConsumableProducts)
        {
            builder.AddProduct(item, ProductType.NonConsumable);
        }

        UnityPurchasing.Initialize(this, builder);
    }

    //IAP servisinin başlatılıp başlatılmadığı bilgisini döndürür.
    //Eğer IAP çalışıyorsa true değeri dönecektir.
    private bool IsInitialized()
    {
        return storeController != null && storeExtensionProvider != null;
    }

    //Ürün satın alma işlemini yapar.
    public void BuyProduct(string productId)
    {
        BuyProductID(productId);
    }

    //Ürün satın alma işlemini yapar.
    private void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            //Gelen parametre(productId) değerine göre ürün bilgilerini alır.
            Product product = storeController.products.WithID(productId);

            //Eğer product boş değilse ve ürün satılmaya hazırsa
            //storeController ile ürün satış işlemleri başlatılır.
            if (product != null && product.availableToPurchase)
            {
                storeController.InitiatePurchase(product);
            }
            else
            {
                //Ya productId ye ait bir ürün yok ya da ürün henüz satılmaya hazır değil.
                //Bunun anlamı siz bir ürün oluşturmuş olabilirsiniz ama ürünü yayınlamadıysanız veya
                //yayınladığınız ürün güncellemesi mağaza tarafından henüz kullanıma konulmadıysa   
                //product.availableToPurchase değeri false dönecektir. Bu yüzdende ürün satışı başlamayacaktır.
                Debug.Log("HATA! Ürün satışı başlatılamadı.");
            }
        }
        else
        {
            Debug.Log("HATA! IAP Servisi başlatılamadı.");
        }
    }

    //Ürünün her ülke için fiyatını para birimi simgesi ile verir.
    //Örneğin; Türkiye'den erişim için 1₺ 
    //         ABD'den erişim için 1$
    public string GetProductPrice(string productId)
    {
        Product tmpProduct = storeController.products.WithID(productId);
        return tmpProduct.metadata.localizedPriceString;
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Tüm IAP servislerinin yapılandırılmasnı sağlar.
        storeController = controller;
        storeExtensionProvider = extensions;

        Debug.Log("IAP servisleri başlatıldı.");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("HATA! IAP Servisleri başlatılamadı. " + error);
    }

    //Ürün satışı başarısız olduğu zaman çalışır.
    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        if (iapListener != null)
            iapListener.PurchaseStatus(i.definition.id, false);

    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        foreach (string item in consumableProducts)
        {
            if (String.Equals(e.purchasedProduct.definition.id, item, StringComparison.Ordinal))
            {
                if (iapListener != null)
                    iapListener.PurchaseStatus(item, true);
            }
        }

        foreach (string item in nonConsumableProducts)
        {
            if (String.Equals(e.purchasedProduct.definition.id, item, StringComparison.Ordinal))
            {
                if (iapListener != null)
                    iapListener.PurchaseStatus(item, true);
            }
        }

        return PurchaseProcessingResult.Complete;
    }
}
