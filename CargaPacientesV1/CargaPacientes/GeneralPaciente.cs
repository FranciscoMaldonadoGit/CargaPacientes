using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CargaPacientesV1.CargaPacientes
{
    public class GeneralPaciente
    {
        public string CuentaPaciente { get; set; }
        public string Historia { get; set; }
        public string Nombre { get; set; }
        public string FechadeIngreso { get; set; }
        public string CuentaActiva { get; set; }
        public string TipoPaciente { get; set; }
        public string Habitacion { get; set; }
        public string Cama { get; set; }
        public string Medico { get; set; }
        public string Observaciones { get; set; }
        public string SUBTOTAL { get; set; }
        public string IVA { get; set; }
        public string TOTAL { get; set; }
        public string MONTODEPAGOSENCAJA { get; set; }
        public string DESCUENTOS { get; set; }
        public string SALDODECUENTAPACIENTE { get; set; }
        public string TOTALDEPAGOS { get; set; }
        public string Continuaenpágina { get; set; }
    }
}
