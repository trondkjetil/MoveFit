package md566e9d4633adee2394374d4367e76a5cb;


public class LocationServiceBinder
	extends android.os.Binder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("TestApp.LocationServiceBinder, moveFit, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", LocationServiceBinder.class, __md_methods);
	}


	public LocationServiceBinder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == LocationServiceBinder.class)
			mono.android.TypeManager.Activate ("TestApp.LocationServiceBinder, moveFit, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public LocationServiceBinder (md566e9d4633adee2394374d4367e76a5cb.LocationService p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == LocationServiceBinder.class)
			mono.android.TypeManager.Activate ("TestApp.LocationServiceBinder, moveFit, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "TestApp.LocationService, moveFit, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
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
