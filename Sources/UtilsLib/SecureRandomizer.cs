using System;
using System.Security.Cryptography;


namespace ModelAppLib
{
    /// <summary>
    /// Une version sécurisée de gestion de nombres aléatoires
    /// </summary>
    public class SecureRandomizer : IRandomizer
    {
        public int GetRandomInt(int min, int max)
        {
            byte[] bytes = new byte[sizeof(int)];
            RandomNumberGenerator.Create().GetBytes(bytes);
            UInt32 scale = BitConverter.ToUInt32(bytes, 0);
            int val = (int)(min + (max - min) * (scale / (uint.MaxValue + 1.0)));
            return val;
        }
    }
}
