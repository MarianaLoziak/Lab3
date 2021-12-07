using System;
using Extreme.Mathematics;


namespace Lab3
{
    public class LCG
    {
        public BigInteger a;
        public BigInteger c;
        public BigInteger modulus = new BigInteger(Math.Pow(2, 32));

        public void findVariables(BigInteger[] numbers)
        {
            BigInteger diff1 = BigInteger.Subtract(numbers[2], numbers[1]);
            BigInteger diff2 = BigInteger.Subtract(numbers[1], numbers[0]);
            BigInteger inverse = BigInteger.ModularInverse(diff2, modulus);

            a = BigInteger.Mod(BigInteger.Multiply(diff1, inverse), modulus);
            c = BigInteger.Mod(BigInteger.Subtract(numbers[1], BigInteger.Multiply(a, numbers[0])),
                modulus);
        }

        public BigInteger next(BigInteger last, BigInteger a, BigInteger c)
        {
            last = BigInteger.Mod(BigInteger.Add(BigInteger.Multiply(a,last), c), modulus);
            return last;
        }

    }
}