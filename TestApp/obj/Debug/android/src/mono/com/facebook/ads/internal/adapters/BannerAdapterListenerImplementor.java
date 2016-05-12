package mono.com.facebook.ads.internal.adapters;


public class BannerAdapterListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.facebook.ads.internal.adapters.BannerAdapterListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onBannerAdClicked:(Lcom/facebook/ads/internal/adapters/BannerAdapter;)V:GetOnBannerAdClicked_Lcom_facebook_ads_internal_adapters_BannerAdapter_Handler:Xamarin.Facebook.Ads.Internal.Adapters.IBannerAdapterListenerInvoker, Xamarin.Facebook.AudienceNetwork\n" +
			"n_onBannerAdExpanded:(Lcom/facebook/ads/internal/adapters/BannerAdapter;)V:GetOnBannerAdExpanded_Lcom_facebook_ads_internal_adapters_BannerAdapter_Handler:Xamarin.Facebook.Ads.Internal.Adapters.IBannerAdapterListenerInvoker, Xamarin.Facebook.AudienceNetwork\n" +
			"n_onBannerAdLoaded:(Lcom/facebook/ads/internal/adapters/BannerAdapter;Landroid/view/View;)V:GetOnBannerAdLoaded_Lcom_facebook_ads_internal_adapters_BannerAdapter_Landroid_view_View_Handler:Xamarin.Facebook.Ads.Internal.Adapters.IBannerAdapterListenerInvoker, Xamarin.Facebook.AudienceNetwork\n" +
			"n_onBannerAdMinimized:(Lcom/facebook/ads/internal/adapters/BannerAdapter;)V:GetOnBannerAdMinimized_Lcom_facebook_ads_internal_adapters_BannerAdapter_Handler:Xamarin.Facebook.Ads.Internal.Adapters.IBannerAdapterListenerInvoker, Xamarin.Facebook.AudienceNetwork\n" +
			"n_onBannerError:(Lcom/facebook/ads/internal/adapters/BannerAdapter;Lcom/facebook/ads/AdError;)V:GetOnBannerError_Lcom_facebook_ads_internal_adapters_BannerAdapter_Lcom_facebook_ads_AdError_Handler:Xamarin.Facebook.Ads.Internal.Adapters.IBannerAdapterListenerInvoker, Xamarin.Facebook.AudienceNetwork\n" +
			"n_onBannerLoggingImpression:(Lcom/facebook/ads/internal/adapters/BannerAdapter;)V:GetOnBannerLoggingImpression_Lcom_facebook_ads_internal_adapters_BannerAdapter_Handler:Xamarin.Facebook.Ads.Internal.Adapters.IBannerAdapterListenerInvoker, Xamarin.Facebook.AudienceNetwork\n" +
			"";
		mono.android.Runtime.register ("Xamarin.Facebook.Ads.Internal.Adapters.IBannerAdapterListenerImplementor, Xamarin.Facebook.AudienceNetwork, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", BannerAdapterListenerImplementor.class, __md_methods);
	}


	public BannerAdapterListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == BannerAdapterListenerImplementor.class)
			mono.android.TypeManager.Activate ("Xamarin.Facebook.Ads.Internal.Adapters.IBannerAdapterListenerImplementor, Xamarin.Facebook.AudienceNetwork, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onBannerAdClicked (com.facebook.ads.internal.adapters.BannerAdapter p0)
	{
		n_onBannerAdClicked (p0);
	}

	private native void n_onBannerAdClicked (com.facebook.ads.internal.adapters.BannerAdapter p0);


	public void onBannerAdExpanded (com.facebook.ads.internal.adapters.BannerAdapter p0)
	{
		n_onBannerAdExpanded (p0);
	}

	private native void n_onBannerAdExpanded (com.facebook.ads.internal.adapters.BannerAdapter p0);


	public void onBannerAdLoaded (com.facebook.ads.internal.adapters.BannerAdapter p0, android.view.View p1)
	{
		n_onBannerAdLoaded (p0, p1);
	}

	private native void n_onBannerAdLoaded (com.facebook.ads.internal.adapters.BannerAdapter p0, android.view.View p1);


	public void onBannerAdMinimized (com.facebook.ads.internal.adapters.BannerAdapter p0)
	{
		n_onBannerAdMinimized (p0);
	}

	private native void n_onBannerAdMinimized (com.facebook.ads.internal.adapters.BannerAdapter p0);


	public void onBannerError (com.facebook.ads.internal.adapters.BannerAdapter p0, com.facebook.ads.AdError p1)
	{
		n_onBannerError (p0, p1);
	}

	private native void n_onBannerError (com.facebook.ads.internal.adapters.BannerAdapter p0, com.facebook.ads.AdError p1);


	public void onBannerLoggingImpression (com.facebook.ads.internal.adapters.BannerAdapter p0)
	{
		n_onBannerLoggingImpression (p0);
	}

	private native void n_onBannerLoggingImpression (com.facebook.ads.internal.adapters.BannerAdapter p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
