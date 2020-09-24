using Android.App;
using Android.Content.PM;
using Android.OS;
using Prism;
using Prism.Ioc;
using System.Net.Sockets;
using XamarinPrismQrGen.ViewModels;

namespace XamarinPrismQrGen.Droid
{
    [Activity(Theme = "@style/MainTheme",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            //Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            //global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            LoadApplication(new App(new AndroidInitializer()));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        //public override void OnBackPressed()
        //{
        //    Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
            
        //    builder.SetPositiveButton("확인", (senderAlert, args) => {
        //        LoginViewModel.tcpClient.Close();
        //        Finish();
        //    });

        //    builder.SetNegativeButton("취소", (senderAlert, args) => {
        //        return;
        //    });

        //    Android.App.AlertDialog alterDialog = builder.Create();
        //    alterDialog.SetTitle("알림");
        //    alterDialog.SetMessage("프로그램을 종료하시겠습니까?");
        //    alterDialog.Show();
        //}
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

