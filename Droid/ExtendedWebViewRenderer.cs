using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Android;
using Android.Content;
using Android.Webkit;
using Android.App;

using LocationWebView;
using LocationWebView.Droid;

[assembly: ExportRenderer(typeof(ExtendedWebView), typeof(ExtendedWebViewRenderer))]
namespace LocationWebView.Droid
{
    public class ExtendedWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

			if (e.OldElement != null || Element == null)
			{
				return;
			}

            Control.Settings.JavaScriptEnabled = true;
			Control.Settings.SetGeolocationEnabled(true);
            Control.Settings.SetGeolocationDatabasePath(Control.Context.FilesDir.Path);

			Control.SetWebViewClient(new CustomWebViewClient());
			Control.SetWebChromeClient(new CustomChromeClient(Control.Context));
        }
    }

	public class CustomWebViewClient : WebViewClient
    {
		public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, string url)
        {
			view.LoadUrl(url);
			return true;
        }
	}

	public class CustomChromeClient : WebChromeClient
    {
		private readonly Context _context;

		public CustomChromeClient(Context context)
		{
			_context = context;
		}

		public override void OnGeolocationPermissionsShowPrompt(string origin, GeolocationPermissions.ICallback callback)
		{
			const bool remember = false;
			var builder = new AlertDialog.Builder(_context);
			builder.SetTitle("Location")
				.SetMessage(string.Format("{0} would like to use your current location", origin))
				.SetPositiveButton("Allow", (sender, args) => callback.Invoke(origin, true, remember))
				.SetNegativeButton("Disallow", (sender, args) => callback.Invoke(origin, false, remember));
			var alert = builder.Create();
			alert.Show();
		}
	}
}
