using Newtonsoft.Json.Linq;

namespace CrystallineSociety.Server.Extensions
{
    public static class HashCode
    {
        public static int GetDeterministicHashCode(this string str)
        {
            unchecked
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < str?.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }

        public static int GetDeterministicHashCode(this Guid? guid)
        {
            unchecked
            {
               return GetDeterministicHashCode(guid.ToString());
            }
        }

        public static int GetDeterministicHashCode(this Guid guid)
        {
            unchecked
            {
                return GetDeterministicHashCode(guid.ToString());
            }
        }

        public static int GetDeterministicHashCode(this JToken token)
        {
            unchecked
            {
                return GetDeterministicHashCode(token.ToString());
            }
        }

        public static int GetDeterministicHashCode(this int value)
        {
            unchecked
            {
                return GetDeterministicHashCode(value.ToString());
            }
        }

        public static int GetDeterministicHashCode(this long value)
        {
            unchecked
            {
                return GetDeterministicHashCode(value.ToString());
            }
        }

        public static int GetDeterministicHashCode(this int? value)
        {
            unchecked
            {
                return GetDeterministicHashCode(value.ToString());
            }
        }

        public static int GetDeterministicHashCode(this long? value)
        {
            unchecked
            {
                return GetDeterministicHashCode(value.ToString());
            }
        }
    }
}
