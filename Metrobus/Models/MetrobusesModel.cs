using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Metrobus.Models
{
    public class MetrobusesModel
    {
        /// <summary>
        /// Identificador del vehículo
        /// </summary>
        public int Vehicle_ID { get; set; }
        /// <summary>
        /// Latitud del Metrobus
        /// </summary>
        public string Latitud { get; set; }
        /// <summary>
        /// Longitud del Metrobus
        /// </summary>
        public string Longitud { get; set; }
        /// <summary>
        /// Fecha de actualización de las coordenadas del Metrobus
        /// </summary>
        public DateTime Date_Update { get; set; }
    }
}
