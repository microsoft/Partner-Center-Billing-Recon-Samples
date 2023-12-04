using MSGraphBillingSample.Models;

namespace Microsoft.Partner.Billing.V2.Demo.Models
{
    /// <summary>
    /// Export opreation that has failed
    /// </summary>
    public class ExportFailedOperation : Operation
    {
        public PublicError Error { get; set; }  
    }
}
