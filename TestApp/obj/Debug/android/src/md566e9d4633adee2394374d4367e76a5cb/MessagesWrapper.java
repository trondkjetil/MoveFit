package md566e9d4633adee2394374d4367e76a5cb;


public class MessagesWrapper
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("TestApp.MessagesWrapper, moveFit, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MessagesWrapper.class, __md_methods);
	}


	public MessagesWrapper () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MessagesWrapper.class)
			mono.android.TypeManager.Activate ("TestApp.MessagesWrapper, moveFit, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
