using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;


namespace TestApp
{
			
	public class ContactsAdapter : BaseAdapter
	{
		// This class is in charge of creating a adapter that will fit the needs of a user
		// It will be able to display the users image aswell as info
		List<Contact> contactList;
		Activity activity;

		public ContactsAdapter (Activity activity)
		{
			this.activity = activity;
			FillContacts ();
		}

		void FillContacts ()
		{
			var uri = ContactsContract.Contacts.ContentUri;

			string[] projection = {
				ContactsContract.Contacts.InterfaceConsts.Id,
				ContactsContract.Contacts.InterfaceConsts.DisplayName,
				ContactsContract.Contacts.InterfaceConsts.PhotoId
			};

			var cursor = activity.ManagedQuery (uri, projection, null,
				null, null);

			contactList = new List<Contact> ();

			if (cursor.MoveToFirst ()) {
				do {
					contactList.Add (new Contact{
						Id = cursor.GetLong (
							cursor.GetColumnIndex (projection [0])),
						DisplayName = cursor.GetString (
							cursor.GetColumnIndex (projection [1])),
						PhotoId = cursor.GetString (
							cursor.GetColumnIndex (projection [2]))
					});
				} while (cursor.MoveToNext());
			}
				
		}


		public override int Count {
			get { return contactList.Count; }
		}

		public override Java.Lang.Object GetItem (int position) {
			// could wrap a Contact in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public override long GetItemId (int position) {
			return contactList [position].Id;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? activity.LayoutInflater.Inflate (Resource.Layout.resultList, parent, false);
			var contactName = view.FindViewById<TextView> (Resource.Id.ContactName);
			var contactImage = view.FindViewById<ImageView> (Resource.Id.ContactImage);
			contactName.Text = contactList [position].DisplayName;

			if (contactList [position].PhotoId == null) {
				contactImage = view.FindViewById<ImageView> (Resource.Id.ContactImage);
				contactImage.SetImageResource (Resource.Drawable.tt);
			

			}  else {
				var contactUri = ContentUris.WithAppendedId (
					ContactsContract.Contacts.ContentUri, contactList [position].Id);
			

				var contactPhotoUri = Android.Net.Uri.WithAppendedPath (contactUri,
					Contacts.Photos.ContentDirectory);


				contactImage.SetImageURI (contactPhotoUri);
			}
			return view;
		}



		 class Contact
		{
			public long Id { get; set; }
			public string DisplayName{ get; set; }
			public string PhotoId { get; set; }
		}
	}

}