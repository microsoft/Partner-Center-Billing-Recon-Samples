using Microsoft.Partner.Billing.V2.Demo.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSGraphBillingSample.Models
{
    /// <summary>
    /// Gets the data for billed usage export operation
    /// </summary>
    public class BilledExportRequest
    {
        public string InvoiceId { get; set; }   

        public AttributeSet AttributeSet { get; set; }
    }
}
