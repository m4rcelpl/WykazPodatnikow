using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BialaLista.data
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class Entity
    {
        /// <summary>
        /// Firma (nazwa) lub imię i nazwisko
        /// </summary>
        /// <value>Firma (nazwa) lub imię i nazwisko </value>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Nip
        /// </summary>
        [DataMember(Name = "nip", EmitDefaultValue = false)]
        public string Nip { get; set; }

        /// <summary>
        /// Status podatnika VAT.
        /// </summary>
        /// <value>Status podatnika VAT. </value>
        [DataMember(Name = "statusVat", EmitDefaultValue = false)]
        public string StatusVat { get; set; }

        /// <summary>
        /// Numer identyfikacyjny REGON
        /// </summary>
        /// <value>Numer identyfikacyjny REGON </value>
        [DataMember(Name = "regon", EmitDefaultValue = false)]
        public string Regon { get; set; }

        /// <summary>
        /// Gets or Sets Pesel
        /// </summary>
        [DataMember(Name = "pesel", EmitDefaultValue = false)]
        public Pesel Pesel { get; set; }

        /// <summary>
        /// numer KRS jeżeli został nadany
        /// </summary>
        /// <value>numer KRS jeżeli został nadany </value>
        [DataMember(Name = "krs", EmitDefaultValue = false)]
        public string Krs { get; set; }

        /// <summary>
        /// Adres siedziby
        /// </summary>
        /// <value>Adres siedziby </value>
        [DataMember(Name = "residenceAddress", EmitDefaultValue = false)]
        public string ResidenceAddress { get; set; }

        /// <summary>
        /// Adres stałego miejsca prowadzenia działalności lub adres miejsca zamieszkania w przypadku braku adresu stałego miejsca prowadzenia działalności
        /// </summary>
        /// <value>Adres stałego miejsca prowadzenia działalności lub adres miejsca zamieszkania w przypadku braku adresu stałego miejsca prowadzenia działalności </value>
        [DataMember(Name = "workingAddress", EmitDefaultValue = false)]
        public string WorkingAddress { get; set; }

        /// <summary>
        /// Imiona i nazwiska osób wchodzących w skład organu uprawnionego do reprezentowania podmiotu oraz ich numery NIP i/lub PESEL
        /// </summary>
        /// <value>Imiona i nazwiska osób wchodzących w skład organu uprawnionego do reprezentowania podmiotu oraz ich numery NIP i/lub PESEL </value>
        [DataMember(Name = "representatives", EmitDefaultValue = false)]
        public List<Person> Representatives { get; set; }

        /// <summary>
        /// Imiona i nazwiska prokurentów oraz ich numery NIP i/lub PESEL
        /// </summary>
        /// <value>Imiona i nazwiska prokurentów oraz ich numery NIP i/lub PESEL </value>
        [DataMember(Name = "authorizedClerks", EmitDefaultValue = false)]
        public List<Person> AuthorizedClerks { get; set; }

        /// <summary>
        /// Imiona i nazwiska lub firmę (nazwa) wspólnika oraz jego numeryNIP i/lub PESEL
        /// </summary>
        /// <value>Imiona i nazwiska lub firmę (nazwa) wspólnika oraz jego numeryNIP i/lub PESEL </value>
        [DataMember(Name = "partners", EmitDefaultValue = false)]
        public List<EntityPerson> Partners { get; set; }

        /// <summary>
        /// Data rejestracji jako podatnika VAT
        /// </summary>
        /// <value>Data rejestracji jako podatnika VAT </value>
        [DataMember(Name = "registrationLegalDate", EmitDefaultValue = false)]
        public DateTime? RegistrationLegalDate { get; set; }

        /// <summary>
        /// Data odmowy rejestracji jako podatnika VAT
        /// </summary>
        /// <value>Data odmowy rejestracji jako podatnika VAT </value>
        [DataMember(Name = "registrationDenialDate", EmitDefaultValue = false)]
        public DateTime? RegistrationDenialDate { get; set; }

        /// <summary>
        /// Podstawa prawna odmowy rejestracji
        /// </summary>
        /// <value>Podstawa prawna odmowy rejestracji </value>
        [DataMember(Name = "registrationDenialBasis", EmitDefaultValue = false)]
        public string RegistrationDenialBasis { get; set; }

        /// <summary>
        /// Data przywrócenia jako podatnika VAT
        /// </summary>
        /// <value>Data przywrócenia jako podatnika VAT </value>
        [DataMember(Name = "restorationDate", EmitDefaultValue = false)]
        public DateTime? RestorationDate { get; set; }

        /// <summary>
        /// Podstawa prawna przywrócenia jako podatnika VAT
        /// </summary>
        /// <value>Podstawa prawna przywrócenia jako podatnika VAT </value>
        [DataMember(Name = "restorationBasis", EmitDefaultValue = false)]
        public string RestorationBasis { get; set; }

        /// <summary>
        /// Data wykreślenia odmowy rejestracji jako podatnika VAT
        /// </summary>
        /// <value>Data wykreślenia odmowy rejestracji jako podatnika VAT </value>
        [DataMember(Name = "removalDate", EmitDefaultValue = false)]
        public DateTime? RemovalDate { get; set; }

        /// <summary>
        /// Podstawa prawna wykreślenia odmowy rejestracji jako podatnika VAT
        /// </summary>
        /// <value>Podstawa prawna wykreślenia odmowy rejestracji jako podatnika VAT </value>
        [DataMember(Name = "removalBasis", EmitDefaultValue = false)]
        public string RemovalBasis { get; set; }

        /// <summary>
        /// Gets or Sets AccountNumbers
        /// </summary>
        [DataMember(Name = "accountNumbers", EmitDefaultValue = false)]
        public List<string> AccountNumbers { get; set; }

        /// <summary>
        /// Podmiot posiada maski kont wirtualnych
        /// </summary>
        /// <value>Podmiot posiada maski kont wirtualnych </value>
        [DataMember(Name = "hasVirtualAccounts", EmitDefaultValue = false)]
        public bool? HasVirtualAccounts { get; set; }
    }
}