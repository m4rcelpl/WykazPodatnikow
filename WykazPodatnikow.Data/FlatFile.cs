using System;
using System.Collections.Generic;
using System.Text;

namespace WykazPodatnikow.Data
{
    public enum FlatFile
    {
        FoundInActiveVatPayer,
        FoundInExemptVatPayer,
        InvalidNip,
        InvalidBankAccount,
        NotFound
    }
}
