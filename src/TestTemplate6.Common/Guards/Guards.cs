using System;
using System.Globalization;
using System.Text.RegularExpressions;
using TestTemplate6.Common.Exceptions;

namespace TestTemplate6.Common.Guards
{
    public class Guards
    {
        public static void NonNull<T>(T target, Guid id)
            where T : class
        {
            if (target == null)
            {
                throw new EntityNotFoundException(typeof(T).Name, id);
            }
        }
        public static void NonEmpty(string target, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(target))
            {
                throw new BusinessException($"{parameterName} cannot be empty.");
            }
        }
        public static void NonDefault<T>(T target, string parameterName)
            where T : class
        {
            if (target == default(T))
            {
                throw new BusinessException($"{parameterName} cannot be default value.");
            }
        }
        public static void NonDefault(Guid target, string parameterName)
        {
            if (target == default)
            {
                throw new BusinessException($"{parameterName} cannot be default value.");
            }
        }
        public static void NonDefault(DateTime target, string parameterName)
        {
            if (target == default)
            {
                throw new BusinessException($"{parameterName} cannot be default value.");
            }
        }
        public static void ValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new BusinessException($"{email} cannot be default value.");
            }

            try
            {
                // Normalize the domain
                email = Regex.Replace(
                    email,
                    @"(@)(.+)$",
                    DomainMapper,
                    options: RegexOptions.None,
                    TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                static string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                throw new BusinessException($"{email} not in valid format.");
            }
            catch (ArgumentException)
            {
                throw new BusinessException($"{email} not in valid format.");
            }

            try
            {
                if (!Regex.IsMatch(
                    email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase,
                    matchTimeout: TimeSpan.FromMilliseconds(250)))
                {
                    throw new BusinessException($"{email} not in valid format.");
                }
            }
            catch (RegexMatchTimeoutException)
            {
                throw new BusinessException($"{email} not in valid format.");
            }
        }
    }
}
