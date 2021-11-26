using CargaPacientesV1.CargaPacientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaPacientesV1
{
    class Program
    {
        static void Main(string[] args)
        {
            CargaPacientesOperaciones cargaPacientesOperaciones = new CargaPacientesOperaciones();
            cargaPacientesOperaciones.CargaPacientesOperacionesCargaTxt();
            Console.ReadKey(); 
        }
    }
}
