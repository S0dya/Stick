using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager instance;
    static IStoreController m_StoreController;
    static IExtensionProvider m_StoreExtensionProvider;

    string[] donates = new string[] { "donate1", "donate2", "donate3", "donate4", "donate5" };


    public void InitializePurchasing()
    {
        if (IsInitialized()) 
        { 
            return; 
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        foreach (var d in donates)
        {
            builder.AddProduct(d, ProductType.NonConsumable);
        }

        UnityPurchasing.Initialize(this, builder);
    }
    bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void OnBuyDonate(int i)
    {
        BuyProductID(donates[i]);
    }

    public void ClickRestorePurchaseButton()
    {
        IAPManager.instance.RestorePurchases();
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        bool found = false;
        foreach (var d in donates)
        {
            if (String.Equals(args.purchasedProduct.definition.id, d, StringComparison.Ordinal))
            {
                SSTools.ShowMessage("Remove Ads Successful", SSTools.Position.bottom, SSTools.Time.threeSecond);
                found = true;
                break;
            }
        }
        if (!found)
        {
            SSTools.ShowMessage("Purchase Failed", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
        return PurchaseProcessingResult.Complete;
    }

    
    
    
    
    
    
    
    void Awake()
    {
        TestSingleton();

    }

    void Start()
    {
        if (m_StoreController == null) 
        { 
            InitializePurchasing(); 
        }
    }

     void TestSingleton()
    {
        if (instance != null) 
        { 
            Destroy(gameObject); 
            return; 
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                SSTools.ShowMessage("BuyProductID: FAIL. Not purchasing product", SSTools.Position.bottom, SSTools.Time.threeSecond);
            }
        }
        else
        {
            SSTools.ShowMessage("BuyProductID FAIL. Not initialized.", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                SSTools.ShowMessage("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.", SSTools.Position.bottom, SSTools.Time.threeSecond);
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            SSTools.ShowMessage("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform, SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string str)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error + str);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}