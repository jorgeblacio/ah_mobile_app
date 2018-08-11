using System;
using System.Collections.Generic;
using System.Text;

namespace ah_mobile_app.BaseStructs
{
    public class Cita : IComparable
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public DateTime duracion { get; set; }
        public int mascota_id { get; set; }

        public override string ToString()
        {
            return "Cita Fecha: " + fecha.ToShortDateString() + "\n Hora: " + fecha.ToShortTimeString();
        }

        public int CompareTo(object obj)
        {
            Cita cita = obj as Cita;
            if (cita == null)
            {
                throw new ArgumentException("Object is not Cita");
            }
            return this.fecha.CompareTo(cita.fecha);
        }
    }    
}
