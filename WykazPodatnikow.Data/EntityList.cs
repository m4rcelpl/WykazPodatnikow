using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WykazPodatnikow.Data
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class EntityList
    {
        /// <summary>
        /// Lista podmiotów
        /// </summary>
        /// <value>Lista podmiotów </value>
        [DataMember(Name = "subjects", EmitDefaultValue = false)]
        public List<Entity> Subjects { get; set; }

        /// <summary>
        /// Gets or Sets RequestId
        /// </summary>
        [DataMember(Name = "requestId", EmitDefaultValue = false)]
        public string RequestId { get; set; }
    }
}