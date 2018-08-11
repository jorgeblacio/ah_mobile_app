using System;
using System.Collections.Generic;
using System.Text;

namespace ah_mobile_app.BaseStructs
{
    public class CitaMascota:IComparable
    {
        public int cita_id { get; set; }
        public DateTime cita_fecha { get; set; }
        public DateTime cita_duracion { get; set; }
        public int mascota_id { get; set; }
        public string mascota_nombre { get; set; }

        public CitaMascota(int cita_id, DateTime cita_fecha, 
            DateTime cita_duracion, int mascota_id, string mascota_nombre)
        {
            this.cita_id = cita_id;
            this.cita_fecha = cita_fecha;
            this.cita_duracion = cita_duracion;
            this.mascota_id = mascota_id;
            this.mascota_nombre = mascota_nombre;
        }

        public override string ToString()
        {
            return "Mascota: " + mascota_nombre + "\nCita Fecha: " + cita_fecha.ToShortDateString() + "\n Hora: " + cita_fecha.ToShortTimeString();
        }

        public int CompareTo(object obj)
        {
            CitaMascota citaMascota = obj as CitaMascota;
            if (citaMascota == null)
            {
                throw new ArgumentException("Object is not Cita");
            }
            return this.cita_fecha.CompareTo(citaMascota.cita_fecha);
        }
    }
}
