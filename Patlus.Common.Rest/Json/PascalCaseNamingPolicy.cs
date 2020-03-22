using System;
using System.Text.Json;

namespace Patlus.Common.Rest.Filters.Actions
{
    /// <summary>
    /// Addapted from <see href="https://github.com/dotnet/corefx/blob/master/src/System.Text.Json/src/System/Text/Json/Serialization/JsonCamelCaseNamingPolicy.cs">JsonCamelCaseNamingPolic</see>
    /// </summary>
    public sealed class PascalCaseNamingPolicy : JsonNamingPolicy
    {
        public static readonly PascalCaseNamingPolicy Default = new PascalCaseNamingPolicy();

        public override string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name) || !char.IsLower(name[0]))
            {
                return name;
            }

#if BUILDING_INBOX_LIBRARY
            return string.Create(name.Length, name, (chars, name) =>
            {
                name.AsSpan().CopyTo(chars);
                FixCasing(chars);
            });
#else
            char[] chars = name.ToCharArray();
            FixCasing(chars);
            return new string(chars);
#endif
        }

        private static void FixCasing(Span<char> chars)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !char.IsLower(chars[i]))
                {
                    break;
                }

                bool hasNext = (i + 1 < chars.Length);

                // Stop when next char is already lowercase.
                if (i > 0 && hasNext && !char.IsLower(chars[i + 1]))
                {
                    // If the next char is a space, lowercase current char before exiting.
                    if (chars[i + 1] == ' ')
                    {
                        chars[i] = char.ToUpperInvariant(chars[i]);
                    }

                    break;
                }

                chars[i] = char.ToUpperInvariant(chars[i]);
            }
        }
    }
}
