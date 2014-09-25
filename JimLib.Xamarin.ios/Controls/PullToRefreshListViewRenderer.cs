﻿using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(PullToRefreshListView), typeof(PullToRefreshListViewRenderer))]

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
	public class PullToRefreshListViewRenderer : ListViewRenderer
	{
	    private FormsUIRefreshControl _refreshControl;
	
		protected override void OnElementChanged (ElementChangedEventArgs<ListView> e)
		{
			base.OnElementChanged (e);

			if (_refreshControl != null)
				return;

			var pullToRefreshListView = (PullToRefreshListView)Element; 

			_refreshControl = new FormsUIRefreshControl
			{
			    RefreshCommand = pullToRefreshListView.RefreshCommand, 
                Message = pullToRefreshListView.Message
			};
		    Control.AddSubview (_refreshControl);
		}

	    /// <summary>
	    /// Raises the element property changed event.
	    /// </summary>
	    /// <param name="sender">Sender.</param>
	    /// <param name="e">E.</param>
	    protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
	    {
	        base.OnElementPropertyChanged(sender, e);
	        var pullToRefreshListView = Element as PullToRefreshListView;
	        if (pullToRefreshListView == null)
	            return;

	        if (e.PropertyName == PullToRefreshListView.IsRefreshingProperty.PropertyName)
	            _refreshControl.IsRefreshing = pullToRefreshListView.IsRefreshing;
	        else if (e.PropertyName == PullToRefreshListView.MessageProperty.PropertyName)
	            _refreshControl.Message = pullToRefreshListView.Message;
	        else if (e.PropertyName == PullToRefreshListView.RefreshCommandProperty.PropertyName)
	            _refreshControl.RefreshCommand = pullToRefreshListView.RefreshCommand;
	    }
	}
}
