using Android.Content;
using Android.Graphics;
using Android.Views;
using BookApp.Ndro.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomRenderer))]
namespace BookApp.Ndro.Droid
{
    public class CustomRenderer : NavigationPageRenderer, TextureView.ISurfaceTextureListener
    {
        public CustomRenderer(Context context) : base(context)
        {
        }

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            return true;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
            
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
        {
            base.OnElementChanged(e);
        }
    }
}