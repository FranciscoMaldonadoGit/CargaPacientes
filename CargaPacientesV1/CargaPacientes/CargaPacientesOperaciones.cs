using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CargaPacientesV1.CargaPacientes
{
    public class CargaPacientesOperaciones
    {
        string[] valoresEncabezado;
        string[] valoresRegistro;
        string jsonServicios = "";


        public void DefineValores(int index, string line, bool esEncabezado = false)
        {
            if (esEncabezado)
            {
                valoresEncabezado[index] = line.Trim();
            }
            else
            {
                valoresRegistro[index] = line.Trim();
            }
        }

        public string IgualarStrings(string linea, int lineasGuiatamanio)
        {
            string espacio = " ";
            int espacionFaltantesEncabezado = lineasGuiatamanio - linea.Length;

            if (linea.Length < lineasGuiatamanio)
            {
                for (int i = 0; i < espacionFaltantesEncabezado; i++)
                {
                    linea += espacio;
                }
            }

            return linea;
        }

        /// <summary>
        /// Recorre registros
        /// </summary>
        /// <param name="valores"></param>
        /// <param name="linea"></param>
        /// <param name="esEncabezado"></param>
        public void Construye(string[] valores, string linea, int lineasGuiatamanio = 0, bool esEncabezado = false)
        {
            //una columna no existe
            //pendiente mejora 
            if (esEncabezado)
            {
                linea = IgualarStrings(linea, lineasGuiatamanio);
                valoresEncabezado = new string[valores.Count()];
            }

            valoresRegistro = new string[valores.Count()];

            int index = 0;
            int pos = 0;
            foreach (string item in valores)
            {
                if (index == 0)
                {
                    DefineValores(index, linea.Substring(pos, valores[index].Length), esEncabezado);
                }
                else
                {
                    if (index == 1)
                    {
                        pos += valores[index - 1].Length;
                        pos += 2;
                        DefineValores(index, linea.Substring(pos - 1, valores[index].Length), esEncabezado);
                    }
                    else
                    {
                        pos += valores[index - 1].Length;
                        pos += 1;
                        DefineValores(index, linea.Substring(pos - 1, valores[index].Length), esEncabezado);
                    }
                }
                index++;
            }
        }

        public void ConstruyeJsonServicios()
        {
            int indexConstruyeJson = 0;
            jsonServicios += jsonServicios != "" ? ",{" : "[{";

            foreach (var item in valoresEncabezado)
            {
                var prop = ConstruyePropiedad(item); 
                jsonServicios += $" \"{prop}\" : \"{valoresRegistro[indexConstruyeJson]}\" ";

                if (indexConstruyeJson != valoresRegistro.Length - 1)
                    jsonServicios += " ,";
                indexConstruyeJson++;
            }

            jsonServicios += "}";
        }

        public string  ConstruyePropiedad(string item)
        {
            return item.Replace(" ", "").Replace(".", "_").Replace("/", "_");
        }

        public void CargaPacientesOperacionesCargaTxt()
        {
            string text = System.IO.File.ReadAllText(@"C:\Users\Luz Maldonado\Documents\Proyectos\CargaPacientes\ejemplo1.txt");

            string SimboloFlechaClave = @"[a-zA-ZÀ-ÿ]+\s[a-zA-ZÀ-ÿ]+\s[a-zA-ZÀ-ÿ]+\s+[==>]{3}";
            string UnaPalabra = @"[a-zA-ZÀ-ÿ]+[:]{1}";
            string DosPalabra = @"[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+[:]{1}";
            string TresPalabra = @"[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+[:]{1}"; ;
            string CuatroPalabra = @"[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+[:]{1}";
            string CincoPalabra = @"[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+[:]{1}";
            string pattern = @"(" + SimboloFlechaClave + "|" + UnaPalabra + "|" + DosPalabra + "|" + TresPalabra + "|" + CuatroPalabra + "|" + CincoPalabra + ")";

            Regex rgx = new Regex(pattern);
            var matchCollection = rgx.Matches(text);

            string SimboloFlechaValue = @"[==>]{3}\s+[0-9.,]+";
            string UnaPalabraVal = @"[:]{1}\s{2,25}[a-zA-Z0-9/.,]+";
            //string DosPalabraVal = @"[:]{1}\s{2,15}[a-zA-Z0-9/,]+\s{1}[a-zA-Z0-9/,]+";
            string TresPalabraVal = @"[:]{1}\s{2,15}[a-zA-Z0-9/.,]+\s{1}[a-zA-Z0-9/.,]+\s{1}[a-zA-Z0-9/.,]+"; ;
            string CuatroPalabraVal = @"[:]{1}\s{2,15}[a-zA-Z0-9/.,]+\s{1}[a-zA-Z0-9/.,]+\s{1}[a-zA-Z0-9/.,]+\s{1}[a-zA-Z0-9/.,]+";
            string CincoPalabraVal = @"[:]{1}\s{2,15}[a-zA-Z0-9/.,]+\s{1}[a-zA-Z0-9/.,]+\s{1}[a-zA-Z0-9/.,]+\s{1}[a-zA-Z0-9/.,]+\s{1}[a-zA-Z0-9/.,]+";
            //string pattern2 = @"(" + UnaPalabraVal + "|" + DosPalabraVal + "|" + TresPalabraVal + "|" + CuatroPalabraVal + "|" + CincoPalabraVal + ")";
            //string pattern2 = @"(" + UnaPalabraVal + "|"+ TresPalabraVal + "|" + CuatroPalabraVal + "|" + CincoPalabraVal + ")";
            string pattern2 = @"(" + SimboloFlechaValue + "|" + CincoPalabraVal + "|" + CuatroPalabraVal + "|" + TresPalabraVal + "|" + UnaPalabraVal + ")";

            Regex rgx2 = new Regex(pattern2);
            var matchCollection2 = rgx2.Matches(text);

            #region objeto datos generales
            string remplzar = "";
            int index = 0;
            string[] arr1 = new string[matchCollection2.Count];
            foreach (Match match2 in matchCollection2)
            {
                //del principio remover ==>
                if (match2.Value.Substring(0, 3) == "==>")
                {
                    remplzar = match2.Value.Remove(0, 3);
                }
                //del principio remover :
                else
                {
                    remplzar = match2.Value.Remove(0, 1);
                }

                arr1[index] = remplzar.Trim();

                index++;
            }

            index = 0;
            string objPrincipa = "";
            foreach (Match match in matchCollection)
            {
                //tomar en cuenta a partir de cuenta paciente
                if (index >= 3)
                {
                    // del final remover ==>
                    if (match.Value.Substring(match.Value.Length - 3, 3) == "==>")
                    {
                        objPrincipa += $"\"{match.Value.Remove(match.Value.Length - 3, 3).Trim().Replace(" ", "")}\":\"{arr1[index]}\",\n";
                    }
                    //del final remover :
                    else
                    {
                        objPrincipa += $"\"{match.Value.Remove(match.Value.Length - 1, 1).Trim().Replace(" ", "")}\":\"{arr1[index]}\",\n";
                    }
                }
                index++;
            }
            objPrincipa = objPrincipa.Remove(objPrincipa.Length - 2, 1);  //remover ulitma coma (,)
            string objPrincipaFinal = "{" + objPrincipa + "}";
            Console.WriteLine(objPrincipaFinal);
            #endregion

            #region datosGenerales
            int index3 = 0;
            bool indexColumnas = false;
            bool enconcuentraTamColumnas = false;
            string[] numerocaracteres = null;
            string lineaEncabezados = null;
            int lineasGuiatamanio = 0;

            foreach (string line in File.ReadLines(@"C:\Users\Luz Maldonado\Documents\Proyectos\CargaPacientes\ejemplo1.txt"))
            {
                if (line.Contains("Cod.Cargo        ") || line.Contains("Fecha    "))
                {
                    lineaEncabezados = line;
                    indexColumnas = true;
                    continue;  //evitar que inice aqui
                }

                //enconcuentraTamColumnas : variable para controlar que se haga una sola vez.
                //indexColumnas   : indice para encontrar la linea que señala el tamaño y cantidad de columnas de la tabla
                //Ejemplo : ---- ------- .
                if (indexColumnas == true && enconcuentraTamColumnas == false)
                {
                    var lineColumnas = line.Split(' ');
                    numerocaracteres = new string[lineColumnas.Count()];
                    numerocaracteres = lineColumnas;
                    lineasGuiatamanio = line.Length;
                    enconcuentraTamColumnas = true;
                    continue; //evitar que inicie aqui
                }

                //identificar final de la tabla
                if (indexColumnas == true && line == "")
                    break;

                if (indexColumnas == true)
                {
                    Construye(numerocaracteres, lineaEncabezados, lineasGuiatamanio, true);
                    Construye(numerocaracteres, line);
                    ConstruyeJsonServicios();
                    index3++;
                }
            }
            
            jsonServicios += "]";

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine(jsonServicios);

            ConversionesJsonObjetosCSharp(objPrincipaFinal, jsonServicios);
            
            #endregion
        }

        private void ConversionesJsonObjetosCSharp(string json, string jsonServicios)
        {
            GeneralPaciente settingCorreo = JsonConvert.DeserializeObject<GeneralPaciente>(json);
            List<GeneralDetallePaciente> settingCorreo2 = JsonConvert.DeserializeObject<List<GeneralDetallePaciente>>(jsonServicios);
        }

    }
}
