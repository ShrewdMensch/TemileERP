using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Domain;

namespace Utility.Comparer
{
    public class PersonnelPayrollComparer : IEqualityComparer<Payroll>
    {
        public bool Equals([AllowNull] Payroll x, [AllowNull] Payroll y)
        {
            if (x.PersonnelId == y.PersonnelId)
                return true;


            return false;
        }

        public int GetHashCode([DisallowNull] Payroll payroll)
        {
            int hashPersonnelId = payroll.PersonnelId.GetHashCode();

            return hashPersonnelId;
        }
    }
}