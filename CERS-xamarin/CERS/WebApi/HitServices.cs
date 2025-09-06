using Newtonsoft.Json.Linq;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Web;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using CERS.Models;
using Newtonsoft.Json;

namespace CERS.WebApi
{
    public class HitServices
    {
        UserDetailsDatabase userDetailsDatabase = new UserDetailsDatabase();
        List<UserDetails> userDetailslist;
        LanguageMasterDatabase languageMasterDatabase = new LanguageMasterDatabase();
        ExpenseSourcesDatabase expenseSourcesDatabase = new ExpenseSourcesDatabase();
        PaymentModesDatabase paymentModesDatabase = new PaymentModesDatabase();
        ExpenditureDetailsDatabase expenditureDetailsDatabase = new ExpenditureDetailsDatabase();
        ObserverExpenditureDetailsDatabase observerExpenditureDetailsDatabase = new ObserverExpenditureDetailsDatabase();
        ObserverWardsDatabase observerWardsDatabase = new ObserverWardsDatabase();
        ObserverCandidatesDatabase observerCandidatesDatabase = new ObserverCandidatesDatabase();
        ObservorLoginDetailsDatabase observorLoginDetailsDatabase = new ObservorLoginDetailsDatabase();
        ViewAllRemarksDatabase viewAllRemarksDatabase = new ViewAllRemarksDatabase();

        /* public static string baseurl = "http://10.146.2.121/CERSWebApi/";
        public static string PrivacyPolicyUrl = "http://10.146.2.121/CERSWebApi/privacypolicy.aspx?";*/

        /*    public string baseurl = "https://sechimachal.nic.in/cerswebapi/";
            public string PrivacyPolicyUrl = "https://sechimachal.nic.in/cerswebapi/privacypolicy.aspx?";*/

        public  string baseurl = "http://10.146.2.8/CERSWebApi/";
        public  string PrivacyPolicyUrl = "http://10.146.2.8/CERSWebApi/privacypolicy.aspx?";

        /*  public async void AppVersion()l̥
          {
              var current = Connectivity.NetworkAccess;
              if (current == NetworkAccess.Internet)
              {
                  try
                  {
                      var client = new HttpClient();
                     // //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                     // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                      string _Platform = "A";
                      if (DevicePlatform.Android == DevicePlatform.Android)
                      {
                          _Platform = "A";
                      }
                      else if (DevicePlatform.iOS == DevicePlatform.iOS)
                      {
                          _Platform = "I";
                      }
                      double installedVersionNumber = double.Parse(VersionTracking.CurrentVersion);
                      double latestVersionNumber = installedVersionNumber;

                      string parameters = baseurl + $"api/AppVersion?" +
                      $"Platform={HttpUtility.UrlEncode(AESCryptography.EncryptAES(_Platform))}" +
                      $"&packageid={HttpUtility.UrlEncode(AESCryptography.EncryptAES(AppInfo.PackageName))}";

                      HttpResponseMessage response = await client.GetAsync(parameters);
                      var result = await response.Content.ReadAsStringAsync();
                      var parsed = JObject.Parse(result);

                      if ((int)response.StatusCode == 200)
                      {
                          if (result.Contains("Mandatory"))
                          {
                              var m = parsed["data"][0]["VersionNumber"].ToString();
                              latestVersionNumber = double.Parse(AESCryptography.DecryptAES(parsed["data"][0]["VersionNumber"].ToString()));
                              if (installedVersionNumber < latestVersionNumber)
                              {
                                  if (AESCryptography.DecryptAES(parsed["data"][0]["Mandatory"].ToString()) == "Y")
                                  {
                                      await Application.Current.MainPage.DisplayAlert("New Version", $"There is a new version (v{AESCryptography.DecryptAES(parsed["data"][0]["VersionNumber"].ToString())}) of this app available.\nWhatsNew: {AESCryptography.DecryptAES(parsed["data"][0]["WhatsNew"].ToString())}", "Update");
                                      await Launcher.OpenAsync(AESCryptography.DecryptAES(parsed["data"][0]["StoreLink"].ToString()));
                                  }
                                  else
                                  {
                                      var update = await Application.Current.MainPage.DisplayAlert("New Version", $"There is a new version (v{AESCryptography.DecryptAES(parsed["data"][0]["VersionNumber"].ToString())}) of this app available.\nWhatsNew: {AESCryptography.DecryptAES(parsed["data"][0]["WhatsNew"].ToString())}\nWould you like to update now?", "Yes", "No");
                                      if (update)
                                      {
                                          await Launcher.OpenAsync(AESCryptography.DecryptAES(parsed["data"][0]["StoreLink"].ToString()));
                                      }
                                  }
                              }
                          }
                      }
                      //else if ((int)response.StatusCode != 404)
                      //{
                      //    await App.Current.MainPage.DisplayAlert(App.AppName, parsed["Message"].ToString(), App.Btn_Close);
                      //}
                      //return (int)response.StatusCode;
                  }
                  catch (Exception)
                  {
                      //await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", "OK");
                      //return 500;
                  }
              }
              else
              {
                  //await App.Current.MainPage.DisplayAlert(App.AppName, App.NoInternet_, App.Btn_Close);
                  //return 101;
              }
          }*/

        public async void AppVersion()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var client = new HttpClient();
                    /* var byteArray = Encoding.ASCII.GetBytes(Preferences.Get("BasicAuth", "xx:xx"));
                     client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));*/
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                    string _Plateform = "A";
                    if (DevicePlatform.Android == DevicePlatform.Android)
                    {
                        _Plateform = "A";
                    }
                    else if (DevicePlatform.iOS == DevicePlatform.iOS)
                    {
                        _Plateform = "I";
                    }
                    double installedVersionNumber = double.Parse(VersionTracking.CurrentVersion);

                    string parameters = baseurl + $"api/AppVersion?" +
                    $"Platform={HttpUtility.UrlEncode(AESCryptography.EncryptAES(_Plateform))}" +
                    $"&packageid={HttpUtility.UrlEncode(AESCryptography.EncryptAES(AppInfo.PackageName))}";

                    HttpResponseMessage response = await client.GetAsync(parameters);
                    var result = await response.Content.ReadAsStringAsync();
                    var parsed = JObject.Parse(result);

                    if ((int)response.StatusCode == 200)
                    {
                        foreach (var pair in parsed)
                        {
                            if (pair.Key == "data")
                            {
                                double latestVersionNumber = double.Parse(AESCryptography.DecryptAES(parsed["data"][0]["VersionNumber"].ToString()));
                                if (installedVersionNumber < latestVersionNumber)
                                {
                                    if (AESCryptography.DecryptAES(parsed["data"][0]["Mandatory"].ToString()) == "Y")
                                    {
                                        await Application.Current.MainPage.DisplayAlert("New Version", $"There is a new version (v{AESCryptography.DecryptAES(parsed["data"][0]["VersionNumber"].ToString())}) of this app available.\nWhatsNew: {AESCryptography.DecryptAES(parsed["data"][0]["WhatsNew"].ToString())}", "Update");
                                        await Launcher.OpenAsync(AESCryptography.DecryptAES(parsed["data"][0]["StoreLink"].ToString()));
                                    }
                                    else
                                    {
                                        var update = await Application.Current.MainPage.DisplayAlert("New Version", $"There is a new version (v{AESCryptography.DecryptAES(parsed["data"][0]["VersionNumber"].ToString())}) of this app available.\nWhatsNew: {AESCryptography.DecryptAES(parsed["data"][0]["WhatsNew"].ToString())}\nWould you like to update now?", "Yes", "No");
                                        if (update)
                                        {
                                            await Launcher.OpenAsync(AESCryptography.DecryptAES(parsed["data"][0]["StoreLink"].ToString()));
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //return (int)response.StatusCode;
                }
                catch (Exception)
                {
                    //await App.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", "OK");
                    //return 500;
                }
            }
            else
            {
                //await App.Current.MainPage.DisplayAlert( AppName, App.NoInternet_, App.Btn_Close);
                //return 101;
            }
        }

        public async Task<string> GetToken()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var client = new HttpClient();
                    var byteArray = Encoding.ASCII.GetBytes(Preferences.Get("BasicAuth", "xx:xx"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    string url = baseurl + $"api/GenerateToken";

                    HttpResponseMessage response = await client.GetAsync(url);

                    var result = await response.Content.ReadAsStringAsync();
                    var parsed = JObject.Parse(result);                
                    return parsed["TokenID"]?.ToString() ?? "";
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", "OK");
                    return "";
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("CERS", App.NoInternet_, App.Btn_Close);
                return "";
            }
        }
  
        public async Task<int> userlogin_Get(string MobileNo)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var client = new HttpClient();

                    //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                    string url = baseurl + $"api/UserLogin?" +
                          $"MobileNo={HttpUtility.UrlEncode(AESCryptography.EncryptAES(MobileNo))}";
                    HttpResponseMessage response = await client.GetAsync(url);

                    var result = await response.Content.ReadAsStringAsync();
                    var parsed = JObject.Parse(result);

                    if ((int)response.StatusCode == 200)
                    {
                        userDetailsDatabase.DeleteUserDetails();
                        foreach (var pair in parsed)
                        {
                            if (pair.Key == "data")
                            {
                                var nodes = pair.Value;
                                foreach (var node in nodes)
                                {
                                    var item = new UserDetails();

                                    //item.UserLevel = AESCryptography.DecryptAES(node["StateCode"].ToString());
                                    item.AUTO_ID = AESCryptography.DecryptAES(node["AUTO_ID"].ToString());
                                    item.EPIC_NO = AESCryptography.DecryptAES(node["EPIC_NO"].ToString());
                                    item.VOTER_NAME = AESCryptography.DecryptAES(node["VOTER_NAME"].ToString());
                                    item.RELATION_TYPE = AESCryptography.DecryptAES(node["RELATION_TYPE"].ToString());
                                    item.RELATIVE_NAME = AESCryptography.DecryptAES(node["RELATIVE_NAME"].ToString());
                                    item.GENDER = AESCryptography.DecryptAES(node["GENDER"].ToString());
                                    item.AGE = AESCryptography.DecryptAES(node["AGE"].ToString());
                                    item.EMAIL_ID = AESCryptography.DecryptAES(node["EMAIL_ID"].ToString());
                                    item.MOBILE_NUMBER = AESCryptography.DecryptAES(node["MOBILE_NUMBER"].ToString());
                                    item.AgentName = AESCryptography.DecryptAES(node["AgentName"].ToString());
                                    item.AgentMobile = AESCryptography.DecryptAES(node["AgentMobile"].ToString());
                                    item.Panchayat_Name = (AESCryptography.DecryptAES(node["Panchayat_Name"].ToString()));
                                    item.LoggedInAs = AESCryptography.DecryptAES(node["LoggedInAs"].ToString());
                                    item.OTPID = AESCryptography.DecryptAES(node["OTPID"].ToString());
                                    item.NominationForName = AESCryptography.DecryptAES(node["NominationForName"].ToString());
                                    item.NominationForNameLocal = AESCryptography.DecryptAES(node["NominationForNameLocal"].ToString());
                                    item.PollDate = AESCryptography.DecryptAES(node["PollDate"].ToString());
                                    item.NominationDate = AESCryptography.DecryptAES(node["NominationDate"].ToString());
                                    item.postcode = AESCryptography.DecryptAES(node["postcode"].ToString());
                                    item.LimitAmt = AESCryptography.DecryptAES(node["LimitAmt"].ToString());
                                    item.ResultDate = AESCryptography.DecryptAES(node["ResultDate"].ToString());
                                    item.Resultdatethirtydays = AESCryptography.DecryptAES(node["Resultdatethirtydays"].ToString());
                                    item.Block_Code = AESCryptography.DecryptAES(node["Block_Code"].ToString());
                                    item.panwardcouncilname = AESCryptography.DecryptAES(node["panwardcouncilname"].ToString());
                                    item.panwardcouncilnamelocal = AESCryptography.DecryptAES(node["panwardcouncilnamelocal"].ToString());
                                    item.ExpStatus = AESCryptography.DecryptAES(node["ExpStatus"].ToString());

                                    //adeded on 25sep24
                                    try
                                    {
                                        item.expStartDate = AESCryptography.DecryptAES(node["expStartDate"].ToString());
                                        item.expEndDate = AESCryptography.DecryptAES(node["expEndDate"].ToString());
                                    }
                                    catch { }

                                    userDetailsDatabase.AddUserDetails(item);
                                }
                            }
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("CERS", parsed["Message"].ToString(), App.Btn_Close);
                    }
                    return (int)response.StatusCode;
                }
                catch 
                {
                    await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", "OK");
                    return 500;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("CERS", App.NoInternet_, App.Btn_Close);
                return 101;
            }
        }

        public async Task<int> observorlogin_Get(string MobileNo)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var client = new HttpClient();

                    //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                    string url = baseurl + $"api/ObservorLogin?" +
                          $"MobileNo={HttpUtility.UrlEncode(AESCryptography.EncryptAES(MobileNo))}";
                    HttpResponseMessage response = await client.GetAsync(url);

                    var result = await response.Content.ReadAsStringAsync();
                    var parsed = JObject.Parse(result);

                    if ((int)response.StatusCode == 200)
                    {
                        observorLoginDetailsDatabase.DeleteObservorLoginDetails();
                        foreach (var pair in parsed)
                        {
                            if (pair.Key == "data")
                            {
                                var nodes = pair.Value;
                                foreach (var node in nodes)
                                {
                                    var item = new ObservorLoginDetails();


                                    item.Auto_ID = AESCryptography.DecryptAES(node["Auto_ID"].ToString());
                                    item.ObserverName = AESCryptography.DecryptAES(node["ObserverName"].ToString());
                                    item.ObserverContact = AESCryptography.DecryptAES(node["ObserverContact"].ToString());
                                    item.ObserverDesignation = AESCryptography.DecryptAES(node["ObserverDesignation"].ToString());
                                    item.Pritype = AESCryptography.DecryptAES(node["Pritype"].ToString());

                                    observorLoginDetailsDatabase.AddObservorLoginDetails(item);
                                }
                            }
                        }
                    }
                    else
                    {
                        //await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), parsed["Message"].ToString(), App.Btn_Close);
                    }
                    return (int)response.StatusCode;
                }
                catch
                {
                    await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", "OK");
                    return 500;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), App.NoInternet_, App.Btn_Close);
                return 101;
            }
        }

        public async Task<int> GetOtp(string MobileNo)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var client = new HttpClient();

                    //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                    string url = baseurl + $"api/GetOTP?" +
                          $"MobileNo={HttpUtility.UrlEncode(AESCryptography.EncryptAES(MobileNo))}";
                    HttpResponseMessage response = await client.GetAsync(url);

                    var result = await response.Content.ReadAsStringAsync();
                    var parsed = JObject.Parse(result);

                    if ((int)response.StatusCode == 200)
                    {
                        foreach (var pair in parsed)
                        {
                            if (pair.Key == "data")
                            {
                                var nodes = pair.Value;
                                foreach (var node in nodes)
                                {
                                    var item = new UserDetails();
                                    item.OTPID = AESCryptography.DecryptAES(node["OTPID"].ToString());
                                    if (Preferences.Get("UserType", "").Equals("Observor"))
                                    {
                                        observorLoginDetailsDatabase.UpdateCustomquery($"update observorLoginDetails set OTPID ='{item.OTPID}'");
                                    }
                                    else
                                    {
                                        userDetailsDatabase.UpdateCustomquery($"update userDetails set OTPID ='{item.OTPID}'");
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("CERS", parsed["Message"].ToString(), App.Btn_Close);
                    }
                    return (int)response.StatusCode;
                }
                catch
                {
                    await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", "OK");
                    return 500;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("CERS", App.NoInternet_, App.Btn_Close);
                return 101;
            }
        }

        public async Task<int> checkotp_Get(string MobileNo, string UserOtp)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                List<ObservorLoginDetails> observorLoginDetailslist;
                string otpId;
                if (Preferences.Get("UserType", "").Equals("Observor"))
                {
                    observorLoginDetailslist = observorLoginDetailsDatabase.GetObservorLoginDetails("Select * from ObservorLoginDetails").ToList();
                    otpId = observorLoginDetailslist.ElementAt(0).OTPID;
                }
                else
                {

                    userDetailslist = userDetailsDatabase.GetUserDetails("Select * from UserDetails").ToList();
                    otpId = userDetailslist.ElementAt(0).OTPID;
                }
                try
                {
                    var client = new HttpClient();

                    //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                    string url = baseurl + $"api/CheckOtp?" +
                          $"MobileNo={HttpUtility.UrlEncode(AESCryptography.EncryptAES(MobileNo))}" +
                          $"&UserOtp={HttpUtility.UrlEncode(AESCryptography.EncryptAES(UserOtp))}" +
                          $"&otpId={HttpUtility.UrlEncode(AESCryptography.EncryptAES(otpId))}";
                    HttpResponseMessage response = await client.GetAsync(url);

                    var result = await response.Content.ReadAsStringAsync();
                    var parsed = JObject.Parse(result);
                    return (int)response.StatusCode;
                }
                catch
                {
                    await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", "OK");
                    return 500;
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("CERS", App.NoInternet_, App.Btn_Close);
                return 101;
            }
        }

        public async Task<int> LocalResources_Get()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var client = new HttpClient();
                    //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());

                    HttpResponseMessage response = await client.GetAsync(baseurl + $"api/LocalResources");

                    var result = await response.Content.ReadAsStringAsync();
                    var parsed = JObject.Parse(result);
                    languageMasterDatabase.DeleteLanguageMaster();
                    if ((int)response.StatusCode == 200)
                    {
                        foreach (var pair in parsed)
                        {
                            if (pair.Key == "data")
                            {
                                var nodes = pair.Value;
                                foreach (var node in nodes)
                                {
                                    var item = new LanguageMaster();
                                    item.MultipleResourceKey = AESCryptography.DecryptAES(node["MultipleResourceKey"].ToString());
                                    item.ResourceKey = AESCryptography.DecryptAES(node["ResourceKey"].ToString());
                                    item.ResourceValue = AESCryptography.DecryptAES(node["ResourceValue"].ToString());
                                    item.LocalResourceValue = AESCryptography.DecryptAES(node["LocalResourceValue"].ToString());
                                    languageMasterDatabase.AddLanguageMaster(item);
                                }
                            }
                        }
                        App.MyLanguage = languageMasterDatabase.GetLanguageMaster($"select ResourceKey, (case when ({App.Language} = 0) then ResourceValue else LocalResourceValue end)ResourceValue from  LanguageMaster").ToList();

                    }
                    else if ((int)response.StatusCode == 404)
                    {
                        await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), parsed["Message"].ToString(), App.Btn_Close);
                    }
                    return (int)response.StatusCode;
                }
                catch
                {
                    await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", "OK");
                    return 500;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), App.NoInternet_, App.Btn_Close);
                return 101;
            }
        }

        public async Task<int> ExpenseSources_Get()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var client = new HttpClient();
                    //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());

                    HttpResponseMessage response = await client.GetAsync(baseurl + $"api/ExpenseSource");

                    var result = await response.Content.ReadAsStringAsync();
                    var parsed = JObject.Parse(result);
                    expenseSourcesDatabase.DeleteExpenseSources();
                    if ((int)response.StatusCode == 200)
                    {
                        foreach (var pair in parsed)
                        {
                            if (pair.Key == "data")
                            {
                                var nodes = pair.Value;
                                foreach (var node in nodes)
                                {
                                    var item = new ExpenseSources();
                                    item.Exp_code = AESCryptography.DecryptAES(node["Exp_code"].ToString());
                                    item.Exp_Desc = AESCryptography.DecryptAES(node["Exp_Desc"].ToString());
                                    item.Exp_Desc_Local = AESCryptography.DecryptAES(node["Exp_Desc_Local"].ToString());

                                    expenseSourcesDatabase.AddExpenseSources(item);
                                }
                            }
                        }
                    }
                    else if ((int)response.StatusCode == 404)
                    {
                        await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), parsed["Message"].ToString(), App.Btn_Close);
                    }
                    return (int)response.StatusCode;
                }
                catch
                {
                    await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", "OK");
                    return 500;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), App.NoInternet_, App.Btn_Close);
                return 101;
            }
        }

        public async Task<int> PaymentMode_Get()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var client = new HttpClient();
                    //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());

                    HttpResponseMessage response = await client.GetAsync(baseurl + $"api/PaymentMode");

                    var result = await response.Content.ReadAsStringAsync();
                    var parsed = JObject.Parse(result);
                    paymentModesDatabase.DeletePaymentModes();
                    if ((int)response.StatusCode == 200)
                    {
                        foreach (var pair in parsed)
                        {
                            if (pair.Key == "data")
                            {
                                var nodes = pair.Value;
                                foreach (var node in nodes)
                                {
                                    var item = new PaymentModes();
                                    item.paymode_code = AESCryptography.DecryptAES(node["paymode_code"].ToString());
                                    item.paymode_Desc = AESCryptography.DecryptAES(node["paymode_Desc"].ToString());
                                    item.paymode_Desc_Local = AESCryptography.DecryptAES(node["paymode_Desc_Local"].ToString());

                                    paymentModesDatabase.AddPaymentModes(item);
                                }
                            }
                        }
                    }
                    else if ((int)response.StatusCode == 404)
                    {
                        await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), parsed["Message"].ToString(), App.Btn_Close);
                    }
                    return (int)response.StatusCode;
                }
                catch
                {
                    await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", "OK");
                    return 500;
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), App.NoInternet_, App.Btn_Close);
                return 101;
            }
        }

        public async Task<int> ExpenditureDetails_Get()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    userDetailslist = userDetailsDatabase.GetUserDetails("Select * from UserDetails").ToList();
                    string AutoID = userDetailslist.ElementAt(0).AUTO_ID;
                    var client = new HttpClient();
                    //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                    string url = baseurl + $"api/ExpenditureDetails?" +
                       $"AutoID={HttpUtility.UrlEncode(AESCryptography.EncryptAES(AutoID))}";
                    HttpResponseMessage response = await client.GetAsync(url);

                    var result = await response.Content.ReadAsStringAsync();
                    var parsed = JObject.Parse(result);
                    expenditureDetailsDatabase.DeleteExpenditureDetails();
                    if ((int)response.StatusCode == 200)
                    {
                        foreach (var pair in parsed)
                        {
                            if (pair.Key == "data")
                            {
                                var nodes = pair.Value;
                                foreach (var node in nodes)
                                {
                                    var item = new ExpenditureDetails();
                                    item.ExpenseID = AESCryptography.DecryptAES(node["ExpenseID"].ToString());
                                    item.AutoID = AESCryptography.DecryptAES(node["AutoID"].ToString());
                                    item.expDate = AESCryptography.DecryptAES(node["expDate"].ToString());
                                    item.expCode = AESCryptography.DecryptAES(node["expCode"].ToString());
                                    item.amtType = AESCryptography.DecryptAES(node["amtType"].ToString());
                                    item.amount = AESCryptography.DecryptAES(node["amount"].ToString());
                                    item.paymentDate = AESCryptography.DecryptAES(node["paymentDate"].ToString());
                                    item.voucherBillNumber = AESCryptography.DecryptAES(node["voucherBillNumber"].ToString());
                                    item.payMode = AESCryptography.DecryptAES(node["payMode"].ToString());
                                    item.payeeName = AESCryptography.DecryptAES(node["payeeName"].ToString());
                                    item.payeeAddress = AESCryptography.DecryptAES(node["payeeAddress"].ToString());
                                    item.sourceMoney = AESCryptography.DecryptAES(node["sourceMoney"].ToString());
                                    item.remarks = AESCryptography.DecryptAES(node["remarks"].ToString());
                                    item.remarks = AESCryptography.DecryptAES(node["remarks"].ToString());
                                    item.DtTm = AESCryptography.DecryptAES(node["DtTm"].ToString());
                                    item.ExpStatus = AESCryptography.DecryptAES(node["ExpStatus"].ToString());
                                    item.ExpTypeName = AESCryptography.DecryptAES(node["ExpTypeName"].ToString());
                                    item.ExpTypeNameLocal = AESCryptography.DecryptAES(node["ExpTypeNameLocal"].ToString());
                                    item.PayModeName = AESCryptography.DecryptAES(node["PayModeName"].ToString());
                                    item.PayModeNameLocal = AESCryptography.DecryptAES(node["PayModeNameLocal"].ToString());
                                    item.evidenceFile = AESCryptography.DecryptAES(node["evidenceFile"].ToString());
                                    item.expDateDisplay = AESCryptography.DecryptAES(node["expDateDisplay"].ToString());
                                    item.paymentDateDisplay = AESCryptography.DecryptAES(node["paymentDateDisplay"].ToString());
                                    item.amountoutstanding = AESCryptography.DecryptAES(node["amountoutstanding"].ToString());
                                    item.ObserverRemarks = AESCryptography.DecryptAES(node["ObserverRemarks"].ToString());
                                    item.lastupdated = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                                    expenditureDetailsDatabase.AddExpenditureDetails(item);
                                }
                            }
                        }
                    }
                    else if ((int)response.StatusCode == 404)
                    {//norecordsfound
                     //  await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), parsed["Message"].ToString(), App.Btn_Close);
                    }
                    return (int)response.StatusCode;
                }
                catch
                {
                    await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", "OK");
                    return 500;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), App.NoInternet_, App.Btn_Close);
                return 101;
            }
        }

        public async Task<int> ObserverExpenditureDetails_Get(string AutoID)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var client = new HttpClient();
                    //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                    string url = baseurl + $"api/ExpenditureDetailsObserver?" +
                       $"AutoID={HttpUtility.UrlEncode(AESCryptography.EncryptAES(AutoID))}";
                    HttpResponseMessage response = await client.GetAsync(url);

                    var result = await response.Content.ReadAsStringAsync();
                    var parsed = JObject.Parse(result);
                    observerExpenditureDetailsDatabase.DeleteObserverExpenditureDetails();
                    if ((int)response.StatusCode == 200)
                    {
                        foreach (var pair in parsed)
                        {
                            if (pair.Key == "data")
                            {
                                var nodes = pair.Value;
                                foreach (var node in nodes)
                                {
                                    var item = new ObserverExpenditureDetails();
                                    item.ExpenseID = AESCryptography.DecryptAES(node["ExpenseID"].ToString());
                                    item.AutoID = AESCryptography.DecryptAES(node["AutoID"].ToString());
                                    item.expDate = AESCryptography.DecryptAES(node["expDate"].ToString());
                                    item.expCode = AESCryptography.DecryptAES(node["expCode"].ToString());
                                    item.amtType = AESCryptography.DecryptAES(node["amtType"].ToString());
                                    item.amount = AESCryptography.DecryptAES(node["amount"].ToString());
                                    item.paymentDate = AESCryptography.DecryptAES(node["paymentDate"].ToString());
                                    item.voucherBillNumber = AESCryptography.DecryptAES(node["voucherBillNumber"].ToString());
                                    item.payMode = AESCryptography.DecryptAES(node["payMode"].ToString());
                                    item.payeeName = AESCryptography.DecryptAES(node["payeeName"].ToString());
                                    item.payeeAddress = AESCryptography.DecryptAES(node["payeeAddress"].ToString());
                                    item.sourceMoney = AESCryptography.DecryptAES(node["sourceMoney"].ToString());
                                    item.remarks = AESCryptography.DecryptAES(node["remarks"].ToString());
                                    item.remarks = AESCryptography.DecryptAES(node["remarks"].ToString());
                                    item.DtTm = AESCryptography.DecryptAES(node["DtTm"].ToString());
                                    item.ExpStatus = AESCryptography.DecryptAES(node["ExpStatus"].ToString());
                                    item.ExpTypeName = AESCryptography.DecryptAES(node["ExpTypeName"].ToString());
                                    item.ExpTypeNameLocal = AESCryptography.DecryptAES(node["ExpTypeNameLocal"].ToString());
                                    item.PayModeName = AESCryptography.DecryptAES(node["PayModeName"].ToString());
                                    item.PayModeNameLocal = AESCryptography.DecryptAES(node["PayModeNameLocal"].ToString());
                                    item.evidenceFile = AESCryptography.DecryptAES(node["evidenceFile"].ToString());
                                    item.expDateDisplay = AESCryptography.DecryptAES(node["expDateDisplay"].ToString());
                                    item.paymentDateDisplay = AESCryptography.DecryptAES(node["paymentDateDisplay"].ToString());
                                    item.amountoutstanding = AESCryptography.DecryptAES(node["amountoutstanding"].ToString());
                                    item.CONSTITUENCY_CODE = AESCryptography.DecryptAES(node["CONSTITUENCY_CODE"].ToString());
                                    item.VOTER_NAME = AESCryptography.DecryptAES(node["VOTER_NAME"].ToString());
                                    item.AgentName = AESCryptography.DecryptAES(node["AgentName"].ToString());
                                    item.ObserverRemarks = AESCryptography.DecryptAES(node["ObserverRemarks"].ToString());

                                    item.lastupdated = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                                    observerExpenditureDetailsDatabase.AddObserverExpenditureDetails(item);
                                }
                            }
                        }
                    }
                    else if ((int)response.StatusCode == 404)
                    {
                        await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), parsed["Message"].ToString(), App.Btn_Close);
                    }
                    return (int)response.StatusCode;
                }
                catch
                {
                    await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", "OK");
                    return 500;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), App.NoInternet_, App.Btn_Close);
                return 101;
            }
        }

        public async Task<int> ObserverWards_Get(string MobileNo)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var client = new HttpClient();
                    //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                    string url = baseurl + $"api/ObserverWards?" +
                       $"MobileNo={HttpUtility.UrlEncode(AESCryptography.EncryptAES(MobileNo))}";
                    HttpResponseMessage response = await client.GetAsync(url);

                    var result = await response.Content.ReadAsStringAsync();
                    var parsed = JObject.Parse(result);
                    observerWardsDatabase.DeleteObserverWards();
                    if ((int)response.StatusCode == 200)
                    {
                        foreach (var pair in parsed)
                        {
                            if (pair.Key == "data")
                            {
                                var nodes = pair.Value;
                                foreach (var node in nodes)
                                {
                                    var item = new ObserverWards();
                                    item.Panchayat_Code = AESCryptography.DecryptAES(node["Panchayat_Code"].ToString());
                                    item.Panchayat_Name = AESCryptography.DecryptAES(node["Panchayat_Name"].ToString());
                                    item.Panchayat_Name_Local = AESCryptography.DecryptAES(node["Panchayat_Name_Local"].ToString());
                                    item.lastupdated = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                                    observerWardsDatabase.AddObserverWards(item);
                                }
                            }
                        }

                    }
                    else if ((int)response.StatusCode == 404)
                    {
                        //await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), parsed["Message"].ToString(), App.Btn_Close);
                    }
                    return (int)response.StatusCode;
                }
                catch
                {
                    await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", "OK");
                    return 500;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), App.NoInternet_, App.Btn_Close);
                return 101;
            }
        }

        public async Task<int> CheckUserType_Get(string MobileNo)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var client = new HttpClient();
                   // string basicauthj = App.basic_auth();
                    //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                    string url = baseurl + $"api/CheckUserType?" +
                       $"MobileNo={HttpUtility.UrlEncode(AESCryptography.EncryptAES(MobileNo))}";
                    HttpResponseMessage response = await client.GetAsync(url);

                    var result = await response.Content.ReadAsStringAsync();
                    var parsed = JObject.Parse(result);
                    //observerWardsDatabase.DeleteObserverWards();
                    if ((int)response.StatusCode == 200)
                    {
                        foreach (var pair in parsed)
                        {
                            if (pair.Key == "data")
                            {
                                var nodes = pair.Value;
                                foreach (var node in nodes)
                                {
                                    string UserType = AESCryptography.DecryptAES(node["UserType"].ToString());
                                    Preferences.Set("UserType", UserType);
                                    /* var item = new ObserverWards();
                                     item.Panchayat_Code = AESCryptography.DecryptAES(node["UserType"].ToString());
                                     item.Panchayat_Name = AESCryptography.DecryptAES(node["Panchayat_Name"].ToString());
                                     item.Panchayat_Name_Local = AESCryptography.DecryptAES(node["Panchayat_Name_Local"].ToString());
                                     item.lastupdated = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                                     observerWardsDatabase.AddObserverWards(item);*/
                                }
                            }
                        }                        
                    }
                    else if ((int)response.StatusCode == 404)
                    {
                        await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName") ?? "CERS", parsed["Message"].ToString(), App.Btn_Close);
                    }
                    else if ((int)response.StatusCode == 401)
                    {
                        await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName")?? "CERS", parsed["Message"].ToString(), App.Btn_Close);
                    }
                    return (int)response.StatusCode;
                }
                catch
                {
                    await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", "OK");
                    return 500;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName") ?? "CERS", App.NoInternet_, App.Btn_Close);
                return 101;
            }
        }

        public async Task<int> ObserverCandidates_Get(string PanchWardCode)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var client = new HttpClient();
                    //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                    string url = baseurl + $"api/ObserverCandidates?" +
                       $"PanchWardCode={HttpUtility.UrlEncode(AESCryptography.EncryptAES(PanchWardCode))}";
                    HttpResponseMessage response = await client.GetAsync(url);

                    var result = await response.Content.ReadAsStringAsync();
                    var parsed = JObject.Parse(result);
                    observerCandidatesDatabase.DeleteObserverCandidates();
                    if ((int)response.StatusCode == 200)
                    {
                        foreach (var pair in parsed)
                        {
                            if (pair.Key == "data")
                            {
                                var nodes = pair.Value;
                                foreach (var node in nodes)
                                {
                                    var item = new ObserverCandidates();
                                    item.AUTO_ID = AESCryptography.DecryptAES(node["AUTO_ID"].ToString());
                                    item.Amount = AESCryptography.DecryptAES(node["Amount"].ToString());
                                    item.VOTER_NAME = AESCryptography.DecryptAES(node["VOTER_NAME"].ToString());
                                    // item.lastupdated = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                                    observerCandidatesDatabase.AddObserverCandidates(item);
                                }
                            }
                        }
                    }
                    else if ((int)response.StatusCode == 404)
                    {
                        await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), parsed["Message"].ToString(), App.Btn_Close);
                    }
                    return (int)response.StatusCode;
                }
                catch
                {
                    await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", "OK");
                    return 500;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), App.NoInternet_, App.Btn_Close);
                return 101;
            }
        }

        public async Task<int> SaveObserverRemarks_Post(string expenseid, string ObserverId, string remarks)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var client = new HttpClient();
                    //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());

                    string jsonData = JsonConvert.SerializeObject(new
                    {
                        ExpenseId = AESCryptography.EncryptAES(expenseid),
                        ObserverId = AESCryptography.EncryptAES(ObserverId),
                        ObserverRemarks = AESCryptography.EncryptAES(remarks),
                    });

                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(baseurl + "api/AddObserverRemarks", content);

                    var result = await response.Content.ReadAsStringAsync();
                    var parsed = JObject.Parse(result);
                    await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), parsed["Message"].ToString(), App.Btn_Close);
                    /*if ((int)response.StatusCode != 200)
                    {

                        await App.Current.MainPage.DisplayAlert( App.GetLabelByKey("AppName"), parsed["Message"].ToString(), App.Btn_Close);
                    }*/
                    return (int)response.StatusCode;

                }
                catch (Exception)
                {
                    await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", "OK");
                    return 500;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), App.NoInternet_, App.Btn_Close);
                return 101;
            }
        }

        public async Task<int> SaveExpenditure(string AutoID, string expDate, string expCode,
              string amtType, string amount, string amountoutstanding, string paymentDate, string voucherBillNumber, string payMode,
              string payeeName, string payeeAddress, string sourceMoney, string remarks, String _file)
        {
            int responsereturned = 400;
            try
            {
                var client = new HttpClient();
                string url = baseurl + "SaveExpenditureDetails.aspx?";
                client.BaseAddress = new Uri($"{url}");
                //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());

                string paramteres =
                $"AutoID={HttpUtility.UrlEncode(AESCryptography.EncryptAES(AutoID))}" +
                $"&expDate={HttpUtility.UrlEncode(AESCryptography.EncryptAES(expDate))}" +
                $"&expCode={HttpUtility.UrlEncode(AESCryptography.EncryptAES(expCode))}" +
                $"&amtType={HttpUtility.UrlEncode(AESCryptography.EncryptAES(amtType))}" +
                $"&amount={HttpUtility.UrlEncode(AESCryptography.EncryptAES(amount))}" +
                $"&amountoutstanding={HttpUtility.UrlEncode(AESCryptography.EncryptAES(amountoutstanding))}" +
                $"&paymentDate={HttpUtility.UrlEncode(AESCryptography.EncryptAES(paymentDate))}" +
                $"&voucherBillNumber={HttpUtility.UrlEncode(AESCryptography.EncryptAES(voucherBillNumber))}" +
                $"&payMode={HttpUtility.UrlEncode(AESCryptography.EncryptAES(payMode))}" +
                $"&payeeName={HttpUtility.UrlEncode(AESCryptography.EncryptAES(payeeName))}" +
                $"&payeeAddress={HttpUtility.UrlEncode(AESCryptography.EncryptAES(payeeAddress))}" +
                $"&sourceMoney={HttpUtility.UrlEncode(AESCryptography.EncryptAES(sourceMoney))}" +
                $"&remarks={HttpUtility.UrlEncode(AESCryptography.EncryptAES(remarks))}" +
                $"&file={HttpUtility.UrlEncode(_file)}";


                /* string paramteres2 =
                 $"AutoID:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(AutoID))}" +
                 $"\nexpDate:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(expDate))}" +
                 $"\nexpCode:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(expCode))}" +
                 $"\namtType:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(amtType))}" +
                 $"\namount:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(amount))}" +
                 $"\npaymentDate:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(paymentDate))}" +
                 $"\nvoucherBillNumber:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(voucherBillNumber))}" +
                 $"\npayMode:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(payMode))}" +
                 $"\npayeeName:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(payeeName))}" +
                 $"\npayeeAddress:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(payeeAddress))}" +
                 $"\nsourceMoney:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(sourceMoney))}" +
                 $"\nremarks:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(remarks))}" +
                 $"\nfile:{HttpUtility.UrlEncode(_file)}";

                 string paramteres1 =
                   $"AutoID:{HttpUtility.UrlEncode((AutoID))}" +
                   $"\nexpDate:{HttpUtility.UrlEncode((expDate))}" +
                   $"\nexpCode:{HttpUtility.UrlEncode((expCode))}" +
                   $"\namtType:{HttpUtility.UrlEncode((amtType))}" +
                   $"\namount:{HttpUtility.UrlEncode((amount))}" +
                   $"\npaymentDate:{HttpUtility.UrlEncode((paymentDate))}" +
                   $"\nvoucherBillNumber:{HttpUtility.UrlEncode((voucherBillNumber))}" +
                   $"\npayMode:{HttpUtility.UrlEncode((payMode))}" +
                   $"\npayeeName:{HttpUtility.UrlEncode((payeeName))}" +
                   $"\npayeeAddress:{HttpUtility.UrlEncode((payeeAddress))}" +
                   $"\nsourceMoney:{HttpUtility.UrlEncode((sourceMoney))}" +
                   $"\nremarks:{HttpUtility.UrlEncode((remarks))}" +
                   $"\nfile:{HttpUtility.UrlEncode(_file)}";*/

                var content = new StringContent(paramteres, Encoding.UTF8, "application/x-www-form-urlencoded");
                HttpResponseMessage response = await client.PostAsync(client.BaseAddress, content);

                if (!response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert(App.AppName, $"{response.StatusCode}-" + "Unable To Connect" + "\n" + "Please Try Again", "Close");
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    responsereturned = int.Parse(parsed["status"].ToString());
                    if (parsed["status"].ToString().Equals("200"))
                    {
                        await ExpenditureDetails_Get();
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["message"].ToString(), App.Btn_Close);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["message"].ToString(), App.Btn_Close);
                    }
                }
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", App.Btn_Close);
                responsereturned = 500;
                return responsereturned;
            }
            return responsereturned;
        }

        public async Task<int> UpdateExpenditure(string ExpenseID, string expDate, string expCode,
             string amtType, string amount, string amountoutstanding, string paymentDate, string voucherBillNumber, string payMode,
             string payeeName, string payeeAddress, string sourceMoney, string remarks, String _file)
        {
            int responsereturned = 400;
            try
            {
                var client = new HttpClient();
                string url = baseurl + "UpdateExpenditureDetails.aspx?";
                client.BaseAddress = new Uri($"{url}");
                //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());

                string paramteres =
                $"ExpenseID={HttpUtility.UrlEncode(AESCryptography.EncryptAES(ExpenseID))}" +
                $"&expDate={HttpUtility.UrlEncode(AESCryptography.EncryptAES(expDate))}" +
                $"&expCode={HttpUtility.UrlEncode(AESCryptography.EncryptAES(expCode))}" +
                $"&amtType={HttpUtility.UrlEncode(AESCryptography.EncryptAES(amtType))}" +
                $"&amount={HttpUtility.UrlEncode(AESCryptography.EncryptAES(amount))}" +
                $"&amountoutstanding={HttpUtility.UrlEncode(AESCryptography.EncryptAES(amountoutstanding))}" +
                $"&paymentDate={HttpUtility.UrlEncode(AESCryptography.EncryptAES(paymentDate))}" +
                $"&voucherBillNumber={HttpUtility.UrlEncode(AESCryptography.EncryptAES(voucherBillNumber))}" +
                $"&payMode={HttpUtility.UrlEncode(AESCryptography.EncryptAES(payMode))}" +
                $"&payeeName={HttpUtility.UrlEncode(AESCryptography.EncryptAES(payeeName))}" +
                $"&payeeAddress={HttpUtility.UrlEncode(AESCryptography.EncryptAES(payeeAddress))}" +
                $"&sourceMoney={HttpUtility.UrlEncode(AESCryptography.EncryptAES(sourceMoney))}" +
                $"&remarks={HttpUtility.UrlEncode(AESCryptography.EncryptAES(remarks))}" +
                $"&file={HttpUtility.UrlEncode(_file)}";


                /* string paramteres2 =
                 $"AutoID:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(AutoID))}" +
                 $"\nexpDate:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(expDate))}" +
                 $"\nexpCode:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(expCode))}" +
                 $"\namtType:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(amtType))}" +
                 $"\namount:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(amount))}" +
                 $"\npaymentDate:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(paymentDate))}" +
                 $"\nvoucherBillNumber:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(voucherBillNumber))}" +
                 $"\npayMode:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(payMode))}" +
                 $"\npayeeName:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(payeeName))}" +
                 $"\npayeeAddress:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(payeeAddress))}" +
                 $"\nsourceMoney:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(sourceMoney))}" +
                 $"\nremarks:{HttpUtility.UrlEncode(AESCryptography.EncryptAES(remarks))}" +
                 $"\nfile:{HttpUtility.UrlEncode(_file)}";

                 string paramteres1 =
                   $"AutoID:{HttpUtility.UrlEncode((AutoID))}" +
                   $"\nexpDate:{HttpUtility.UrlEncode((expDate))}" +
                   $"\nexpCode:{HttpUtility.UrlEncode((expCode))}" +
                   $"\namtType:{HttpUtility.UrlEncode((amtType))}" +
                   $"\namount:{HttpUtility.UrlEncode((amount))}" +
                   $"\npaymentDate:{HttpUtility.UrlEncode((paymentDate))}" +
                   $"\nvoucherBillNumber:{HttpUtility.UrlEncode((voucherBillNumber))}" +
                   $"\npayMode:{HttpUtility.UrlEncode((payMode))}" +
                   $"\npayeeName:{HttpUtility.UrlEncode((payeeName))}" +
                   $"\npayeeAddress:{HttpUtility.UrlEncode((payeeAddress))}" +
                   $"\nsourceMoney:{HttpUtility.UrlEncode((sourceMoney))}" +
                   $"\nremarks:{HttpUtility.UrlEncode((remarks))}" +
                   $"\nfile:{HttpUtility.UrlEncode(_file)}";*/

                var content = new StringContent(paramteres, Encoding.UTF8, "application/x-www-form-urlencoded");
                HttpResponseMessage response = await client.PostAsync(client.BaseAddress, content);

                if (!response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert(App.AppName, $"{response.StatusCode}-" + "Unable To Connect" + "\n" + "Please Try Again", "Close");
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    responsereturned = int.Parse(parsed["status"].ToString());
                    if (parsed["status"].ToString().Equals("200"))
                    {
                        await ExpenditureDetails_Get();
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["message"].ToString(), App.Btn_Close);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["message"].ToString(), App.Btn_Close);
                    }
                }
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", App.Btn_Close);
                responsereturned = 500;
                return responsereturned;
            }
            return responsereturned;
        }

        public async Task<int> finalsubmit(string AutoID)
        {
            int responsereturned = 400;
            try
            {
                var client = new HttpClient();
                string url = baseurl + "FinalSubmitExpenditureNov23.aspx?";
                client.BaseAddress = new Uri($"{url}");
                //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());

                string paramteres = $"AutoID={HttpUtility.UrlEncode(AESCryptography.EncryptAES(AutoID))}";
                var content = new StringContent(paramteres, Encoding.UTF8, "application/x-www-form-urlencoded");
                HttpResponseMessage response = await client.PostAsync(client.BaseAddress, content);

                if (!response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert(App.AppName, $"{response.StatusCode}-" + "Unable To Connect" + "\n" + "Please Try Again", "Close");
                }
                else
                {
                    var result = await response.Content.ReadAsStringAsync();
                    JObject parsed = JObject.Parse(result);
                    responsereturned = int.Parse(parsed["status"].ToString());
                    Preferences.Set("FinalSubmitMsg", parsed["message"].ToString());
                    /* if (parsed["status"].ToString().Equals("200"))
                     {
                         await ExpenditureDetails_Get();                        
                     }
                     await Application.Current.MainPage.DisplayAlert(App.AppName, parsed["message"].ToString(), App.Btn_Close);*/
                }
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", App.Btn_Close);
                responsereturned = 500;
                return responsereturned;
            }
            return responsereturned;
        }

        public async Task<int> SaveUserRemarks_Post(string expenseid, string remarks, string ObserverRemarksId)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var client = new HttpClient();
                    //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());

                    string jsonData = JsonConvert.SerializeObject(new
                    {
                        ExpenseId = AESCryptography.EncryptAES(expenseid),
                        UserRemarks = AESCryptography.EncryptAES(remarks),
                        ObserverRemarksId = AESCryptography.EncryptAES(ObserverRemarksId),
                    });

                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(baseurl + "api/AddUserRemarks", content);

                    var result = await response.Content.ReadAsStringAsync();
                    var parsed = JObject.Parse(result);
                    await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), parsed["Message"].ToString(), App.Btn_Close);
                    /*if ((int)response.StatusCode != 200)
                    {

                        await App.Current.MainPage.DisplayAlert( App.GetLabelByKey("AppName"), parsed["Message"].ToString(), App.Btn_Close);
                    }*/
                    return (int)response.StatusCode;
                }
                catch (Exception)
                {
                    await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", "OK");
                    return 500;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), App.NoInternet_, App.Btn_Close);
                return 101;
            }
        }

        public async Task<int> Remarks_Get(string ExpenseId)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var client = new HttpClient();
                    //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());
                    string url = baseurl + $"api/ViewRemarks?" +
                       $"ExpenseId={HttpUtility.UrlEncode(AESCryptography.EncryptAES(ExpenseId))}";
                    HttpResponseMessage response = await client.GetAsync(url);

                    var result = await response.Content.ReadAsStringAsync();
                    var parsed = JObject.Parse(result);

                    viewAllRemarksDatabase.DeleteViewAllRemarks();
                    if ((int)response.StatusCode == 200)
                    {
                        foreach (var pair in parsed)
                        {
                            if (pair.Key == "data")
                            {
                                var nodes = pair.Value;
                                foreach (var node in nodes)
                                {
                                    var item = new ViewAllRemarks();
                                    item.ExpenseId = AESCryptography.DecryptAES(node["ExpenseId"].ToString());
                                    item.UserRemarksId = AESCryptography.DecryptAES(node["UserRemarksId"].ToString());
                                    item.ObserverRemarksId = AESCryptography.DecryptAES(node["ObserverRemarksId"].ToString());
                                    item.ObserverRemarks = AESCryptography.DecryptAES(node["ObserverRemarks"].ToString());
                                    item.UserRemarks = AESCryptography.DecryptAES(node["UserRemarks"].ToString());
                                    item.UserRemarksDtTm = AESCryptography.DecryptAES(node["UserRemarksDtTm"].ToString());
                                    item.ObserverRemarksDtTm = AESCryptography.DecryptAES(node["ObserverRemarksDtTm"].ToString());
                                    viewAllRemarksDatabase.AddViewAllRemarks(item);
                                }
                            }
                        }
                    }
                    else if ((int)response.StatusCode == 404)
                    {
                        //await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), parsed["Message"].ToString(), App.Btn_Close);
                    }
                    return (int)response.StatusCode;
                }
                catch
                {
                    await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", "OK");
                    return 500;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), App.NoInternet_, App.Btn_Close);
                return 101;
            }
        }

        public async Task<int> UpdateObserverRemarks_Post(string expenseid, string ObserverRemarksId, string remarks)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                try
                {
                    var client = new HttpClient();
                    //var byteArray = Encoding.ASCII.GetBytes(App.basic_auth());
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetToken());

                    string jsonData = JsonConvert.SerializeObject(new
                    {
                        ExpenseId = AESCryptography.EncryptAES(expenseid),
                        ObserverRemarksId = AESCryptography.EncryptAES(ObserverRemarksId),
                        ObserverRemarks = AESCryptography.EncryptAES(remarks),
                    });

                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(baseurl + "api/UpdateObserverRemarks", content);

                    var result = await response.Content.ReadAsStringAsync();
                    var parsed = JObject.Parse(result);
                    await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), parsed["Message"].ToString(), App.Btn_Close);
                    /*if ((int)response.StatusCode != 200)
                    {

                        await App.Current.MainPage.DisplayAlert( App.GetLabelByKey("AppName"), parsed["Message"].ToString(), App.Btn_Close);
                    }*/
                    return (int)response.StatusCode;
                }
                catch (Exception)
                {
                    await Application.Current.MainPage.DisplayAlert("Exception", "Something went wrong. Please try again!", "OK");
                    return 500;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(App.GetLabelByKey("AppName"), App.NoInternet_, App.Btn_Close);
                return 101;
            }
        }
    }
}
