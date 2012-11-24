using System.Collections.Generic;

namespace Problem3
{
    public static class CongruentialMethod
    {
        public static List<float> MixedCongruential(int modulus, int seed, int multiplier, int increment)
        {
            var randomNumbers = new List<float> ();

            long numb = (multiplier * seed) + increment;
            randomNumbers.Add(((numb) % modulus) / (float)modulus);

            do
            {
                numb = (multiplier*numb) + increment;
                randomNumbers.Add(((numb)%modulus)/(float) modulus);

            } while (randomNumbers[0] != randomNumbers[randomNumbers.Count - 1]);


            return randomNumbers;
        }
        public static List<float> MultiplicativeCongruential(int modulus, int seed, int multiplier)
        {

            return new List<float>();
        }
    }
}