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
                    var urlMetrobusByID = "https://localhost:44325/api/Metrobus/id?id=";
                    var urlInserMetrobus = "https://localhost:44325/api/Metrobus";

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

                                var metrobusFindJSON = await http.GetStringAsync(urlMetrobusByID + metrobus.Vehicle_ID.ToString());

                                if (!string.IsNullOrEmpty(metrobusFindJSON))
                                {
                                    var metrobusFind = JsonConvert.DeserializeObject<MetrobusesModel>(metrobusFindJSON);
                                    if (metrobusFind.Vehicle_ID != metrobus.Vehicle_ID && metrobusFind.Latitud != metrobus.Latitud && metrobusFind.Longitud != metrobus.Longitud && metrobusFind.Date_Update != metrobus.Date_Update)
                                    {
                                        var json = JsonConvert.SerializeObject(metrobus);
                                        var metrobusNew = new StringContent(json, Encoding.UTF8, "application/json");
                                        var insertMetrobus = await http.PostAsync(urlInserMetrobus, metrobusNew);
                                    }
                                }
                                else
                                {
                                    var json = JsonConvert.SerializeObject(metrobus);
                                    var metrobusNew = new StringContent(json, Encoding.UTF8, "application/json");
                                    var insertMetrobus = await http.PostAsync(urlInserMetrobus, metrobusNew);
                                }
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
                var urlMetrobus = "https://localhost:44325/api/Metrobus";
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
                var url = "https://localhost:44325/api/Metrobus/id?id=" + id.ToString();

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
            var url = "https://localhost:44325/api/Alcaldias";

            using(var http =  new HttpClient())
            {
                var response = await http.GetStringAsync(url);
                ViewBag.alcaldias = JsonConvert.DeserializeObject<List<AlcaldiasModel>>(response);
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
