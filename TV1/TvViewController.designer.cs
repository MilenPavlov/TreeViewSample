// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TV1
{
	[Register ("TvViewController")]
	partial class TvViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIScrollView ScrollView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView TableView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ScrollView != null) {
				ScrollView.Dispose ();
				ScrollView = null;
			}
			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}
		}
	}
}
