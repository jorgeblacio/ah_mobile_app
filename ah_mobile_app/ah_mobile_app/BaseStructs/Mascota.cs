using System;
using System.Collections.Generic;
using System.Text;

namespace ah_mobile_app.BaseStructs
{
    public class Mascota
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string raza { get; set; }
        public float peso { get; set; }
        public int edad { get; set; }
        public string propietario { get; set; }
        public List<Cita> citas { get; set; }



        public override string ToString()
        {
            return "Raza: " + raza + "\n Peso(Lbs): " + peso + "\n Edad: " + edad + " años.";
        }
    }
}
