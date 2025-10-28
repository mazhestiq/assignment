using System.ComponentModel.DataAnnotations;

namespace InterviewApi.Models.Customers;

public class Customer : IValidatableObject
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime RegistrationDate { get; set; }
    public int TotalPurchases { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Name))
            yield return new ValidationResult($"Name can't be empty");

        if (string.IsNullOrWhiteSpace(Email))
            yield return new ValidationResult($"Email can't be empty");
    }
}