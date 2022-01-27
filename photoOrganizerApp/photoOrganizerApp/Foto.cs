using System;
using System.Collections.Generic;
using System.Text;

namespace photoOrganizerApp
{
    public class Foto
    {
        public string nombre { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaUltModificacion { get; set; }
        public DateTime fechaCaptura { get; set; }
        public string extension { get; set; }
    }
}
