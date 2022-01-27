using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Drawing;
using System.Globalization;
using System.Drawing.Imaging;
using System.Threading.Tasks.Sources;

namespace photoOrganizerApp
{
    public class Computador
    {
        public Computador()
        {
            fotos = new List<Foto>();
        }
        public List<Foto> fotos { get; set; }

        public void ClasificarFotos(string rutaDirectorio, string rutaDestino)
        {
            var extensiones = "*.jpg,*.jpeg,*.png,*.gif"; //extensiones que pidió el profesor

            foreach (var archivo in Directory.GetFiles(rutaDirectorio, "*.*", SearchOption.AllDirectories).Where(s => extensiones.Contains(Path.GetExtension(s).ToLower()))) //busca los archivos en ruta entregada
            {
                Foto nuevaFoto = new Foto();

                nuevaFoto.fechaCreacion = File.GetCreationTime(archivo); //fecha creación foto
                nuevaFoto.fechaUltModificacion = File.GetLastWriteTime(archivo); //fecha última modificación de la foto
                nuevaFoto.extension = Path.GetExtension(archivo); //obtiene extension de la foto
                nuevaFoto.nombre = Path.GetFileNameWithoutExtension(archivo); // obtiene nombre de la foto


                var anioCreFoto = nuevaFoto.fechaCreacion.Year; // año de creación de la foto
                var anioUltModFoto = nuevaFoto.fechaUltModificacion.Year; //año de ult modificación de la foto
                var mesCreFoto = nuevaFoto.fechaCreacion.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-CL")); //mes de creación de la foto
                var diaCreFoto = nuevaFoto.fechaCreacion.Day; // dia de creacion de la foto
                var mesUltModFoto = nuevaFoto.fechaUltModificacion.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-CL")); ; //mes de ult modificación de la foto
                var diaUltModFoto = nuevaFoto.fechaUltModificacion.Day; //dia de ult modificacion de la foto

                Image imagen = new Bitmap(archivo); //cada foto necesito convertirla

                string nombreArchivoParaRuta; //nombre archivo (fecha horaMinuto .extension)
                string rutaFinal; //ruta con todo (carpetas+nombre)

                if (imagen.PropertyIdList.Any(p => p == 36867)) //si es que en la lista de propiedades existe el id= 36867 (fecha captura)
                {
                    PropertyItem FechaCapturaEXIF = imagen.GetPropertyItem(0x9003); //obtengo id de la propiedad fecha de captura
                    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding(); //decodificador
                    string valor = encoding.GetString(FechaCapturaEXIF.Value); // recibo la fecha en string

                    var espacio = valor.Split(':', ' '); // spliteo la fecha de captura(EXIF) que es un string yyyy:mm:dd hh:mm, necesito splitear el espacio y los :
                    var anioCaptura = espacio[0]; //año captura de foto
                    var mesCaptura = espacio[1]; //mes captura de foto
                    var diaCaptura = espacio[2]; //dia captura de foto
                    var horaCaptura = espacio[3];//hora captura de foto
                    var minutoCaptura = espacio[4]; //minuto captura de foto
                    var segundosCaptura = espacio[5]; //segundo captura de foto
                    var fechaEntera = diaCaptura + "/" + mesCaptura + "/" + anioCaptura + " " + horaCaptura + ":" + minutoCaptura + ":" + segundosCaptura; //armo un string con la fecha para poder transformarla a datetime

                    nuevaFoto.fechaCaptura = DateTime.Parse(fechaEntera); //fecha de captura de foto en datetime lista para usar

                    nombreArchivoParaRuta = nuevaFoto.fechaCaptura.ToString("ddMMyy HHmm") + nuevaFoto.extension; //Nombre archivo + extension
                    rutaFinal = rutaDestino + "\\" + anioCaptura + "\\" + nuevaFoto.fechaCaptura.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-CL")) + "\\" + nombreArchivoParaRuta; //ruta final del archivo en su respectiva carpeta

                    if (File.Exists(rutaFinal)) //si el archivo ya existe
                    {
                        string nuevaRuta = rutaFinal;
                        int contador = 1;
                        while (File.Exists(nuevaRuta)) // mientras el archivo exista el nombre tendra un nombre (i) .extension, i aumenta
                        {
                            string tempFileName = string.Format("{0}({1}){2}", nuevaFoto.fechaCaptura.ToString("ddMMyy HHmm"), contador++, nuevaFoto.extension);

                            Directory.CreateDirectory(rutaDestino + "\\" + anioCaptura + "\\" + nuevaFoto.fechaCaptura.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-CL"))); // crea directorio si es que no existe ruta+año+mes

                            nuevaRuta = Path.Combine(rutaDestino + "\\" + anioCaptura + "\\" + nuevaFoto.fechaCaptura.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-CL")), tempFileName); //ruta final del archivo en su respectiva carpeta


                        }
                        File.Copy(archivo, nuevaRuta);




                    }
                    else
                    {
                        nombreArchivoParaRuta = nuevaFoto.fechaCaptura.ToString("ddMMyy HHmm") + nuevaFoto.extension; //Nombre archivo + extension
                        Directory.CreateDirectory(rutaDestino + "\\" + anioCaptura + "\\" + nuevaFoto.fechaCaptura.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-CL"))); // crea directorio si es que no existe ruta+año+mes
                        rutaFinal = rutaDestino + "\\" + anioCaptura + "\\" + nuevaFoto.fechaCaptura.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-CL")) + "\\" + nombreArchivoParaRuta; //ruta final del archivo en su respectiva carpeta
                        File.Copy(archivo, rutaFinal); // copia el archivo sin borrar el original
                    }




                }
                else if (!(imagen.PropertyIdList.Any(p => p == 36867))) //si no existe fecha captura
                {
                    nombreArchivoParaRuta = nuevaFoto.fechaUltModificacion.ToString("ddMMyy HHmm") + nuevaFoto.extension; //nombre archivo
                    rutaFinal = rutaDestino + "\\" + anioUltModFoto + "\\" + mesUltModFoto + "\\" + nombreArchivoParaRuta; //ruta del archivo en su respectiva carpeta


                    if (File.Exists(rutaFinal)) //si el archivo ya existe 
                    {
                        int contador = 1;
                        string nuevaRuta = rutaFinal;
                        while (File.Exists(nuevaRuta))// mientras el archivo exista el nombre tendra un nombre (i) .extension, i aumenta
                        {
                            string tempFileName = string.Format("{0}({1}){2}", nuevaFoto.fechaUltModificacion.ToString("ddMMyy HHmm"), contador++, nuevaFoto.extension);

                            Directory.CreateDirectory(rutaDestino + "\\" + anioUltModFoto + "\\" + mesUltModFoto); // crea directorio si es que no existe ruta+año+mes

                            nuevaRuta = Path.Combine(rutaDestino + "\\" + anioUltModFoto + "\\" + mesUltModFoto, tempFileName);//ruta final del archivo en su respectiva carpeta


                        }
                        File.Copy(archivo, nuevaRuta);// copia el archivo sin borrar el original


                    }
                    else
                    {

                        nombreArchivoParaRuta = nuevaFoto.fechaUltModificacion.ToString("ddMMyy HHmm") + nuevaFoto.extension; //nombre archivo
                        Directory.CreateDirectory(rutaDestino + "\\" + anioUltModFoto + "\\" + mesUltModFoto); //crea el directorio si es que no existe, ruta+año+mes
                        rutaFinal = rutaDestino + "\\" + anioUltModFoto + "\\" + mesUltModFoto + "\\" + nombreArchivoParaRuta; //ruta del archivo en su respectiva carpeta
                        File.Copy(archivo, rutaFinal); // copia el archivo sin borrar el original

                    }

                }
                else if (!(imagen.PropertyIdList.Any(p => p == 36867)) || nuevaFoto.fechaUltModificacion == null)
                {
                    nombreArchivoParaRuta = nuevaFoto.fechaCreacion.ToString("ddMMyy HHmmss") + nuevaFoto.extension;
                    rutaFinal = rutaDestino + "\\" + anioCreFoto + "\\" + mesCreFoto + "\\" + nombreArchivoParaRuta;

                    if (File.Exists(rutaFinal)) //si el archivo existe le agrego los segundos al final
                    {
                        int contador = 1;
                        string nuevaRuta = rutaFinal;
                        while (File.Exists(nuevaRuta)) // mientras el archivo exista el nombre tendra un nombre (i) .extension, i aumenta
                        {
                            string tempFileName = string.Format("{0}({1}){2}", nuevaFoto.fechaCreacion.ToString("ddMMyy HHmm"), contador++, nuevaFoto.extension);

                            Directory.CreateDirectory(rutaDestino + "\\" + anioCreFoto + "\\" + mesCreFoto); // crea directorio si es que no existe ruta+año+mes

                            nuevaRuta = Path.Combine(rutaDestino + "\\" + anioCreFoto + "\\" + mesCreFoto, tempFileName);  //ruta final del archivo en su respectiva carpeta


                        }
                        File.Copy(archivo, nuevaRuta);


                    }
                    else
                    {
                        nombreArchivoParaRuta = nuevaFoto.fechaCreacion.ToString("ddMMyy HHmm") + nuevaFoto.extension; //nombre archivo + extension
                        Directory.CreateDirectory(rutaDestino + "\\" + anioCreFoto + "\\" + mesCreFoto); // crea el directorio si es que no existe, ruta+año+mes
                        rutaFinal = rutaDestino + "\\" + anioCreFoto + "\\" + mesCreFoto + "\\" + nombreArchivoParaRuta; // ruta del archivo en su respectiva carpeta
                        File.Copy(archivo, rutaFinal); // copia el archivo sin borrar el original
                    }

                }
                else if (!(imagen.PropertyIdList.Any(p => p != 36867)) || nuevaFoto.fechaUltModificacion == null || nuevaFoto.fechaCreacion == null)// si no exisste ni fecha captura ni fecha ult modificacion ni fechacreacion
                {

                    nombreArchivoParaRuta = nuevaFoto.nombre + nuevaFoto.extension; //nombre archivo
                    Directory.CreateDirectory(rutaDestino + "\\" + "Desconocido"); // crea el directorio si es que no existe, ruta+año+mes
                    rutaFinal = rutaDestino + "\\" + "Desconocido" + "\\" + nombreArchivoParaRuta; // ruta del archivo en su respectiva carpeta
                    File.Copy(archivo, rutaFinal); // copia el archivo sin borrar el original

                }





                fotos.Add(nuevaFoto);
            }

        }
    }
}
