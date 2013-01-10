using System.Collections.Generic;
using Microsoft.Scripting.Math;
using Problem3.Helper;

namespace Problem3
{
    public static class CongruentialMethod
    {
        public static ReportList MixedCongruential(int modulus, int seed, int multiplier, int increment)
        {
            var randomNumbers = new List<float>();

            BigInteger numb = (multiplier*seed) + increment;
            randomNumbers.Add(((float) (numb%modulus)/modulus));

            do
            {
                numb = ((multiplier*numb) + increment);
                float newNubmer = (float) ((numb)%modulus)/modulus;
                if (randomNumbers.Contains(newNubmer))
                    break;
                randomNumbers.Add(newNubmer);
            } while (true);
            return new ReportList(randomNumbers, modulus, seed, multiplier, increment);
        }

        public static ReportList MultiplicativeCongruential(int modulus, int seed, int multiplier)
        {
            return MixedCongruential(modulus, seed, multiplier, 0);
        }
    }
}