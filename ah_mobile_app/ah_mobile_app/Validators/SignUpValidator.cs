using ah_mobile_app.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ah_mobile_app.Validators
{
    class SignUpValidator
    {
        public bool Validate(RegistrarUserViewModel registro)
        {
            String errorMessage = "Se presentaron los siguientes errores en el formulario de registro: \n";
            bool returnValue = true;

            if(registro.Nombre == null || registro.Direccion == null || registro.Cedula == null
                || registro.Email == null || registro.Password == null || registro.Password2 == null
                || registro.Telefono == null)
            {
                errorMessage += "* Asegurese de llenar todos los campos.\n";
                returnValue = false;
                registro.DisplayError(errorMessage);
                return returnValue;
            }

            if(!Regex.IsMatch(registro.Nombre, "^[a-zA-Z]+$"))
            {
                errorMessage += "* Nombre contiene caracteres no válidos.\n";
                returnValue = false;
            }

            if(registro.Cedula.Length < 10 || !Regex.IsMatch(registro.Cedula, "^[0-9]+$"))
            {
                errorMessage += "* Número de cédula no válido.\n";
                returnValue = false;
            }                 

            if (!Regex.IsMatch(registro.Email, "^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$"))
            {
                errorMessage += "* Email no tiene un formato válido.\n";
                returnValue = false;
            }

            if (registro.Telefono.Length < 9 || !Regex.IsMatch(registro.Telefono, "^[0-9]+$"))
            {
                errorMessage += "* Teléfono no tiene un formato válido.\n";
                returnValue = false;
            }

            if (registro.Password != registro.Password2 || registro.Password2.Length < 8)
            {
                errorMessage += "* Las contraseñas deben coincidir y tener más de 8 caracteres de largo.\n";
                returnValue = false;
            }
            if (!returnValue)
                registro.DisplayError(errorMessage);

            return returnValue;
        }
    }
}
