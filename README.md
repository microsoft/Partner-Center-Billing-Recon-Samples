# Project

> This repo contains a samples showing usage of the new MS Graph async model APIs to get billing and recon data. For API details, please refer to the MS Graph docs.

## To run samples
### Prerequisites
- You need to have Visual studio 2022 with .NET 6.0 installed.
- Bearer token to access MS Graph API, with appropriate permission to get billing data.

After downloading the file, open the solution *MSGraphSample\MSGraphBillingSample.sln* and update below values in *program.cs* based on your scenario.
- accessToken: bearer token for authentication and authorization.
- invoiceid: invoiceid for which to get billed data. Invoices from September 2023 onwards are only available. Example G012040490
- downloadPath: local path where billing blobs should be downloaded. Example c:\downloads
- extractUsageFilesPath: local path where billing blobs after uncompression will be generated. Example c:\downloads\

Update below configuration values in *App.config*
- graphRooturl: 
    - for Beta endpoint use https://graph.microsoft.com/beta
    - for Prod endpoint use https://graph.microsoft.com/v1.0
- dbconnection: SQL db connection string where sample will insert downloaded data
- billedusagetablename: SQL table name in which data will be inserted. table schema should match with attributes in the downloaded file(s).


## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft 
trademarks or logos is subject to and must follow 
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.
