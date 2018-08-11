using System;
using System.Collections.Generic;
using System.Text;

namespace ah_mobile_app.BaseStructs
{
    public class Cliente
    {
        public string cedula { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public List<Mascota> mascotas { get; set; }

        public override string ToString()
        {
            return "Cliente: " + nombre + " email" + email;
        }
    }
}
