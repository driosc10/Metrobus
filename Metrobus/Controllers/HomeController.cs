using Metrobus.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Metrobus.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Consulta(int consulta)
        {
            HttpContext.Session.Clear();
            try
            {
                if (consulta > 0)
                {
                    var urlMetrobusCDMX = "https://datos.cdmx.gob.mx/api/3/action/datastore_search?resource_id=ad360a0e-b42f-482c-af12-1fd72140032e";
                    var urlMetrobusByID = "http://localhost:8080/api/Metrobus/id?id=";
                    var urlInserMetrobus = "http://localhost:8080/api/Metrobus";

                    using (var http = new HttpClient())
                    {
                        var responseCDMX = await http.GetStringAsync(urlMetrobusCDMX);

                        var data = JsonConvert.DeserializeObject<dynamic>(responseCDMX);

                        var result = data["result"];
                        var records = result["records"];

                        foreach(var item in records)
                        {
                            var metrobus = new MetrobusesModel();

                            metrobus.Vehicle_ID = item["vehicle_id"];
                            if (metrobus.Vehicle_ID > 0)
                            {
                                metrobus.Date_Update = item["date_updated"];
                                metrobus.Latitud = item["position_latitude"];
                                metrobus.Longitud = item["position_longitude"];
                                var json = JsonConvert.SerializeObject(metrobus);
                                var metrobusNew = new StringContent(json, Encoding.UTF8, "application/json");
                                var insertMetrobus = await http.PostAsync(urlInserMetrobus, metrobusNew);
                                
                            }
                        }
                        ViewBag.ErrorMessage = "La información se consulto de forma correcta";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            finally
            {
                var urlMetrobus = "http://localhost:8080/api/Metrobus";
                using (var http = new HttpClient())
                {
                    var response = await http.GetStringAsync(urlMetrobus);
                    ViewBag.metrobuses = JsonConvert.DeserializeObject<List<MetrobusesModel>>(response).OrderByDescending(x => x.Date_Update).OrderBy(x => x.Vehicle_ID).ToList();
                }
            }
            return View();
        }

        [HttpGet]

        public void SetHistorial(int id)
        {
            HttpContext.Session.SetString("VehicleID",id.ToString());
        }

        public async Task<IActionResult> Historial()
        {
            string id = HttpContext.Session.GetString("VehicleID");
            if (!string.IsNullOrEmpty(id))
            {
                var url = "http://localhost:8080/api/Metrobus/id?id=" + id.ToString();

                using (var http = new HttpClient())
                {
                    var response = await http.GetStringAsync(url);
                    ViewBag.metrobuses = JsonConvert.DeserializeObject<List<MetrobusesModel>>(response);
                }
            }
            return View();
        }
        public async Task<IActionResult> Alcaldias()
        {
            HttpContext.Session.Clear();
            var url = "http://localhost:8080/api/Alcaldias";

            using(var http =  new HttpClient())
            {
                var response = await http.GetStringAsync(url);
                var alcaldias = JsonConvert.DeserializeObject<List<AlcaldiasModel>>(response);

                if(alcaldias.Count() == 0)
                {
                    List<AlcaldiasModel> alcaldiasList = new List<AlcaldiasModel>();
                    alcaldiasList = GetAlcaldias();

                    foreach (var alcadia in alcaldiasList)
                    {
                        var json = JsonConvert.SerializeObject(alcadia);
                        var alcadiaNew = new StringContent(json, Encoding.UTF8, "application/json");
                        var insertAlcadia = await http.PostAsync(url, alcadiaNew);
                    }

                    var response2 = await http.GetStringAsync(url);
                    var alcaldias2 = JsonConvert.DeserializeObject<List<AlcaldiasModel>>(response);
                    ViewBag.alcaldias = alcaldias2;
                }
                else
                {
                    ViewBag.alcaldias = alcaldias;
                }
            }

            return View();
        }

        private List<AlcaldiasModel> GetAlcaldias()
        {
            List<AlcaldiasModel> alcaldiasList = new List<AlcaldiasModel>();
            alcaldiasList.Add(new AlcaldiasModel
            {
                Name = "Milpa Alta",
                Latitud = "19.1394565999",
                Longitud = "-99.0510954218"
            });

            alcaldiasList.Add(new AlcaldiasModel
            {
                Name = "Benito Juárez",
                Latitud = "19.3806424162",
                Longitud = "-99.1611346584"
            });

            alcaldiasList.Add(new AlcaldiasModel
            {
                Name = "Gustavo A. Madero",
                Latitud = "19.5040652077",
                Longitud = "-99.1158642087"
            });

            alcaldiasList.Add(new AlcaldiasModel
            {
                Name = "Coyoacan",
                Latitud = "19.3266672536",
                Longitud = "-99.1503763525"
            });

            alcaldiasList.Add(new AlcaldiasModel
            {
                Name = "Miguel Hidalgo",
                Latitud = "19.4280623649",
                Longitud = "-99.2045669144"
            });

            alcaldiasList.Add(new AlcaldiasModel
            {
                Name = "La Magdalena Contreras",
                Latitud = "19.2689765031",
                Longitud = "-99.2684129061"
            });

            alcaldiasList.Add(new AlcaldiasModel
            {
                Name = "Tláhuac",
                Latitud = "19.2769983772",
                Longitud = "-99.0028216137"
            });

            alcaldiasList.Add(new AlcaldiasModel
            {
                Name = "Azcapotzalco",
                Latitud = "19.4853286147",
                Longitud = "-99.1821069423"
            });

            alcaldiasList.Add(new AlcaldiasModel
            {
                Name = "Iztacalco",
                Latitud = "19.396911897",
                Longitud = "-99.094329797"
            });

            alcaldiasList.Add(new AlcaldiasModel
            {
                Name = "Álvaro Obregón",
                Latitud = "19.336175562",
                Longitud = "-99.246819712"
            });

            alcaldiasList.Add(new AlcaldiasModel
            {
                Name = "Xochimilco",
                Latitud = "19.2451450458",
                Longitud = "-99.0903636045"
            });

            alcaldiasList.Add(new AlcaldiasModel
            {
                Name = "Venustiano Carranza",
                Latitud = "19.4304954545",
                Longitud = "-99.0931057959"
            });

            alcaldiasList.Add(new AlcaldiasModel
            {
                Name = "Tlalpan",
                Latitud = "19.1983396763",
                Longitud = "-99.2062207957"
            });

            alcaldiasList.Add(new AlcaldiasModel
            {
                Name = "Cuajimalpa de Morelos",
                Latitud = "19.3246343001",
                Longitud = "-99.3107285253"
            });

            alcaldiasList.Add(new AlcaldiasModel
            {
                Name = "Cuauhtémoc",
                Latitud = "19.4313734294",
                Longitud = "-99.1490557562"
            });

            alcaldiasList.Add(new AlcaldiasModel
            {
                Name = "Iztapalapa",
                Latitud = "19.3491663204",
                Longitud = "-99.0567989642"
            });

            return alcaldiasList;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
