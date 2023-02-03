using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace LuYao.Toolkit.Channels.Networks
{
    public partial class HttpProxyCheckerViewModel : ViewModelBase
    {
        public enum ResponseStatus
        {
            Ready,
            Running,
            OK,
            Fail
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(GetCommand))]
        [ViewStates.WatchViewState(nameof(Host))]
        private string _host = "127.0.0.1";

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(GetCommand))]
        [ViewStates.WatchViewState(nameof(Port))]
        private int _port = 10809;

        [ObservableProperty]
        [ViewStates.WatchViewState(nameof(Username))]
        private string _username;

        [ObservableProperty]
        [ViewStates.WatchViewState(nameof(Password))]
        private string _password;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(GetCommand))]
        [ViewStates.WatchViewState(nameof(RequestUrl))]
        private string _requestUrl = "https://www.coderbusy.com";

        [ObservableProperty]
        private ResponseStatus _status = ResponseStatus.Ready;

        [ObservableProperty]
        private Exception _exception;

        [ObservableProperty]
        private HttpResponseMessage _response;

        [ObservableProperty]
        private string _responseText;

        [ObservableProperty]
        private TimeSpan _cost = TimeSpan.Zero;


        private bool CanGet()
        {
            if (string.IsNullOrWhiteSpace(this.Host)) return false;
            if (this.Port < 1 && this.Port > 65535) return false;
            if (string.IsNullOrWhiteSpace(this.RequestUrl)) return false;
            if (!Uri.TryCreate(this.RequestUrl, UriKind.Absolute, out _)) return false;
            return true;
        }

        [RelayCommand(CanExecute = nameof(CanGet))]
        private async Task Get()
        {
            using var _ = this.Busy();
            var sw = new Stopwatch();
            try
            {
                this.Status = ResponseStatus.Running;
                var proxy = new WebProxy(this.Host, this.Port);
                if (!string.IsNullOrWhiteSpace(this.Username)) proxy.Credentials = new NetworkCredential(this.Username, this.Password);
                using var handler = new SocketsHttpHandler()
                {
                    UseProxy = true,
                    Proxy = proxy,
                    AutomaticDecompression = DecompressionMethods.All,
                    SslOptions = new SslClientAuthenticationOptions
                    {
                        RemoteCertificateValidationCallback = this.RemoteCertificateValidationCallback,
                    },
                    ConnectTimeout = TimeSpan.FromSeconds(10),
                    PooledConnectionLifetime = TimeSpan.FromSeconds(1000),
                };
                using var http = new HttpClient(handler, true);
                using var request = new HttpRequestMessage(HttpMethod.Get, this.RequestUrl);
                request.AddCommonHeader();
                sw.Start();
                var response = await http.SendAsync(request);
                var html = await response.ReadAsHtmlAsync();
                sw.Stop();
                this.Response = response;
                this.ResponseText = html;
                this.Status = ResponseStatus.OK;
            }
            catch (Exception e)
            {
                this.Exception = e;
                this.Status = ResponseStatus.Fail;
            }
            finally
            {
                if (sw.IsRunning) sw.Stop();
                this.Cost = sw.Elapsed;
            }
        }

        private bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
