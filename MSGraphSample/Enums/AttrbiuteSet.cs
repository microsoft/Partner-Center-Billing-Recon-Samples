// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Partner.Billing.V2.Demo.Enums
{
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AttributeSet
    {
        /// <summary>
        /// Get all attrbiutes
        /// </summary>
        Full,

        /// <summary>
        /// limited set of attributes
        /// </summary>
        Basic
    }
}
