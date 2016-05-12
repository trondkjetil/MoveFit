package mono.com.facebook.ads.internal.adapters;


public class InterstitialAdapterListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.facebook.ads.internal.adapters.InterstitialAdapterListener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onInterstitialAdClicked:(Lcom/facebook/ads/internal/adapters/InterstitialAdapter;Ljava/lang/String;Z)V:GetOnInterstitialAdClicked_Lcom_facebook_ads_internal_adapters_InterstitialAdapter_Ljava_lang_String_ZHandler:Xamarin.Facebook.Ads.Internal.Adapters.IInterstitialAdapterListenerInvoker, Xamarin.Facebook.AudienceNetwork\n" +
			"n_onInterstitialAdDismissed:(Lcom/facebook/ads/internal/adapters/InterstitialAdapter;)V:GetOnInterstitialAdDismissed_Lcom_facebook_ads_internal_adapters_InterstitialAdapter_Handler:Xamarin.Facebook.Ads.Internal.Adapters.IInterstitialAdapterListenerInvoker, Xamarin.Facebook.AudienceNetwork\n" +
			"n_onInterstitialAdDisplayed:(Lcom/facebook/ads/internal/adapters/InterstitialAdapter;)V:GetOnInterstitialAdDisplayed_Lcom_facebook_ads_internal_adapters_InterstitialAdapter_Handler:Xamarin.Facebook.Ads.Internal.Adapters.IInterstitialAdapterListenerInvoker, Xamarin.Facebook.AudienceNetwork\n" +
			"n_onInterstitialAdLoaded:(Lcom/facebook/ads/internal/adapters/InterstitialAdapter;)V:GetOnInterstitialAdLoaded_Lcom_facebook_ads_internal_adapters_InterstitialAdapter_Handler:Xamarin.Facebook.Ads.Internal.Adapters.IInterstitialAdapterListenerInvoker, Xamarin.Facebook.AudienceNetwork\n" +
			"n_onInterstitialError:(Lcom/facebook/ads/internal/adapters/InterstitialAdapter;Lcom/facebook/ads/AdError;)V:GetOnInterstitialError_Lcom_facebook_ads_internal_adapters_InterstitialAdapter_Lcom_facebook_ads_AdError_Handler:Xamarin.Facebook.Ads.Internal.Adapters.IInterstitialAdapterListenerInvoker, Xamarin.Facebook.AudienceNetwork\n" +
			"n_onInterstitialLoggingImpression:(Lcom/facebook/ads/internal/adapters/InterstitialAdapter;)V:GetOnInterstitialLoggingImpression_Lcom_facebook_ads_internal_adapters_InterstitialAdapter_Handler:Xamarin.Facebook.Ads.Internal.Adapters.IInterstitialAdapterListenerInvoker, Xamarin.Facebook.AudienceNetwork\n" +
			"";
		mono.android.Runtime.register ("Xamarin.Facebook.Ads.Internal.Adapters.IInterstitialAdapterListenerImplementor, Xamarin.Facebook.AudienceNetwork, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", InterstitialAdapterListenerImplementor.class, __md_methods);
	}


	public InterstitialAdapterListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == InterstitialAdapterListenerImplementor.class)
			mono.android.TypeManager.Activate ("Xamarin.Facebook.Ads.Internal.Adapters.IInterstitialAdapterListenerImplementor, Xamarin.Facebook.AudienceNetwork, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onInterstitialAdClicked (com.facebook.ads.internal.adapters.InterstitialAdapter p0, java.lang.String p1, boolean p2)
	{
		n_onInterstitialAdClicked (p0, p1, p2);
	}

	private native void n_onInterstitialAdClicked (com.facebook.ads.internal.adapters.InterstitialAdapter p0, java.lang.String p1, boolean p2);


	public void onInterstitialAdDismissed (com.facebook.ads.internal.adapters.InterstitialAdapter p0)
	{
		n_onInterstitialAdDismissed (p0);
	}

	private native void n_onInterstitialAdDismissed (com.facebook.ads.internal.adapters.InterstitialAdapter p0);


	public void onInterstitialAdDisplayed (com.facebook.ads.internal.adapters.InterstitialAdapter p0)
	{
		n_onInterstitialAdDisplayed (p0);
	}

	private native void n_onInterstitialAdDisplayed (com.facebook.ads.internal.adapters.InterstitialAdapter p0);


	public void onInterstitialAdLoaded (com.facebook.ads.internal.adapters.InterstitialAdapter p0)
	{
		n_onInterstitialAdLoaded (p0);
	}

	private native void n_onInterstitialAdLoaded (com.facebook.ads.internal.adapters.InterstitialAdapter p0);


	public void onInterstitialError (com.facebook.ads.internal.adapters.InterstitialAdapter p0, com.facebook.ads.AdError p1)
	{
		n_onInterstitialError (p0, p1);
	}

	private native void n_onInterstitialError (com.facebook.ads.internal.adapters.InterstitialAdapter p0, com.facebook.ads.AdError p1);


	public void onInterstitialLoggingImpression (com.facebook.ads.internal.adapters.InterstitialAdapter p0)
	{
		n_onInterstitialLoggingImpression (p0);
	}

	private native void n_onInterstitialLoggingImpression (com.facebook.ads.internal.adapters.InterstitialAdapter p0);

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
