using System.Runtime.Serialization;

namespace BialaLista.data
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class EntityCheckResponse
    {
        /// <summary>
        /// Gets or Sets Exception
        /// </summary>
        [DataMember(Name = "exception", EmitDefaultValue = false)]
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or Sets Result
        /// </summary>
        [DataMember(Name = "result", EmitDefaultValue = false)]
        public EntityCheck Result { get; set; }
    }
}