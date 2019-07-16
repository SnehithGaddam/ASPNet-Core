// <auto-generated />
namespace Microsoft.AspNetCore.Session
{
    using System.Globalization;
    using System.Reflection;
    using System.Resources;

    internal static class Resources
    {
        private static readonly ResourceManager _resourceManager
            = new ResourceManager("Microsoft.AspNetCore.Session.Resources", typeof(Resources).GetTypeInfo().Assembly);

        /// <summary>
        /// The key cannot be longer than '{0}' when encoded with UTF-8.
        /// </summary>
        internal static string Exception_KeyLengthIsExceeded
        {
            get => GetString("Exception_KeyLengthIsExceeded");
        }

        /// <summary>
        /// The key cannot be longer than '{0}' when encoded with UTF-8.
        /// </summary>
        internal static string FormatException_KeyLengthIsExceeded(object p0)
            => string.Format(CultureInfo.CurrentCulture, GetString("Exception_KeyLengthIsExceeded"), p0);

        /// <summary>
        /// The session cannot be established after the response has started.
        /// </summary>
        internal static string Exception_InvalidSessionEstablishment
        {
            get => GetString("Exception_InvalidSessionEstablishment");
        }

        /// <summary>
        /// The session cannot be established after the response has started.
        /// </summary>
        internal static string FormatException_InvalidSessionEstablishment()
            => GetString("Exception_InvalidSessionEstablishment");

        /// <summary>
        /// The value cannot be serialized in two bytes.
        /// </summary>
        internal static string Exception_InvalidToSerializeIn2Bytes
        {
            get => GetString("Exception_InvalidToSerializeIn2Bytes");
        }

        /// <summary>
        /// The value cannot be serialized in two bytes.
        /// </summary>
        internal static string FormatException_InvalidToSerializeIn2Bytes()
            => GetString("Exception_InvalidToSerializeIn2Bytes");

        /// <summary>
        /// The value cannot be serialized in three bytes.
        /// </summary>
        internal static string Exception_InvalidToSerializeIn3Bytes
        {
            get => GetString("Exception_InvalidToSerializeIn3Bytes");
        }

        /// <summary>
        /// The value cannot be serialized in three bytes.
        /// </summary>
        internal static string FormatException_InvalidToSerializeIn3Bytes()
            => GetString("Exception_InvalidToSerializeIn3Bytes");

        /// <summary>
        /// The value cannot be negative.
        /// </summary>
        internal static string Exception_NumberShouldNotBeNegative
        {
            get => GetString("Exception_NumberShouldNotBeNegative");
        }

        /// <summary>
        /// The value cannot be negative.
        /// </summary>
        internal static string FormatException_NumberShouldNotBeNegative()
            => GetString("Exception_NumberShouldNotBeNegative");

        /// <summary>
        /// Argument cannot be null or empty string.
        /// </summary>
        internal static string ArgumentCannotBeNullOrEmpty
        {
            get => GetString("ArgumentCannotBeNullOrEmpty");
        }

        /// <summary>
        /// Argument cannot be null or empty string.
        /// </summary>
        internal static string FormatArgumentCannotBeNullOrEmpty()
            => GetString("ArgumentCannotBeNullOrEmpty");

        private static string GetString(string name, params string[] formatterNames)
        {
            var value = _resourceManager.GetString(name);

            System.Diagnostics.Debug.Assert(value != null);

            if (formatterNames != null)
            {
                for (var i = 0; i < formatterNames.Length; i++)
                {
                    value = value.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
                }
            }

            return value;
        }
    }
}
