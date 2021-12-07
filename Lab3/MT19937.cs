using System;

public class MT19937
{
        private const int N = 624;
        private const ulong M = 397;
        private const ulong MATRIX_A = 0x9908B0DFUL;		
        private const ulong UPPER_MASK = 0x80000000UL;		
        private const ulong LOWER_MASK = 0X7FFFFFFFUL;		
        private const ulong DEFAULT_SEED = 5489UL;

        private static ulong[] mt = new ulong[N + 1];	
        private static ulong mti = N + 1;

        public MT19937()
        {
            init_genrand(DEFAULT_SEED);
        }
        public MT19937(ulong s)
        {
            this.init_genrand(s);
        }

        public MT19937(ulong[] states)
        {
            //init_genrand(DEFAULT_SEED);
            init_genrand(states);
            mti = N;
        }
        
        

        
        public void init_genrand(ulong s)
        {
            mt[0] = s & 0xffffffffUL;
            for (mti = 1; mti < N; mti++)
            {
                mt[mti] = (1812433253UL * (mt[mti - 1] ^ (mt[mti - 1] >> 30)) + mti);
                mt[mti] &= 0xffffffffUL;
            }
        }

        public void init_genrand(ulong[] states)
        {
            ulong[] ustates = new ulong[states.Length];
            for (int i = 0; i < states.Length; i++)
            {
                ustates[i] = untempering(states[i]);
            }
            mt = ustates;
        }

       
        public ulong genrand_int32()
        {
            ulong y = 0;
            ulong[] mag01 = new ulong[2];
            mag01[0] = 0x0UL;
            mag01[1] = MATRIX_A;

            if (mti >= N)
            {
                
                ulong kk;

                if (mti == N + 1)   
                    this.init_genrand(DEFAULT_SEED); 

                for (kk = 0; kk < N - M; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1UL];
                }
                for (; kk < N - 1; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk - 227] ^ (y >> 1) ^ mag01[y & 0x1UL];
                }
                y = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
                mt[N - 1] = mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1UL];

                mti = 0;
            }

            y = mt[mti++];
            
            y = tempering(y);

            return y;
        }

        public ulong tempering(ulong y)
        {
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680UL;
            y ^= (y << 15) & 0xefc60000UL;
            y ^= (y >> 18);

            return y;
        }

        public ulong untempering(ulong y)
        {
            y ^= (y >> 18);
            y ^= (y << 15) & 0xefc60000UL;
            y ^=
                ((y << 7) & 0x9d2c5680UL) ^
                ((y << 14) & 0x94284000UL) ^
                ((y << 21) & 0x14200000UL) ^
                ((y << 28) & 0x10000000UL);
            y ^= (y >> 11) ^ (y >> 22);

            return y;
        }
}