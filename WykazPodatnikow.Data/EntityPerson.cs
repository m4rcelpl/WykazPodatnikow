using System.Runtime.Serialization;

namespace WykazPodatnikow.Data
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class EntityPerson
    {
        /// <summary>
        /// Gets or Sets CompanyName
        /// </summary>
        [DataMember(Name = "companyName", EmitDefaultValue = false)]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or Sets FirstName
        /// </summary>
        [DataMember(Name = "firstName", EmitDefaultValue = false)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or Sets LastName
        /// </summary>
        [DataMember(Name = "lastName", EmitDefaultValue = false)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or Sets Pesel
        /// </summary>
        [DataMember(Name = "pesel", EmitDefaultValue = false)]
        public Pesel Pesel { get; set; }

        /// <summary>
        /// Gets or Sets Nip
        /// </summary>
        [DataMember(Name = "nip", EmitDefaultValue = false)]
        public string Nip { get; set; }
    }
}