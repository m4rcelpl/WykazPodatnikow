using System.Runtime.Serialization;

namespace WykazPodatnikow.Data
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class EntityPerson : Person
    {
        /// <summary>
        /// Gets or Sets CompanyName
        /// </summary>
        [DataMember(Name = "companyName", EmitDefaultValue = false)]
        public string CompanyName { get; set; }
    }
}