using EnergyDistribution.Util;
using System.ComponentModel.DataAnnotations;

namespace EnergyDistribution.Shared.ValidModel;

public class EsIP: ValidationAttribute
{

    public EsIP() : base("El campo {0} no corresponde a una dirección ip") { }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value is null)
        {
            return ValidationResult.Success;
        }
        if (value is string)
        {
            //if (ValidacionesString.StringEsIP(value.ToString()!))
            //{
            //    return ValidationResult.Success;
            //}
        }
        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
    }

}
