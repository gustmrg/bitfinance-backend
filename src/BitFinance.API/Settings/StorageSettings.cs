using Microsoft.Extensions.Options;

namespace BitFinance.API.Settings;

public class StorageSettings
{
    public const string SectionName = "Storage";

    public string BucketName { get; set; } = string.Empty;
    public string Region { get; set; } = "us-east-1";
    public string Prefix { get; set; } = "documents";
    public string ServiceUrl { get; set; } = string.Empty;
    public int MaxFileSizeInMB { get; set; } = 10;
    public string[] AllowedExtensions { get; set; } = [".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx"];
}

public class StorageSettingsValidator : IValidateOptions<StorageSettings>
{
    public ValidateOptionsResult Validate(string? name, StorageSettings options)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(options.BucketName))
            errors.Add("Storage:BucketName is required.");

        if (string.IsNullOrWhiteSpace(options.Region))
            errors.Add("Storage:Region is required.");

        if (options.MaxFileSizeInMB <= 0)
            errors.Add("Storage:MaxFileSizeInMB must be greater than 0.");

        if (options.AllowedExtensions.Length == 0)
            errors.Add("Storage:AllowedExtensions must contain at least one file extension.");

        return errors.Count > 0
            ? ValidateOptionsResult.Fail(errors)
            : ValidateOptionsResult.Success;
    }
}