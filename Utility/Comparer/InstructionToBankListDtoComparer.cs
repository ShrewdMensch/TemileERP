using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Utility.DTOs;

namespace Utility.Comparer
{
    public class InstructionToBankListDtoComparer : IEqualityComparer<InstructionToBankListDto>
    {
        public bool Equals([AllowNull] InstructionToBankListDto x, [AllowNull] InstructionToBankListDto y)
        {
            if (x.Vessel == y.Vessel)
                return true;


            return false;
        }

        public int GetHashCode([DisallowNull] InstructionToBankListDto instructionToBankDto)
        {
            int hashVessel = instructionToBankDto.Vessel.GetHashCode();

            return hashVessel;
        }
    }
}