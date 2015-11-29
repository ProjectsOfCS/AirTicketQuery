using System;
using System.Windows.Forms;
using AirTicketQuery.Modules.Code;

namespace AirTicketQuery.Modules.Ajax
{
    /// <summary>
    /// This class used for get the html source when the page contain Ajax.
    /// </summary>
    public class PageSnatch
    {
        delegate void BrowserEventHandler();
        private int hitCount = 0;
        public string Navigate(string url, int timeout, string flightHtmlElementID)
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
                    browser.Navigate(url);
                    while (browser.ReadyState != WebBrowserReadyState.Complete)
                    {
                        Application.DoEvents();
                        System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " Still Loading");
                        System.Threading.Thread.Sleep(interval);
                        double t = Math.Ceiling((DateTime.Now - startTime).TotalSeconds);
                        if (t >= timeout)
                        {
                            throw new Exception("Visiting about new exception delay, since the setting is timeout");
                        }
                    }

                    while (hitCount < 4)
                    {
                        double t = Math.Ceiling((DateTime.Now - startTime).TotalSeconds);
                        if (t >= timeout)
                        {
                            throw new Exception("Visiting about new exception delay, since the setting is timeout");
                        }

                        BrowserEventHandler browserEventHanler = delegate() { isbusy = !browser.IsBusy; };
                        browser.Invoke(browserEventHanler);

                        if (browser.Document.All[flightHtmlElementID] != null)
                        {
                            int len = 0;
                            if (!string.IsNullOrEmpty(browser.Document.All[flightHtmlElementID].InnerHtml))
                                len = browser.Document.All[flightHtmlElementID].InnerHtml.Length;
                            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + string.Format(" hitCnt:{0};len:{1};length:{2}", hitCount, len, length));

                            if (len == length)
                            {
                                hitCount++;
                            }
                            else
                            {
                                hitCount = 0; length = len;
                            }
                        }

                        if (!string.IsNullOrEmpty(browser.Document.All[flightHtmlElementID].InnerHtml))
                            length = browser.Document.All[flightHtmlElementID].InnerHtml.Length;
                        System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " begin DoEvents and Sleep");
                        //Application.DoEvents();
                        System.Threading.Thread.Sleep(interval);
                        System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " end DoEvents and Sleep");
                    }

                    if (browser.Document.All[flightHtmlElementID] != null)
                    {
                        gethtml = browser.Document.All[flightHtmlElementID].InnerHtml;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogErr(ex);
            }

            return gethtml;
        }
    }
}