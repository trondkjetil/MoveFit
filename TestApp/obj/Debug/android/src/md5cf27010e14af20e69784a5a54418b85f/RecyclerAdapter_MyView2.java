package md5cf27010e14af20e69784a5a54418b85f;


public class RecyclerAdapter_MyView2
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("TestApp.RecyclerAdapter+MyView2, TestApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", RecyclerAdapter_MyView2.class, __md_methods);
	}


	public RecyclerAdapter_MyView2 (android.view.View p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == RecyclerAdapter_MyView2.class)
			mono.android.TypeManager.Activate ("TestApp.RecyclerAdapter+MyView2, TestApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Views.View, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}

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
