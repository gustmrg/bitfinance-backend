using Microsoft.Extensions.Options;

namespace BitFinance.API.Settings;

public class StorageSettings
{
    public const string SectionName = "Storage";

    public string Provider { get; set; } = "Local";
    public string LocalPath { get; set; } = "./documents";
    public int MaxFileSizeInMB { get; set; } = 10;
    public string[] AllowedExtensions { get; set; } = [".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx"];
    public S3Settings S3 { get; set; } = new();
}

public class S3Settings
{
    public string BucketName { get; set; } = string.Empty;
    public string Region { get; set; } = "us-east-1";
    public string Prefix { get; set; } = "documents";
}

public class StorageSettingsValidator : IValidateOptions<StorageSettings>
{
    private static readonly string[] ValidProviders = ["Local", "S3"];

    public ValidateOptionsResult Validate(string? name, StorageSettings options)
    {
        var errors = new List<string>();

        if (!ValidProviders.Contains(options.Provider))
        {
            errors.Add($"Storage:Provider '{options.Provider}' is invalid. Valid values: {string.Join(", ", ValidProviders)}.");
        }

        if (options.MaxFileSizeInMB <= 0)
        {
            errors.Add("Storage:MaxFileSizeInMB must be greater than 0.");
        }

        if (options.AllowedExtensions.Length == 0)
        {
            errors.Add("Storage:AllowedExtensions must contain at least one file extension.");
        }

        switch (options.Provider)
        {
            case "Local":
                if (string.IsNullOrWhiteSpace(options.LocalPath))
                    errors.Add("Storage:LocalPath is required when Provider is 'Local'.");
                break;

            case "S3":
                if (string.IsNullOrWhiteSpace(options.S3.BucketName))
                    errors.Add("Storage:S3:BucketName is required when Provider is 'S3'.");
                if (string.IsNullOrWhiteSpace(options.S3.Region))
                    errors.Add("Storage:S3:Region is required when Provider is 'S3'.");
                break;
        }

        return errors.Count > 0
            ? ValidateOptionsResult.Fail(errors)
            : ValidateOptionsResult.Success;
    }
}