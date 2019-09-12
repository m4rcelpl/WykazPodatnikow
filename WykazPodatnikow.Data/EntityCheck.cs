using System.Runtime.Serialization;

namespace WykazPodatnikow.Data
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class EntityCheck
    {
        /// <summary>
        /// Czy rachunek przypisany do podmiotu czynnego
        /// </summary>
        /// <value>Czy rachunek przypisany do podmiotu czynnego </value>
        [DataMember(Name = "accountAssigned", EmitDefaultValue = false)]
        public string AccountAssigned { get; set; }

        /// <summary>
        /// Gets or Sets RequestId
        /// </summary>
        [DataMember(Name = "requestId", EmitDefaultValue = false)]
        public string RequestId { get; set; }
    }
}