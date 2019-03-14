
namespace SlackTalk.Models
{
    /// <summary>
    /// All Web API responses contain a JSON object including some or all of these fields.
    /// </summary>
    public class SlackAPIResponse
    {     
        /// <summary>
        /// Top-level boolean property ok, indicating success or failure.
        /// </summary>
        public bool ok { get; set; }
        
        /// <summary>
        /// For failure results, the error property will contain a short machine-readable error code. 
        /// </summary>
        public string error { get; set; }

        /// <summary>
        /// In the case of problematic calls that could still be completed successfully, <see cref="ok"/> will be true and the <see cref="warning"/> property will contain a short machine-readable warning code (or comma-separated list of them, in the case of multiple warnings). 
        /// </summary>
        public string warning { get; set; }
    }
}