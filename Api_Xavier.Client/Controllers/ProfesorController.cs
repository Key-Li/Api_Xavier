using Microsoft.AspNetCore.Mvc;
using Api_Xavier;
using Api_Xavier.Client.Models;
using Grpc.Net.Client;
using Api_Xavier.Protos;

namespace Api_Xavier.Client.Controllers
{
    public class ProfesorController : Controller
    {
        private ProfesorManager.ProfesorManagerClient profesorManagerClient;
        public async Task<IActionResult> Index()
        {

            var canal = GrpcChannel.ForAddress("https://localhost:7055");
            profesorManagerClient = new ProfesorManager.ProfesorManagerClient(canal);

            var request = new EmptyP();
            var response = await profesorManagerClient.GetAllAsync(request);

            List<ProfesorModel> models = new List<ProfesorModel>();
            foreach (var model in response.Items)
                models.Add(new ProfesorModel()
                {
                    idprof = model.Idprof,
                    nomprof = model.Nomprof,
                    apeprof = model.Apeprof,
                    espeprof = model.Espeprof,
                    dniprof = model.Dniprof,
                    celprof = model.Celprof,
                });

            return View(models);
        }
    }
}
