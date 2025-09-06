using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Essentials;
using Android.Content.Res;
using Java.IO;
using System.Security.Cryptography;
using Java.Lang;
using String = System.String;
using Android.Runtime;
using Android.Widget;

namespace CERS.Droid
{
    [Activity( Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

#if DEBUG
            Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);          
            LoadApplication(new App());
#else
        /* var signature1 = Sig_Hash();          
                    if (signature1 != appsignaturerelease)
                    {
                        showdialog("Signature Key Not matched");
                    }

            if (!RootUtil.IsDeviceRooted())
            {
                Platform.Init(this, savedInstanceState);
                Forms.Init(this, savedInstanceState);               
                LoadApplication(new App());
            }
            else
            {
                Toast.MakeText(ApplicationContext, "Your Device is Rooted. App cannot be used on Rooted Devices.", ToastLength.Long).Show();
                FinishAffinity();
            }*/

            Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);          
            LoadApplication(new App());
#endif
        }
        public string Sig_Hash()
        {
            var Context = Android.App.Application.Context;

            foreach (Signature signature in Context.PackageManager.GetPackageInfo(Context.PackageName, PackageInfoFlags.Signatures).Signatures)
            {
                using (SHA256Managed sha256 = new SHA256Managed())
                {
                    var hash = sha256.ComputeHash(signature.ToByteArray());
                    var sb = new StringBuilder(hash.Length * 2);
                    foreach (byte b in hash)
                    {
                        sb.Append(b.ToString("X2"));
                    }
                    return sb.ToString();
                }

            }
            return "";
        }
        void showdialog(string message)
        {
            AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);
            alertDiag.SetTitle("signature");
            alertDiag.SetMessage(message);
            alertDiag.SetPositiveButton("OK", (senderAlert, args) =>
            {
                alertDiag.Dispose();
                FinishAffinity();
            });
            //alertDiag.SetNegativeButton("Cancel", (senderAlert, args) => {
            //    alertDiag.Dispose();
            //});
            Dialog diag = alertDiag.Create();
            diag.Show();

        }

        public override Resources Resources
        {
            get
            {
                Resources res = base.Resources;
                Configuration config = new Configuration();
                config.SetToDefaults();
                res.UpdateConfiguration(config, res.DisplayMetrics);
                return res;
            }
        }

        public class RootUtil
        {
            public static bool IsDeviceRooted()
            {
                return checkRootMethod1() || checkRootMethod2() || checkRootMethod3();
            }

            private static bool checkRootMethod1()
            {
                String buildTags = Android.OS.Build.Tags;
                return buildTags != null && buildTags.Contains("test-keys");
            }

            private static bool checkRootMethod2()
            {
                string[] paths = { "/system/app/Superuser.apk", "/sbin/su", "/system/bin/su", "/system/xbin/su", "/data/local/xbin/su", "/data/local/bin/su", "/system/sd/xbin/su",
                "/system/bin/failsafe/su", "/data/local/su", "/su/bin/su"};
                foreach (string path in paths)
                {
                    if (new File(path).Exists()) return true;
                }
                return false;
            }

            private static bool checkRootMethod3()
            {
                Java.Lang.Process process = null;
                try
                {
                    process = Runtime.GetRuntime().Exec(new string[] { "/system/xbin/which", "su" });
                    BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(process.InputStream));
                    if (bufferedReader.ReadLine() != null) return true;
                    return false;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (process != null) process.Destroy();
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}