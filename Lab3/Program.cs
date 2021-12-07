using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using Extreme.Mathematics;

namespace Lab3
{
    class Program
    {

        public static string id;
        static void Main(string[] args)
        {
            LcgCrack();
            //MTCrack();
            //BetterMTCrack();
        }

        public static void BetterMTCrack()
        {
            Acount acount = createAccount();
            ulong[] numbers = new ulong[624];
            for (int i = 0; i < 624; i++)
            {
                numbers[i] = (ulong)makeBet("BetterMt", 1, 1);
            }

            MT19937 MT = new (numbers);
            
            for (int i = 0; i < 10; i++)
            {
                long predictedNumber = (long) MT.genrand_int32();
                Console.WriteLine("Predicted number = " + predictedNumber);
                makeBet("BetterMt", 110, predictedNumber);
            }
            

        }
        
        
        public static void MTCrack()
        {
            Acount acount = createAccount();
            DateTimeOffset  time = acount.deletionTime;
            long number = makeBet("Mt", 1, 1);
            ulong seed = (ulong) time.ToUnixTimeSeconds() - 3600;
            long predictedNumber = 0;
            MT19937 MT;
            do{
                MT = new MT19937(seed);
                predictedNumber = (long)MT.genrand_int32();
                Console.WriteLine(predictedNumber);
                seed++;
            } while (predictedNumber!=number);

            for (int i = 0; i < 10; i++)
            {
                makeBet("Mt",110,(long)MT.genrand_int32());
            }
            


        }
        public static void LcgCrack()
        {
            createAccount();
            
            BigInteger[] numbers = new BigInteger[3];
            for (int i = 0; i < 3; i++)
            {
                numbers[i] = new BigInteger(makeBet("Lcg", 1,1));
            }

            LCG lcg = new LCG();
            lcg.findVariables(numbers);
            Console.WriteLine("a = " + lcg.a);
            Console.WriteLine("c = " + lcg.c);

            BigInteger next = lcg.next(numbers[2], lcg.a, lcg.c);
            
            Console.WriteLine("Predicted next number = " + (int)next);
            makeBet("Lcg", 500, (int)next);
            BigInteger next1 = lcg.next(next, lcg.a, lcg.c);
            Console.WriteLine("Predicted next number = " + (int)next1);
            makeBet("Lcg", 501, (int)next1);
        }

        
        public static  Acount createAccount()
        {
            Random random = new Random();
            id = random.Next(20000).ToString();
            var request = WebRequest.Create($"http://95.217.177.249/casino/createacc?id={id}");
            request.Method = "GET";

            using var webResponse = request.GetResponse();
            using var webStream = webResponse.GetResponseStream();

            using var reader = new StreamReader(webStream);
            var data = reader.ReadToEnd();

            Console.WriteLine(data);
            Acount acount = JsonConvert.DeserializeObject<Acount>(data);

            return acount;
        }

        public static long makeBet(string mode, int betAmount, long number)
        {
            var request = WebRequest.Create($"http://95.217.177.249/casino/play{mode}?id={id}&bet={betAmount}&number={number}");
            request.Method = "GET";

            using var webResponse = request.GetResponse();
            using var webStream = webResponse.GetResponseStream();

            using var reader = new StreamReader(webStream);
            var data = reader.ReadToEnd();

            Console.WriteLine(data);
            Bet bet = JsonConvert.DeserializeObject<Bet>(data);

            return bet.realNumber;
        }
    } 
}