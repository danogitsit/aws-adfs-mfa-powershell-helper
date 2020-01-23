using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DO { 

    public partial class Browser : Form
    {

        public Browser()
        {
            InitializeComponent();
        } 

        private HtmlDocument HTMLContent;
        private string Passed;
        private Uri OrigURL;

        private void BrowserCtrl_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HTMLContent = BrowserCtrl.Document;
        }

        internal SAMLResponse LoadURL(string URL)
        {
            UriBuilder URLBuild = new UriBuilder(URL);
            OrigURL = URLBuild.Uri;
            BrowserCtrl.Url = URLBuild.Uri;

            this.ShowDialog();

            SAMLResponse response = new SAMLResponse();

            try
            {
                SAMLDocument respdocument = new SAMLDocument(HTMLContent);
                response.Response = respdocument;
            }
            catch
            {
                response.ResponseCode = SAMLResponse.ExitType.Warning;
            }

            try
            {
                response.ResponseRaw = HTMLContent.Body.InnerHtml;
                response.ResponseCode = SAMLResponse.ExitType.Success;
            }
            catch (Exception ex)
            {
                response.ResponseCode = SAMLResponse.ExitType.Failed;
            }

            return response;
        }

        private void BrowserCtrl_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            Passed = e.Url.AbsoluteUri;

            if (e.Url.DnsSafeHost != OrigURL.DnsSafeHost & e.Url.Scheme != "javascript")
            {
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}
internal struct SAMLResponse
{
    internal enum ExitType
    {
        Failed = 0x1,
        Success = 0x2,
        Warning = 0x3
    }
    public ExitType ResponseCode { get; set; }
    public string ResponseRaw { get; set; }
    public SAMLDocument Response { get; set; }
}

internal struct SAMLDocument
{
    private string _SessionValue;
    private string _SessionAction;
    private string _SessionMethod;


    internal SAMLDocument(HtmlDocument SAMLResponseHTML)
    {
        try
        {
            _SessionValue = SAMLResponseHTML.Forms[0].Children[0].GetAttribute("value");
            _SessionAction = SAMLResponseHTML.Forms[0].GetAttribute("action");
            _SessionMethod = SAMLResponseHTML.Forms[0].GetAttribute("method");
        }
        catch
        {
            throw new System.Exception("Could not load SAML data, the SAML Raw data is included in the response");
        }
    }

    internal string SessionValue => _SessionValue;

    internal string SessionAction => _SessionAction;

    internal string SessionMethod => _SessionMethod;
}
