﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Partner.Billing.V2.Demo.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a single Azure storage blob with billing data
    /// </summary>
    public class BillingBlob
    {
        public string Name { get; set; }

        public string PartitionValue { get; set; }

        public long SizeInBytes { get; set; }

        public long ItemCount { get; set; }
    }
}
