namespace Microsoft.Partner.Billing.V2.Demo.Models
{
    /// <summary>
    /// Success export operation 
    /// which provides manifest details to download files.
    /// </summary>
    public class ExportSuccessOperation : Operation
    {
        public Manifest ResourceLocation { get; set; }
    }
}
