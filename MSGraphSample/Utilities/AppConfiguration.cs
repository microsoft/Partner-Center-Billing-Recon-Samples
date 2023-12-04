﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Partner.Billing.V2.Demo.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class AppConfiguration
    {
        
        /// <summary>
        /// Get database connection string from app.config
        /// </summary>
        /// <returns></returns>
        public static string GetDBConnectionString()
        {
           return ConfigurationManager.ConnectionStrings["dbconnection"].ToString();
        }

        /// <summary>
        /// Get api url
        /// </summary>
        /// <returns></returns>
        public static string GetAPIRootURL()
        {
            return ConfigurationManager.AppSettings["graphRooturl"].ToString();
        }

        /// <summary>
        /// Get table name for ingesting billedusage data into database table.
        /// </summary>
        /// <returns></returns>
        public static string GetBilledUsageTableName()
        {
            return ConfigurationManager.AppSettings["billedusagetablename"].ToString();
        }
    }
}
