package mono.com.facebook.ads.internal.ssp;


public class ANAdRenderer_ListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.facebook.ads.internal.ssp.ANAdRenderer.Listener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onAdClick:()V:GetOnAdClickHandler:Xamarin.Facebook.Ads.Internal.Ssp.ANAdRenderer/IListenerInvoker, Xamarin.Facebook.AudienceNetwork\n" +
			"n_onAdClose:()V:GetOnAdCloseHandler:Xamarin.Facebook.Ads.Internal.Ssp.ANAdRenderer/IListenerInvoker, Xamarin.Facebook.AudienceNetwork\n" +
			"n_onAdError:(Ljava/lang/Throwable;)V:GetOnAdError_Ljava_lang_Throwable_Handler:Xamarin.Facebook.Ads.Internal.Ssp.ANAdRenderer/IListenerInvoker, Xamarin.Facebook.AudienceNetwork\n" +
			"n_onAdImpression:()V:GetOnAdImpressionHandler:Xamarin.Facebook.Ads.Internal.Ssp.ANAdRenderer/IListenerInvoker, Xamarin.Facebook.AudienceNetwork\n" +
			"";
		mono.android.Runtime.register ("Xamarin.Facebook.Ads.Internal.Ssp.ANAdRenderer+IListenerImplementor, Xamarin.Facebook.AudienceNetwork, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ANAdRenderer_ListenerImplementor.class, __md_methods);
	}


	public ANAdRenderer_ListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ANAdRenderer_ListenerImplementor.class)
			mono.android.TypeManager.Activate ("Xamarin.Facebook.Ads.Internal.Ssp.ANAdRenderer+IListenerImplementor, Xamarin.Facebook.AudienceNetwork, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onAdClick ()
	{
		n_onAdClick ();
	}

	private native void n_onAdClick ();


	public void onAdClose ()
	{
		n_onAdClose ();
	}

	private native void n_onAdClose ();


	public void onAdError (java.lang.Throwable p0)
	{
		n_onAdError (p0);
	}

	private native void n_onAdError (java.lang.Throwable p0);


	public void onAdImpression ()
	{
		n_onAdImpression ();
	}

	private native void n_onAdImpression ();

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
