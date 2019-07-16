// <auto-generated />
namespace Microsoft.Extensions.SecretManager.Tools
{
    using System.Globalization;
    using System.Reflection;
    using System.Resources;

    internal static class Resources
    {
        private static readonly ResourceManager _resourceManager
            = new ResourceManager("Microsoft.Extensions.SecretManager.Tools.Resources", typeof(Resources).GetTypeInfo().Assembly);

        /// <summary>
        /// Command failed : {message}
        /// </summary>
        internal static string Error_Command_Failed
        {
            get => GetString("Error_Command_Failed");
        }

        /// <summary>
        /// Command failed : {message}
        /// </summary>
        internal static string FormatError_Command_Failed(object message)
            => string.Format(CultureInfo.CurrentCulture, GetString("Error_Command_Failed", "message"), message);

        /// <summary>
        /// Missing parameter value for '{name}'.
        /// Use the '--help' flag to see info.
        /// </summary>
        internal static string Error_MissingArgument
        {
            get => GetString("Error_MissingArgument");
        }

        /// <summary>
        /// Missing parameter value for '{name}'.
        /// Use the '--help' flag to see info.
        /// </summary>
        internal static string FormatError_MissingArgument(object name)
            => string.Format(CultureInfo.CurrentCulture, GetString("Error_MissingArgument", "name"), name);

        /// <summary>
        /// Cannot find '{key}' in the secret store.
        /// </summary>
        internal static string Error_Missing_Secret
        {
            get => GetString("Error_Missing_Secret");
        }

        /// <summary>
        /// Cannot find '{key}' in the secret store.
        /// </summary>
        internal static string FormatError_Missing_Secret(object key)
            => string.Format(CultureInfo.CurrentCulture, GetString("Error_Missing_Secret", "key"), key);

        /// <summary>
        /// Multiple MSBuild project files found in '{projectPath}'. Specify which to use with the --project option.
        /// </summary>
        internal static string Error_MultipleProjectsFound
        {
            get => GetString("Error_MultipleProjectsFound");
        }

        /// <summary>
        /// Multiple MSBuild project files found in '{projectPath}'. Specify which to use with the --project option.
        /// </summary>
        internal static string FormatError_MultipleProjectsFound(object projectPath)
            => string.Format(CultureInfo.CurrentCulture, GetString("Error_MultipleProjectsFound", "projectPath"), projectPath);

        /// <summary>
        /// No secrets configured for this application.
        /// </summary>
        internal static string Error_No_Secrets_Found
        {
            get => GetString("Error_No_Secrets_Found");
        }

        /// <summary>
        /// No secrets configured for this application.
        /// </summary>
        internal static string FormatError_No_Secrets_Found()
            => GetString("Error_No_Secrets_Found");

        /// <summary>
        /// Could not find a MSBuild project file in '{projectPath}'. Specify which project to use with the --project option.
        /// </summary>
        internal static string Error_NoProjectsFound
        {
            get => GetString("Error_NoProjectsFound");
        }

        /// <summary>
        /// Could not find a MSBuild project file in '{projectPath}'. Specify which project to use with the --project option.
        /// </summary>
        internal static string FormatError_NoProjectsFound(object projectPath)
            => string.Format(CultureInfo.CurrentCulture, GetString("Error_NoProjectsFound", "projectPath"), projectPath);

        /// <summary>
        /// Could not find the global property 'UserSecretsId' in MSBuild project '{project}'. Ensure this property is set in the project or use the '--id' command line option.
        /// </summary>
        internal static string Error_ProjectMissingId
        {
            get => GetString("Error_ProjectMissingId");
        }

        /// <summary>
        /// Could not find the global property 'UserSecretsId' in MSBuild project '{project}'. Ensure this property is set in the project or use the '--id' command line option.
        /// </summary>
        internal static string FormatError_ProjectMissingId(object project)
            => string.Format(CultureInfo.CurrentCulture, GetString("Error_ProjectMissingId", "project"), project);

        /// <summary>
        /// The project file '{path}' does not exist.
        /// </summary>
        internal static string Error_ProjectPath_NotFound
        {
            get => GetString("Error_ProjectPath_NotFound");
        }

        /// <summary>
        /// The project file '{path}' does not exist.
        /// </summary>
        internal static string FormatError_ProjectPath_NotFound(object path)
            => string.Format(CultureInfo.CurrentCulture, GetString("Error_ProjectPath_NotFound", "path"), path);

        /// <summary>
        /// Could not load the MSBuild project '{project}'.
        /// </summary>
        internal static string Error_ProjectFailedToLoad
        {
            get => GetString("Error_ProjectFailedToLoad");
        }

        /// <summary>
        /// Could not load the MSBuild project '{project}'.
        /// </summary>
        internal static string FormatError_ProjectFailedToLoad(object project)
            => string.Format(CultureInfo.CurrentCulture, GetString("Error_ProjectFailedToLoad", "project"), project);

        /// <summary>
        /// Project file path {project}.
        /// </summary>
        internal static string Message_Project_File_Path
        {
            get => GetString("Message_Project_File_Path");
        }

        /// <summary>
        /// Project file path {project}.
        /// </summary>
        internal static string FormatMessage_Project_File_Path(object project)
            => string.Format(CultureInfo.CurrentCulture, GetString("Message_Project_File_Path", "project"), project);

        /// <summary>
        /// Successfully saved {key} = {value} to the secret store.
        /// </summary>
        internal static string Message_Saved_Secret
        {
            get => GetString("Message_Saved_Secret");
        }

        /// <summary>
        /// Successfully saved {key} = {value} to the secret store.
        /// </summary>
        internal static string FormatMessage_Saved_Secret(object key, object value)
            => string.Format(CultureInfo.CurrentCulture, GetString("Message_Saved_Secret", "key", "value"), key, value);

        /// <summary>
        /// Successfully saved {number} secrets to the secret store.
        /// </summary>
        internal static string Message_Saved_Secrets
        {
            get => GetString("Message_Saved_Secrets");
        }

        /// <summary>
        /// Successfully saved {number} secrets to the secret store.
        /// </summary>
        internal static string FormatMessage_Saved_Secrets(object number)
            => string.Format(CultureInfo.CurrentCulture, GetString("Message_Saved_Secrets", "number"), number);

        /// <summary>
        /// Secrets file path {secretsFilePath}.
        /// </summary>
        internal static string Message_Secret_File_Path
        {
            get => GetString("Message_Secret_File_Path");
        }

        /// <summary>
        /// Secrets file path {secretsFilePath}.
        /// </summary>
        internal static string FormatMessage_Secret_File_Path(object secretsFilePath)
            => string.Format(CultureInfo.CurrentCulture, GetString("Message_Secret_File_Path", "secretsFilePath"), secretsFilePath);

        /// <summary>
        /// {key} = {value}
        /// </summary>
        internal static string Message_Secret_Value_Format
        {
            get => GetString("Message_Secret_Value_Format");
        }

        /// <summary>
        /// {key} = {value}
        /// </summary>
        internal static string FormatMessage_Secret_Value_Format(object key, object value)
            => string.Format(CultureInfo.CurrentCulture, GetString("Message_Secret_Value_Format", "key", "value"), key, value);

        /// <summary>
        /// The UserSecretsId '{userSecretsId}' cannot contain any characters that cannot be used in a file path.
        /// </summary>
        internal static string Error_InvalidSecretsId
        {
            get => GetString("Error_InvalidSecretsId");
        }

        /// <summary>
        /// The UserSecretsId '{userSecretsId}' cannot contain any characters that cannot be used in a file path.
        /// </summary>
        internal static string FormatError_InvalidSecretsId(object userSecretsId)
            => string.Format(CultureInfo.CurrentCulture, GetString("Error_InvalidSecretsId", "userSecretsId"), userSecretsId);

        /// <summary>
        /// The MSBuild project '{project}' has already been initialized with a UserSecretsId.
        /// </summary>
        internal static string Message_ProjectAlreadyInitialized
        {
            get => GetString("Message_ProjectAlreadyInitialized");
        }

        /// <summary>
        /// The MSBuild project '{project}' has already been initialized with a UserSecretsId.
        /// </summary>
        internal static string FormatMessage_ProjectAlreadyInitialized(object project)
            => string.Format(CultureInfo.CurrentCulture, GetString("Message_ProjectAlreadyInitialized", "project"), project);

        /// <summary>
        /// Set UserSecretsId to '{userSecretsId}' for MSBuild project '{project}'.
        /// </summary>
        internal static string Message_SetUserSecretsIdForProject
        {
            get => GetString("Message_SetUserSecretsIdForProject");
        }

        /// <summary>
        /// Set UserSecretsId to '{userSecretsId}' for MSBuild project '{project}'.
        /// </summary>
        internal static string FormatMessage_SetUserSecretsIdForProject(object userSecretsId, object project)
            => string.Format(CultureInfo.CurrentCulture, GetString("Message_SetUserSecretsIdForProject", "userSecretsId", "project"), userSecretsId, project);

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
