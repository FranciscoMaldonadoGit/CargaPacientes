using CargaPacientesV1.CargaPacientes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaPacientesV1
{
    class Program
    {
        static void Main(string[] args)
        {
            //string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //var directory = System.IO.Path.GetDirectoryName(path);

            var ruta = @"C:\Users\Luz Maldonado\Documents\Proyectos\CargaPacientes\ejemplo1.txt"; //ruta  de tu archivo
            CargaPacientesServicios cargaPacientesOperaciones = new CargaPacientesServicios(ruta);
            GeneralPaciente generalPaciente = cargaPacientesOperaciones.CargaDatosGeneralesPacienteTxt();
            List<GeneralDetallePaciente> listgeneralDetallePaciente = cargaPacientesOperaciones.CargaDatosServiciosPacienteTxt();
            Console.ReadKey(); 
        }
    }
}
