namespace Prevea.Model.Helpers
{
    #region Using

    using System;
    using System.Web;
    using System.IO;

    #endregion

    public static class HelperClass
    {
        public static bool HasFlag(Enum variable, Enum value)
        {
            if (variable == null)
                return false;

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            // Not as good as the .NET 4 version of this function, but should be good enough
            if (!Enum.IsDefined(variable.GetType(), value))
            {
                throw new ArgumentException($"Enumeration type mismatch.  The flag is of type '{value.GetType()}', was expecting '{variable.GetType()}'.");
            }

            var num = Convert.ToUInt64(value);
            return ((Convert.ToUInt64(variable) & num) == num);
        }

        public static string GetExension(string urlRelative)
        {
            var url = HttpContext.Current.Server.MapPath(urlRelative);
            var extension = Path.GetExtension(urlRelative);

            if (!File.Exists(url))
                extension = string.Empty;

            switch (extension)
            {
                case ".doc":
                case ".docx":
                    return "doc_opt.png";
                case ".xls":
                case ".xlsx":
                    return "xls_opt.png";
                case ".pdf":
                    return "pdf_opt.png";
                default:
                    return "unknown_opt.png";
            }
        }

        public static string GetDescription(string name)
        {
            switch (name)
            {
                case "FromSimulator":
                    return "Desde el Simulador";
                case "FromFormation":
                    return "Desde Formación";
                case "FromSede":
                    return "Desde SEDE";
                case "FromUser":
                    return "Desde Usuario";
                case "Issued":
                    return "Emitida";
                case "Assigned":
                    return "Asignada";
                case "ReAssigned":
                    return "Reasignada";
                case "Validated":
                    return "Validada";
                case "ValidationPending":
                    return "Pendiente de Validación";
                case "Modificated":
                    return "Modificada";
                default:
                    return name;
            }
        }
    }
}
