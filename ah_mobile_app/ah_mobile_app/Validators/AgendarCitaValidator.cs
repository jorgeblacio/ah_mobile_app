using ah_mobile_app.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ah_mobile_app.Validators
{
    class AgendarCitaValidator
    {
        public bool Validate(ReservaCitaViewModel registro)
        {
            String errorMessage = "Se presentaron los siguientes errores en el formulario de registro: \n";
            bool returnValue = true;

            if (registro.Available_Time_Index == -1 || registro.Mascota_ID_Index == -1 || registro.Fecha == null)
            {
                errorMessage += "* Asegurese de llenar todos los campos.\n";
                returnValue = false;
                registro.DisplayError(errorMessage);
                return returnValue;
            }

            if(!returnValue)
                registro.DisplayError(errorMessage);

            return returnValue;
        }
    }
}
