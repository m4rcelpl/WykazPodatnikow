using System.Runtime.Serialization;

namespace WykazPodatnikow.Data
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class EntityResponse
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
        public EntityItem Result { get; set; }
    }
}