using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Drawing;
using System.Globalization;
using System.Drawing.Imaging;
using System.Threading.Tasks.Sources;

namespace photoOrganizerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var computador = new Computador();

            Console.WriteLine("Ingrese la ruta del directorio de sus fotos");
            var rutaDirectorio = Console.ReadLine();
            Console.WriteLine("Ingrese la ruta de destino para clasificar las fotos");
            var rutaDestino = Console.ReadLine();
            try
            {
                computador.ClasificarFotos(rutaDirectorio, rutaDestino);
                Console.WriteLine("FOTOS ORDENADAS CORRECTAMENTE");
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }
            
        }
    }

}

                    

              
        
    



    

