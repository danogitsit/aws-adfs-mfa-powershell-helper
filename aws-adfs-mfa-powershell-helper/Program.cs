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
using Microsoft.VisualBasic;
using Amazon.SecurityToken;
using Amazon.Runtime;
using Amazon.SecurityToken.Model;
using DO;

public class ADFSAWSCredential
{
    public AWSCredentials GetCredential(string IdentityURL, string RoleARN, string ProviderARN)
    {
        Browser ADFSBrowser = new Browser();
        SAMLResponse res = ADFSBrowser.LoadURL(IdentityURL);
        string[] role = new string[] { ProviderARN, RoleARN };

        ImmutableCredentials creds;
        AWSCredentials AWSCreds = new AWSCredentials();

        switch (res.ResponseCode)
        {
            case SAMLResponse.ExitType.Success:

                creds = GetSamlRoleCredentails(res.Response.SessionValue, role).GetCredentials();

                AWSCreds.AccessKeyID = creds.AccessKey;
                AWSCreds.SecretAccessKey = creds.SecretKey;
                AWSCreds.SessionToken = creds.Token;

                break;

            case SAMLResponse.ExitType.Warning:

                throw new System.Exception("SAML generation failed because the SAML data was only partially recieved. Check you have the correct permissions to the ARN roles");

                break;

            case SAMLResponse.ExitType.Failed:

               throw new System.Exception("SAML generation failed because no SAML data was recieved. Check the Identity URL is correct and reachable.");

                break;
        }
        return AWSCreds;
    }


    private SessionAWSCredentials GetSamlRoleCredentails(string samlAssertion, string[] awsRole)
    {
        AssumeRoleWithSAMLRequest samlRequest = new AssumeRoleWithSAMLRequest();
        samlRequest.SAMLAssertion = samlAssertion;
        samlRequest.RoleArn = awsRole[1];
        samlRequest.PrincipalArn = awsRole[0];
        samlRequest.DurationSeconds = 3600;
        AmazonSecurityTokenServiceClient sts;
        AssumeRoleWithSAMLResponse samlResponse;

        try
        {
            sts = new AmazonSecurityTokenServiceClient();
            samlResponse = sts.AssumeRoleWithSAML(samlRequest);
        }
        catch
        {
            sts = new AmazonSecurityTokenServiceClient("a", "b", "c");
            samlResponse = sts.AssumeRoleWithSAML(samlRequest);
        }

        SessionAWSCredentials sessionCredentials = new SessionAWSCredentials(samlResponse.Credentials.AccessKeyId, samlResponse.Credentials.SecretAccessKey, samlResponse.Credentials.SessionToken);
        return sessionCredentials;
    }


    public struct AWSCredentials
    {
        public string AccessKeyID { get; set; }
        public string SecretAccessKey { get; set; }
        public string SessionToken { get; set; }
    }
}
