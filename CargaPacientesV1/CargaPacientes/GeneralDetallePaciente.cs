using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CargaPacientesV1.CargaPacientes
{
    public class GeneralDetallePaciente
    {
        /// <summary>
        /// quitar espacios
        /// remplazar  simbolos . / por  _
        /// </summary>
        public string Fecha { get; set; }
        public string Cod_Cargo { get; set; } //Cod.Cargo
        public string Descripcion { get; set; }
        public string Cant_ { get; set; } //Cant.
        public string PrecioUni_ { get; set; } // PrecioUni.
        public string IVAUnitario { get; set; }
        public string PrecioInc_IVA { get; set; } //PrecioInc/IVA 
        public string AFacturar { get; set; }
        public string Origen { get; set; }
        public string Usuario { get; set; }
        public string Cpto { get; set; }
    }
}

