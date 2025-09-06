using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CERS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadWebViewPage : ContentPage
    {
        public LoadWebViewPage(string url)
        {
            InitializeComponent();
            lbl_navigation_header.Text = App.AppName;
            lbl_heading.Text = App.GetLabelByKey("PrivacyPolicy");
            Loading_activity.IsVisible = true;
            Loading_activity.IsVisible = false;
            webview_loaddata.Source = url;

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {           
            Application.Current.MainPage = new NavigationPage(new DashboardPage());          
        }
    }
}