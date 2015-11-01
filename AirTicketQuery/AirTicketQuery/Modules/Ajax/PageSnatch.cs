using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using AirTicketQuery.Modules.Code;

namespace AirTicketQuery.Modules.Ajax
{
    public class PageSnatch
    {
        delegate void BrowserEventHandler();
        private int hitCount = 0;
        public string Navigate(string url, int timeout)
        {
            string gethtml = string.Empty;
            try
            {
                int interval = 500;
                using (WebBrowser browser = new WebBrowser())
                {
                    browser.ScriptErrorsSuppressed = false;                   
                    DateTime startTime = DateTime.Now;
                    bool isbusy = true;
                    int length = 0;
                    //browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(browser_DocumentCompleted);
                    //browser.Navigating += new WebBrowserNavigatingEventHandler(browser_Navigating);
                    browser.Navigate(url);
                    while (browser.ReadyState != WebBrowserReadyState.Complete)
                    {
                        Application.DoEvents();
                        System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " Still Loading");
                    }

                    while (hitCount < 10)
                    {
                        double t = Math.Ceiling((DateTime.Now - startTime).TotalSeconds);
                        if (t >= timeout)
                        {
                            throw new Exception("Visiting about new exception delay, since the setting is timeout");
                        }

                        BrowserEventHandler browserEventHanler = delegate() { isbusy = !browser.IsBusy; };
                        browser.Invoke(browserEventHanler);

                        if (browser.Document.All["flight-info"] != null)
                        {
                            int len = 0;
                            if (!string.IsNullOrEmpty(browser.Document.All["flight-info"].InnerHtml))
                                len = browser.Document.All["flight-info"].InnerHtml.Length;
                            if (len == length)
                            {
                                hitCount++;
                            }
                            else
                            {
                                hitCount = 0; length = len;
                            }

                            //System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "->flight-info-:InnerHtml" + browser.Document.All["flight-info"].InnerHtml);
                        }

                        if (!string.IsNullOrEmpty(browser.Document.All["flight-info"].InnerHtml))
                            length = browser.Document.All["flight-info"].InnerHtml.Length;
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(interval);
                    }

                    if (browser.Document.All["flight-info"] != null)
                    {
                        System.Diagnostics.Debug.Write("=".PadLeft(50, '='));
                        System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "->flight-info-:InnerHtml" + browser.Document.All["flight-info"].InnerHtml);
                        gethtml = browser.Document.All["flight-info"].InnerHtml;
                    }

                    //var htmldocument = (mshtml.HTMLDocument)browser.Document.DomDocument; System.Diagnostics.Debug.Write("=".PadLeft(50, '='));
                    //System.Diagnostics.Debug.WriteLine(htmldocument.documentElement.outerHTML); System.Diagnostics.Debug.Write("=".PadLeft(50, '='));
                    //System.Diagnostics.Debug.WriteLine(browser.Document.Body.OuterHtml);
                }
            }
            catch (Exception ex)
            {
                Log.LogErr(ex);
            }

            return gethtml;
        }

        //private void browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        //{
        //    //Log.LogInfo("browser_Navigating called.")
        //    System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "browser_Navigating called.");
        //    hitCount++;
        //}

        //private void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        //{
        //    System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "browser_DocumentCompleted called.");
        //    hitCount++;
        //}
    }
}