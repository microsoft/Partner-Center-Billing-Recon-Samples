// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Partner.Billing.V2.Demo.Services
{
    using Microsoft.Partner.Billing.V2.Demo.Enums;
    using Microsoft.Partner.Billing.V2.Demo.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBillingUsageService
    {
        Task<Manifest> GetManifestForBilledUsage(string token, string invoiceId, AttributeSet fragment);

        void IngestBilledUsageData(IEnumerable<BilledUsage> billedUsages);
    }
}
