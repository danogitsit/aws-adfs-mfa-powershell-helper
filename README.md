## AWS ADFS MFA PowerShell Helper

The helper addresses issues when trying to use the AWS PowerShell tools and authenticating against an ADFS server that has MFA enabled. Currently, with the existing AWS PowerShell tools, the authentication process fails when MFA is enabled. This helper presents a "modern auth" method of authenticating with ADFS, processing your MFA challenge and subsequently providing your access key and secret so you can utilise the AWS PowerShell cmdlets.

Although intended for PowerShell, the helper can be used with anything that supports Windows DLLs. It is not designed to support surpressed or pass-through for MFA - this defeats the purpose. But allows you to build PowerShell scripts+ other things intended to be run by users, and requiring MFA with ADFS.

### Pre Reqs

The AWS PowerShell tools and AWS SDK for .NET is required to be installed on the computer running the helper. Both can be downloaded from here [https://aws.amazon.com/powershell/](https://aws.amazon.com/powershell/).

### How it Works

The helper effectively takes in the path to the ADFS page for AWS, the AWS provider ARN and role ARN, presents a web browser control, detects when ADFS is navigating away to the AWS page, passes the SAML token to the AWS SDK, generates the session credentials, passes back to PowerShell.

### Building the Helper

1. Download the code from the repository
2. Open the .SLN file using Visual Studio
3. Build the solution
4. Take the DLL from the built solution place it somewhere you can reference it from your PowerShell code

### Sample PowerShell Code

```markdown
# The path to the DLL built from the Visual Studio project in this repo

$AWSADFSHelperDLLPath = "C:\path\aws-adfs-mfa-powershell-helper\bin\Debug\aws-adfs-mfa-powershell-helper.dll"

# The path to the AWS PowerShell tools

$AWSCmdDir = "C:\Program Files (x86)\AWS Tools\PowerShell\AWSPowerShell\"
$AWSCmdLine = "Initialize-AWSDefaultConfiguration"

# The ADFS and role settings

$AWSSAMLEndpoint = "https://adfs.yourdomain.com/adfs/ls/IdpInitiatedSignOn.aspx?loginToRp=urn:amazon:webservices"
$PrincipalARN = "arn:aws:iam::000000000000:saml-provider/YourProvider"
$ARNRoleName = "arn:aws:iam::000000000000:role/YourRole"

# Invoke the AWS PowerShell tools

$ppath = (Get-Location).Path

Set-Location $AWSCmdDir
invoke-expression $AWSCmdLine -ErrorAction Stop
Set-Location $ppath

# Load the helper assembly

$assembly = [Reflection.Assembly]::LoadFile($AWSADFSHelperDLLPath)

# Invoke the helper objects

$ADFS = new-object ADFSAWSCredential
$ADFScreds = new-object ADFSAWSCredential+AWSCredentials

# Attempt accessing credentials, where the magic happens

$ADFScreds = $ADFS.GetCredential($AWSSAMLEndpoint, $ARNRoleName, $PrincipalARN)

# Set AWS PowerShell tool credentials

Set-AWSCredentials -AccessKey $ADFScreds.AccessKeyID -SecretKey $ADFScreds.SecretAccessKey -SessionToken $ADFScreds.SessionToken -StoreAs Default -ErrorAction stop

# Bin the ADFS creds variable

$ADFScreds = $null

# Run any AWS PowerShell

Get-S3Bucket

```

## Limitations

**DOES NOT** provide a choice for available AWS roles, a single role must be passed to the helper

Tested using;

* ADFS 2016 with an MFA adapter (should work with any that stay within the ADFS interface)
* AWS Tools for PowerShell 4.0.2.0
* AWS SDK for .NET Version 3 (3.3.666.0)
* Built with Visual Studio 2017 Community Edition

### Support or Contact

Get in touch in the comments or via Twitter
