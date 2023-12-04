// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Partner.Billing.V2.Demo.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a manifest to get full billing data
    /// </summary>
    public class Manifest
    {
        public string Id { get; set; }  

        public string SchemaVersion { get; set; }

        public string DataFormat { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ETag { get; set; }

        public string PartnerTenantId { get; set; }

        public string RootDirectory { get; set; }

        public string SASToken { get; set; }

        public DataPartitionType PartitionType { get; set; }

        public int BlobCount { get; set; }       

        public IReadOnlyList<BillingBlob> Blobs { get; set; }
    }
}
