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
    public class CargaPacientesServicios
    {
        public readonly string rutaArchivo = "";
        string[] valoresEncabezado;
        string[] valoresRegistro;
        string jsonServicios = "";
        string objPrincipaFinal = ""; 

        public CargaPacientesServicios(string rutaArchivo)
        {
            this.rutaArchivo = rutaArchivo;
        }

        public void GuardarValoresArreglos(int index, string line, bool esEncabezado = false)
        {
            string valorFila = ConstruyeObjetosDeFilaTablaPropiedad2(line);
            if (esEncabezado)
                valoresEncabezado[index] = valorFila; //line.Trim();
            else
                valoresRegistro[index] = valorFila;   //line.Trim(); 
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
        /// Recorre registros  y encabezado
        /// y cada columna del registr
        /// </summary>
        /// <param name="valores"></param>
        /// <param name="linea"></param>
        /// <param name="esEncabezado"></param>
        public void ConstruyeObjetosDeFilaTabla(string[] valores, string linea, int lineasGuiatamanio = 0, bool esEncabezado = false)
        {
            
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
                    GuardarValoresArreglos(index, linea.Substring(pos, valores[index].Length), esEncabezado);
                }
                else
                {
                    if (index == 1)
                    {
                        pos += valores[index - 1].Length;
                        pos += 2;
                        GuardarValoresArreglos(index, linea.Substring(pos - 1, valores[index].Length), esEncabezado);
                    }
                    else
                    {
                        pos += valores[index - 1].Length;
                        pos += 1;
                        GuardarValoresArreglos(index, linea.Substring(pos - 1, valores[index].Length), esEncabezado);
                    }
                }
                index++;
            }
        }

        public void ConstruyeObjetosDeFilaTablaJsonServicios()
        {
            int indexConstruyeObjetosDeFilaTablaJson = 0;
            jsonServicios += jsonServicios != "" ? ",{" : "[{";

            foreach (var item in valoresEncabezado)
            {
                var prop = ConstruyeObjetosDeFilaTablaPropiedad2(item, true);
                jsonServicios += $" \"{prop}\" : \"{valoresRegistro[indexConstruyeObjetosDeFilaTablaJson]}\" ";

                if (indexConstruyeObjetosDeFilaTablaJson != valoresRegistro.Length - 1)
                    jsonServicios += " ,";

                indexConstruyeObjetosDeFilaTablaJson++;
            }

            jsonServicios += "}";
        }

        //public string ConstruyeObjetosDeFilaTablaPropiedad(string item)
        //{
        //    return item.Replace(" ", "").Replace(".", "_").Replace("/", "_");
        //}

        public string ConstruyeObjetosDeFilaTablaPropiedad2(string item,  bool esPropiedad = false)
        {
            if (esPropiedad)
                return item.Replace(" ", "").Replace(".", "_").Replace("/", "_").Trim();
            else //es valor
                return item.Trim(); 
        }

        public string[] PatronExpresionRegules()
        {
            string SimboloFlechaClave = @"[a-zA-ZÀ-ÿ]+\s[a-zA-ZÀ-ÿ]+\s[a-zA-ZÀ-ÿ]+\s+[==>]{3}";
            string UnaPalabra = @"[a-zA-ZÀ-ÿ]+[:]{1}";
            string DosPalabra = @"[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+[:]{1}";
            string TresPalabra = @"[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+[:]{1}"; ;
            string CuatroPalabra = @"[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+[:]{1}";
            string CincoPalabra = @"[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+[:]{1}";
            string pattern = @"(" + SimboloFlechaClave + "|" + UnaPalabra + "|" + DosPalabra + "|" + TresPalabra + "|" + CuatroPalabra + "|" + CincoPalabra + ")";

            string SimboloFlechaValue = @"[==>]{3}\s+[0-9.,]+";
            string UnaPalabraVal = @"[:]{1}\s{2,25}[a-zA-Z0-9/.,]+";
            string DosPalabraVal = @"[:]{1}\s{2,15}[a-zA-Z0-9/,]+\s{1}[a-zA-Z0-9/,]+";
            string TresPalabraVal = @"[:]{1}\s{2,15}[a-zA-Z0-9/.,]+\s{1}[a-zA-Z0-9/.,]+\s{1}[a-zA-Z0-9/.,]+"; ;
            string CuatroPalabraVal = @"[:]{1}\s{2,15}[a-zA-Z0-9/.,]+\s{1}[a-zA-Z0-9/.,]+\s{1}[a-zA-Z0-9/.,]+\s{1}[a-zA-Z0-9/.,]+";
            string CincoPalabraVal = @"[:]{1}\s{2,15}[a-zA-Z0-9/.,]+\s{1}[a-zA-Z0-9/.,]+\s{1}[a-zA-Z0-9/.,]+\s{1}[a-zA-Z0-9/.,]+\s{1}[a-zA-Z0-9/.,]+";
            string pattern2 = @"(" + SimboloFlechaValue + "|" + CincoPalabraVal + "|" + CuatroPalabraVal + "|" + TresPalabraVal + "|" + DosPalabraVal + "|" + UnaPalabraVal + ")";

            return new string[] { pattern, pattern2 };
        }

        public GeneralPaciente CargaDatosGeneralesPacienteTxt()
        {
            string text = System.IO.File.ReadAllText(this.rutaArchivo);
            Regex RegExClaves = new Regex(PatronExpresionRegules()[0]);
            var matchCollection = RegExClaves.Matches(text);

            Regex RegExValues = new Regex(PatronExpresionRegules()[1]);
            var matchCollection2 = RegExValues.Matches(text);

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

                arr1[index] = ConstruyeObjetosDeFilaTablaPropiedad2(remplzar);//remplzar.Trim();

                index++;
            }

            index = 0;
            string objPrincipa = "";
            foreach (Match match in matchCollection)
            {
                //tomar en cuenta a partir de cuenta paciente index >= 3 
                //ignorar  DETALLE DE LOS PAGOS:
                if (index >= 3 && index != 20){
                    // del final remover ==>
                    if (match.Value.Substring(match.Value.Length - 3, 3) == "==>")
                    {
                        //objPrincipa += $"\"{match.Value.Remove(match.Value.Length - 3, 3).Trim().Replace(" ", "")}\":\"{arr1[index]}\",\n";
                        objPrincipa += $"\"{ConstruyeObjetosDeFilaTablaPropiedad2(match.Value.Remove(match.Value.Length - 3, 3), true )}\":\"{arr1[index]}\"";
                    }
                    //del final remover :
                    else
                    {
                        //objPrincipa += $"\"{match.Value.Remove(match.Value.Length - 1, 1).Trim().Replace(" ", "")}\":\"{arr1[index]}\",\n";
                        objPrincipa += $"\"{ConstruyeObjetosDeFilaTablaPropiedad2(match.Value.Remove(match.Value.Length - 1, 1), true)}\":\"{arr1[index]}\"";
                    }

                    if (index != arr1.Length - 1)//AUN NO llego al final uso comas
                    objPrincipa += ",\n";
                }
                index++;
            }

            objPrincipaFinal = "{" + objPrincipa + "}";
            Console.WriteLine(objPrincipaFinal);

            return JsonConvert.DeserializeObject<GeneralPaciente>(objPrincipaFinal);
        }

        public List<GeneralDetallePaciente> CargaDatosServiciosPacienteTxt()
        {
            int index3 = 0;
            bool indexColumnas = false;
            bool enconcuentraTamColumnas = false;
            string[] numerocaracteres = null;
            string lineaEncabezados = null;
            int lineasGuiatamanio = 0;

            var readlinesTxt = File.ReadLines(this.rutaArchivo);

            foreach (string line in readlinesTxt)
            {
                //Las tablas a verificar deben tener por lo menos estos campos Cod.Cargo y Fecha
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

                //recorrer solo registros
                if (indexColumnas == true)
                {
                    ConstruyeObjetosDeFilaTabla(numerocaracteres, lineaEncabezados, lineasGuiatamanio, true);
                    ConstruyeObjetosDeFilaTabla(numerocaracteres, line);
                    ConstruyeObjetosDeFilaTablaJsonServicios();
                    index3++;
                }
            }

            jsonServicios += "]";

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine(jsonServicios);
            //ConversionesJsonObjetosCSharp(objPrincipaFinal, jsonServicios);
            return JsonConvert.DeserializeObject<List<GeneralDetallePaciente>>(jsonServicios);
        }

        //private void ConversionesJsonObjetosCSharp(string json, string jsonServicios)
        //{
        //    //GeneralPaciente settingCorreo = JsonConvert.DeserializeObject<GeneralPaciente>(json);
        //    List<GeneralDetallePaciente> settingCorreo2 = JsonConvert.DeserializeObject<List<GeneralDetallePaciente>>(jsonServicios);
        //}

    }
}
