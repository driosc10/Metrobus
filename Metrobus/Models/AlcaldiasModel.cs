using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Metrobus.Models
{
    public class AlcaldiasModel
    {
        /// <summary>
        /// Identificador único de la Alcaldia
        /// </summary>
        public int AlcaldiaID { get; set; }
        /// <summary>
        /// Nombre de la Alcaldia
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Latitud de la Alcaldia
        /// </summary>
        public string Latitud { get; set; }
        /// <summary>
        /// Longitud de la Alcaldia
        /// </summary>
        public string Longitud { get; set; }
    }
}
