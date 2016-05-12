
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TestApp
{
	[Activity (Label = "ResultBoardActivity")]			
	public class ResultBoardActivity : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.customListView);

			//Setup custom adapter
			// Initiates the adapter class that will provide test data for an adapter that displays a user in
			//List view
			var contactsAdapter = new ContactsAdapter (this);
			var contactsListView = FindViewById<ListView> (Resource.Id.ContactsListView);
			contactsListView.Adapter = contactsAdapter;


		}
	}
}

