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

        /// <summary>
        /// Recorre registros
        /// </summary>
        /// <param name="valores"></param>
        /// <param name="linea"></param>
        /// <param name="esEncabezado"></param>
        public void Construye(string[] valores, string linea, bool esEncabezado = false)
        {
            //una columna no existe
            //pendiente mejora 
            if (esEncabezado)
            {
                linea += "          ";
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
                        var tamanio = linea.Length;
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
                jsonServicios += $" \"{item}\" : \"{valoresRegistro[indexConstruyeJson]}\" ";

                if(indexConstruyeJson != valoresRegistro.Length -1)
                jsonServicios += " ,"; 
                indexConstruyeJson++;
            }

            jsonServicios += "}";
        }

        public void CargaPacientesOperacionesCargaTxt()
        {
            string text = System.IO.File.ReadAllText(@"C:\Users\Luz Maldonado\Desktop\CargaPacientes\ejemplo1.txt");
            //string text = System.IO.File.ReadAllText(@"C:\Users\Luz Maldonado\Desktop\CargaPacientes\partes\datosgenerales.txt");
            //System.Console.WriteLine("Contenido del archivo = {0}", text);

            //foreach (char letra in text)
            //{
            //    System.Console.WriteLine("valor letra: "+ letra);
            //}

            //string pattern = @"([a-zA-Z]+[:]{1}|[a-zA-Z]+\s[a-zA-Z]+[:]{1}|[a-zA-Z]+\s[a-zA-Z]+\s[a-zA-Z]+[:]{1})";
            //[a-zA-ZÀ-ÿ]+ uno o mas letras, mayusculas y minusculas con o sin acento
            //            string pattern = @"([a-zA-ZÀ-ÿ]+[:]{1}|
            //[a-zA-ZÀ-ÿ]+\s[a-zA-ZÀ-ÿ]+[:]{1}|
            //[a-zA-ZÀ-ÿ]+\+[a-zA-ZÀ-ÿ]+\s+[a-zA-ZÀ-ÿ]+[:]{1}
            //)";
            //Regex  rgx =   new Regex();
            //string oat = @"\d";


            //[a-zA - ZÀ - ÿ]+uno o mas letras, mayusculas y minusculas con o sin acento
            // \s    Espacios de cualquier tipo. (espacio, tab, nueva linea)
            //[a-zA-ZÀ-ÿ]+[:]{1}  1 palabra
            //[a-zA-ZÀ-ÿ]+\s[a-zA-ZÀ-ÿ]+[:]{1}  2 palabras
            //( grupo1 | grupo2 )
            //Patron para tomar claves
            //string pattern = @"([a-zA-ZÀ-ÿ]+[:]{1}|[a-zA-ZÀ-ÿ]+\s[a-zA-ZÀ-ÿ]+[:]{1}|[a-zA-ZÀ-ÿ]+\s[a-zA-ZÀ-ÿ]+\s[a-zA-ZÀ-ÿ]+[:]{1}|[a-zA-ZÀ-ÿ]+\s[a-zA-ZÀ-ÿ]+\s[a-zA-ZÀ-ÿ]+\s[a-zA-ZÀ-ÿ]+[:]{1}|[a-zA-ZÀ-ÿ]+\s[a-zA-ZÀ-ÿ]+\s[a-zA-ZÀ-ÿ]+\s[a-zA-ZÀ-ÿ]+\s[a-zA-ZÀ-ÿ]+[:]{1})";

            string UnaPalabra = @"[a-zA-ZÀ-ÿ]+[:]{1}";
            string DosPalabra = @"[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+[:]{1}";
            string TresPalabra = @"[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+[:]{1}"; ;
            string CuatroPalabra = @"[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+[:]{1}";
            string CincoPalabra = @"[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+\s{0,1}[a-zA-ZÀ-ÿ]+[:]{1}";
            string pattern = @"(" + UnaPalabra + "|" + DosPalabra + "|" + TresPalabra + "|" + CuatroPalabra + "|" + CincoPalabra + ")";

            Regex rgx = new Regex(pattern);
            var matchCollection = rgx.Matches(text);


            string UnaPalabraVal = @"[:]{1}\s{2,25}[a-zA-Z0-9/.,]+";
            //string DosPalabraVal = @"[:]{1}\s{2,15}[a-zA-Z0-9/,]+\s{1}[a-zA-Z0-9/,]+";
            string TresPalabraVal = @"[:]{1}\s{2,15}[a-zA-Z0-9/,]+\s{1}[a-zA-Z0-9/,]+\s{1}[a-zA-Z0-9/,]+"; ;
            string CuatroPalabraVal = @"[:]{1}\s{2,15}[a-zA-Z0-9/,]+\s{1}[a-zA-Z0-9/,]+\s{1}[a-zA-Z0-9/,]+\s{1}[a-zA-Z0-9/,]+";
            string CincoPalabraVal = @"[:]{1}\s{2,15}[a-zA-Z0-9/,]+\s{1}[a-zA-Z0-9/,]+\s{1}[a-zA-Z0-9/,]+\s{1}[a-zA-Z0-9/,]+\s{1}[a-zA-Z0-9/,]+";
            //string pattern2 = @"(" + UnaPalabraVal + "|" + DosPalabraVal + "|" + TresPalabraVal + "|" + CuatroPalabraVal + "|" + CincoPalabraVal + ")";
            //string pattern2 = @"(" + UnaPalabraVal + "|"+ TresPalabraVal + "|" + CuatroPalabraVal + "|" + CincoPalabraVal + ")";
            string pattern2 = @"(" + CincoPalabraVal + "|" + CuatroPalabraVal + "|" + TresPalabraVal + "|" + UnaPalabraVal + ")";

            Regex rgx2 = new Regex(pattern2);
            var matchCollection2 = rgx2.Matches(text);
            //#region part1
            //foreach (Match match in matchCollection)
            //{
            //    Console.WriteLine("Claves '{0}'", match.Value);
            //}

            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();

            //string remplzar = "";
            //foreach (Match match in matchCollection2)
            //{
            //    remplzar = match.Value.Remove(0, 1);
            //    Console.WriteLine("Valores '{0}'", remplzar.Trim());
            //}
            //#endregion

            #region objeto datos generales
            string remplzar = "";
            int index = 0;
            string[] arr1 = new string[matchCollection2.Count];
            foreach (Match match2 in matchCollection2)
            {
                remplzar = match2.Value.Remove(0, 1);
                arr1[index] = remplzar.Trim();
                index++;
            }

            //index = 0;
            //string objPrincipa = "";
            //foreach (Match match in matchCollection)
            //{
            //    //tomar en cuenta a partir de cuenta paciente
            //    if (index >= 3)
            //    {
            //        //pendiente quitar  el \n y poner eb objeto c#
            //        objPrincipa += $"{match.Value}{arr1[index]},\n";
            //    }
            //    index++;
            //}
            //objPrincipa = objPrincipa.Remove(objPrincipa.Length - 2, 1);  //remover ulitma coma (,)
            //Console.WriteLine("{" + objPrincipa + "}");

            #endregion

            int index3 = 0;
            bool indexColumnas = false;
            bool enconcuentraTamColumnas = false;
            string[] numerocaracteres = null;
            string lineaEncabezados = null;
            
            foreach (string line in File.ReadLines(@"C:\Users\Luz Maldonado\Desktop\CargaPacientes\ejemplo1.txt"))
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
                    //Console.WriteLine(lineColumnas.Count());
                    enconcuentraTamColumnas = true;
                    continue; //evitar que inicie aqui
                }

                //identificar final de la tabla
                if (indexColumnas == true && line == "")
                    break;

                if (indexColumnas == true)
                {
                    Construye(numerocaracteres, lineaEncabezados, true);
                    Construye(numerocaracteres, line);
                    ConstruyeJsonServicios();
                    index3++;
                }
            }
            jsonServicios += "]";
            Console.WriteLine(jsonServicios);
        }
    }
}
