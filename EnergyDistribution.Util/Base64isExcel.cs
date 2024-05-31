using OfficeOpenXml;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EnergyDistribution.Util
{

    public class Base64isExcel : ValidationAttribute
    {

        public Base64isExcel() : base("El campo {0} en invalido")
        {

        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            try
            {
                if (value != null && value is string stringValue && !string.IsNullOrEmpty(stringValue))
                {
                    byte[] bytes = Convert.FromBase64String(value.ToString());
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        byte[] header = new byte[8];
                        ms.Read(header, 0, 8);
                        string headerStr = Encoding.ASCII.GetString(header);
                        if (!(headerStr.StartsWith("PK") || headerStr.StartsWith("UEsFBg")))
                        {
                            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

                        }
                    }
                }
                return ValidationResult.Success;
            }
            catch
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
        }
        public static ExcelPackage ConvertirBase64aExcel(string? b64)
        {
            byte[] bytes = Convert.FromBase64String(b64!);
            MemoryStream ms = new MemoryStream(bytes);
            ExcelPackage package = new ExcelPackage(ms);
            return package;
        }
    }

}
