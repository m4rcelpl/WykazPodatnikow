using System.Runtime.Serialization;
using System.Text;

namespace WykazPodatnikow.Data
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class EntityItem
    {
        /// <summary>
        /// Gets or Sets Subject
        /// </summary>
        [DataMember(Name = "subject", EmitDefaultValue = false)]
        public Entity Subject { get; set; }

        /// <summary>
        /// Gets or Sets RequestId
        /// </summary>
        [DataMember(Name = "requestId", EmitDefaultValue = false)]
        public string RequestId { get; set; }
    }
}