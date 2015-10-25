using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace AirTicketQuery.Modules.Ajax
{
    public class PageSnatch
    {
        delegate void BrowserEventHandler();
        private readonly string url = string.Empty;
        private int timeout;
        private const int defaultTimeout = 10;

        public PageSnatch(string url)
        {
            this.timeout = defaultTimeout;
            this.url = url;
        }

        public PageSnatch(string url, int timeout)
            : this(url)
        {
            this.timeout = timeout;
        }

        public Exception Error { set; get; }
        public string Text { set; get; }
        public string TextAsync { set; get; }

        public void Navigate()
        {
            try
            {
                bool completed = false;
                int interval = 500;
                Thread thread = new Thread(delegate()
                {
                    using (WebBrowser browser = new WebBrowser())
                    {
                        browser.ScriptErrorsSuppressed = false;
                        browser.ScrollBarsEnabled = false;
                        browser.AllowNavigation = true;
                        DateTime startTime = DateTime.Now;

                        browser.DocumentCompleted += delegate(object sender, WebBrowserDocumentCompletedEventArgs e)
                        {
                            this.Text = browser.Document.Body.OuterHtml;
                            string url0 = browser.Document.Url.ToString();
                            completed = url0.Equals(e.Url.ToString());
                        };

                        browser.Navigate(url);
                        while (!completed)
                        {
                            System.Windows.Forms.Application.DoEvents();
                            System.Threading.Thread.Sleep(interval);
                        }

                        int count = 6;
                        int index = 0;
                        int length = 0;

                        bool isbusy = true;
                        while (isbusy)
                        {
                            System.Threading.Thread.Sleep(interval);
                            double t = Math.Ceiling((DateTime.Now - startTime).TotalMilliseconds);
                            if (t >= this.timeout)
                            {
                                this.Error = new Exception("Visiting about new exception delay, since the setting is timeout");
                                break;
                            }

                            BrowserEventHandler browserEventHanler = delegate() { isbusy = !browser.IsBusy; };
                            browser.Invoke(browserEventHanler);

                            if (!isbusy)
                            {
                                int len = browser.Document.Body.OuterHtml.Length;
                                if (len == length) { index++; }
                                else { index = 0; length = len; }
                                if (index == count) { isbusy = false; }
                            }

                            length = browser.Document.Body.OuterHtml.Length;
                            this.TextAsync = browser.Document.Body.OuterHtml;
                        }
                    }
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }
            catch (Exception ex) { throw ex; }
        }
    }
}