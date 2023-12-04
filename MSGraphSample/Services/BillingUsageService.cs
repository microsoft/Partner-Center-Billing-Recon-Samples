﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Partner.Billing.V2.Demo.Services
{
    using Microsoft.Partner.Billing.V2.Demo.Enums;
    using Microsoft.Partner.Billing.V2.Demo.Models;
    using Microsoft.Partner.Billing.V2.Demo.Providers;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class BillingUsageService : IBillingUsageService
    {
        private readonly IBillingProvider billingProvider = null;

        public BillingUsageService(IBillingProvider _billingProvider)
        {
            this.billingProvider = _billingProvider;
        }

        /// <summary>
        /// Get billed rated usage line items for the closed billing period
        /// </summary>
        /// <param name="token"></param>
        /// <param name="invoiceId"></param>
        /// <param name="fragment"></param>
        /// <returns></returns>
        public async Task<Manifest> GetManifestForBilledUsage(string token, string invoiceId, AttributeSet fragment)
        {
            try
            {
                // Step 1#: POST to MS Graph API to create operation for latest data
                var operationLocationRequestUri = await this.billingProvider.SubmitRequest(token, invoiceId, fragment);

                bool shouldRetry = false;
                var noOfRetries = 5; 
                var retryCount = 1;

                do
                {
                    // Step 2# : Check if data is ready
                    var operationResult = await GetOperationStatus(token, operationLocationRequestUri);
                    retryCount++;
                    if (operationResult?.Status == OperationStatus.Succeeded)
                    {
                        // if data is ready, get manifest
                        shouldRetry = false;
                        return ((ExportSuccessOperation)operationResult).ResourceLocation;                        
                    }
                    else
                    {
                        // if data is not ready, wait and check data readiness status after suggested time
                        var retryAfter = operationResult?.RetryAfter;
                        await Task.Delay(retryAfter.Value);
                        shouldRetry = true;
                    }
                }

                // Based on consumer flow, it can either wait for more time for data readiness 
                // OR after some time initiate a new flow from Step 1
                while (shouldRetry && retryCount <= noOfRetries); //5 retries

                return null;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public void IngestBilledUsageData(IEnumerable<BilledUsage> billedUsages)
        {
            this.billingProvider.IngestBilledUsageData(billedUsages);
        }

        private async Task<Operation> GetOperationStatus(string token, string operationLocationRequestUri)
        {
           return await this.billingProvider.GetOperationStatus(token, operationLocationRequestUri);
        }
    }
}
