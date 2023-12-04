// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Partner.Billing.V2.Demo
{
    using Azure;
    using Azure.Storage.Blobs;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Partner.Billing.V2.Demo.Enums;
    using Microsoft.Partner.Billing.V2.Demo.HttpRequest;
    using Microsoft.Partner.Billing.V2.Demo.Models;
    using Microsoft.Partner.Billing.V2.Demo.Providers;
    using Microsoft.Partner.Billing.V2.Demo.Services;
    using Newtonsoft.Json;
    using System;
    using System.IO.Compression;
    using JsonSerializer = Newtonsoft.Json.JsonSerializer;

    public class Program
    {
        private static string accessToken = ""; //update MS Graph authentication token value
        private static string invoiceid = ""; //update invoiceid

        private static string downloadPath = "";//update local path for download files
        private static string extractUsageFilesPath = "";//update local path for  extract GZ files.
        private static int sqlMaxRowInsert = 100;// max number of rows that will be saved in single SQL DB call
                                                                  

        private static AttributeSet fragment = AttributeSet.Full; // update fragment value either Full or Basic
        private static IBillingUsageService? billingUsageService = null;

        static async Task Main(string[] args)
        {
            try
            {
                //setup
                var services = new ServiceCollection();
                InitializeDependency(services);

                // initiate sequence of Graph API calls to get Manifest details from
                await GetBilledUsage(accessToken, invoiceid, fragment);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Get billed rated usage line items for the closed billing period
        /// </summary>
        /// <param name="accessToken">bearer token for authentication</param>
        /// <param name="invoiceid">InvoiceId</param>
        /// <param name="fragment"></param>
        /// <returns></returns>
        public static async Task GetBilledUsage(string accessToken, string invoiceid, AttributeSet fragment)
        {
            try
            {
                // Step : 1# Get manifest using Graph APIs,
                var manifest = await billingUsageService.GetManifestForBilledUsage(accessToken, invoiceid, fragment);

                if (manifest == null)
                {
                    throw new Exception("Manifest file is not ready yet. Please try after sometime");
                }

                // Step : 2# Get actual usage data/files from Azure blob storage
                var rootDirectorySAS = manifest.SASToken;
                var rootDirectory = manifest.RootDirectory;
                var blobs = manifest.Blobs;
                await DownloadBlob(rootDirectory ,rootDirectorySAS, blobs);

                Console.WriteLine("Download is completed");

                // Step : 3# Process usage files after files are downloaded.
                ExtractGZFiles();

                // Step : 4# Consume usage data as per consumer needs
                // this sample shows to insert usage data in SQL server
                IngestBilledUsageDataIntoDB();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void IngestBilledUsageDataIntoDB()
        {
            var billedUsages = new List<BilledUsage>();
            var d = new DirectoryInfo(extractUsageFilesPath);

            FileInfo[] Files = d.GetFiles("*.json");

            foreach (FileInfo file in Files)
            {
                // read file as stream to avoid loading large file in memory
                var reader = new StreamReader(file.FullName);
              
                var jsonReader = new JsonTextReader(reader)
                {
                    //data is in JSON Line format
                    SupportMultipleContent = true
                };

                var jsonSerializer = new JsonSerializer();
                int rowProcessed = 0;
                while (jsonReader.Read())
                {
                    rowProcessed++;
                    var billedUsage = jsonSerializer.Deserialize<BilledUsage>(jsonReader);
                    billedUsages.Add(billedUsage);

                    if(rowProcessed == sqlMaxRowInsert)
                    {
                        // insert rows processed till now into sql
                        if (billedUsages.Any())
                        {
                            billingUsageService.IngestBilledUsageData(billedUsages);
                            Console.WriteLine("Number of billing usage recon records are inerted into sql server db is {0}", billedUsages.Count);
                            billedUsages = new List<BilledUsage>();
                            rowProcessed = 0;
                        }
                    }
                }
            }           
        }

        private static void InitializeDependency(IServiceCollection services)
        {
            services.AddScoped<IBillingUsageService, BillingUsageService>();
            services.AddScoped<IBillingProvider, BillingProvider>();
            services.AddScoped<IHttpRequestHandler, HttpRequestHandler>();
            services.AddScoped<SqlServerProvider>();
            var serviceProvider = services.BuildServiceProvider();
            billingUsageService = serviceProvider.GetService<IBillingUsageService>();
        }

        /// <summary>
        /// Download files from storage account using Microsoft Azure Storage SDK
        /// </summary>
        /// <param name="rootFolderSAS"></param>
        /// <param name="blobs"></param>
        private static async Task DownloadBlob(string blobDirectory, string rootDirectorySAS, IReadOnlyList<BillingBlob> blobs)
        {
            AzureSasCredential credential = new AzureSasCredential(rootDirectorySAS);

            foreach (var blob in blobs)
            {
                var blobPath = $"{blobDirectory}/{blob.Name}";
                BlobClient blobClient = new BlobClient(new Uri(blobPath), credential);

                var localFilePath = @$"{downloadPath}\{blob.Name}";

                await blobClient.DownloadToAsync(localFilePath);               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static void ExtractGZFiles()
        {
            var d = new DirectoryInfo(downloadPath);

            FileInfo[] Files = d.GetFiles("*.json.gz");

            foreach (FileInfo file in Files)
            {
                using FileStream compressedFileStream = File.Open(file.FullName, FileMode.Open);
                var extacrFileName = file.Name.Replace(file.Extension, "");
                using FileStream outputFileStream = File.Create(extractUsageFilesPath + extacrFileName);
                using var decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress);
                decompressor.CopyTo(outputFileStream);
            }
        }
    }

}

