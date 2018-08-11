using ah_mobile_app.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ah_mobile_app.Validators
{
    class EditarMascotaValidator
    {
        public bool Validate(EditarMascotaModelView registro)
        {
            String errorMessage = "Se presentaron los siguientes errores en el formulario de registro: \n";
            bool returnValue = true;

            if (registro.Nombre == null || registro.Edad == null || registro.Peso == null
                || registro.Raza == null)
            {
                errorMessage += "* Asegurese de llenar todos los campos.\n";
                returnValue = false;
                registro.DisplayError(errorMessage);
                return returnValue;
            }

            if (!Regex.IsMatch(registro.Nombre, "^[a-zA-Z]+$"))
            {
                errorMessage += "* Nombre contiene caracteres no válidos.\n";
                returnValue = false;
            }

            if (!Regex.IsMatch(registro.Raza, "^[a-zA-Z]+$"))
            {
                errorMessage += "* El campo Raza contiene caracteres no válidos.\n";
                returnValue = false;
            }

            if (!Regex.IsMatch(registro.Edad, "^[0-9]+$"))
            {
                errorMessage += "* Formato de Edad bo válido.\n";
                returnValue = false;
            }

            if (!Regex.IsMatch(registro.Peso, "^[0-9]+$"))
            {
                errorMessage += "* Formato de Peso no válido.\n";
                returnValue = false;
            }

            if (!returnValue)
                registro.DisplayError(errorMessage);

            return returnValue;
        }
    }
}
