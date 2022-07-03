using Microsoft.AspNetCore.Mvc;
using Api_Xavier;
using Api_Xavier.Client.Models;
using Grpc.Net.Client;
using Api_Xavier.Protos;

namespace Api_Xavier.Client.Controllers
{
    public class CursoController : Controller
    {
        private CursosManager.CursosManagerClient cursoManagerClient;
        public async Task<IActionResult> Index()
        {

            var canal = GrpcChannel.ForAddress("https://localhost:7055");
            cursoManagerClient = new CursosManager.CursosManagerClient(canal);

            var request = new EmptyC();
            var response = await cursoManagerClient.GetAllAsync(request);

            List<CursoModel> models = new List<CursoModel>();
            foreach (var model in response.Items)
                models.Add(new CursoModel()
                {
                    idcurso = model.Idcurso,
                    nomcurso = model.Nomcurso,
                    idhorario = model.Idhorario,
                    idprof = model.Idprof,
                });

            return View(models);
        }

        public async Task<IActionResult> Busqueda(string nombre = "")

        {
            var canal = GrpcChannel.ForAddress("https://localhost:7055");
            cursoManagerClient = new CursosManager.CursosManagerClient(canal);


            var request = new CursoId();

            request.Id = nombre;

            var mensaje = await cursoManagerClient.ListadoCursoAsync(request);



            List<CursoModel> models = new List<CursoModel>();
            foreach (var model in mensaje.Items)
                models.Add(new CursoModel()
                {
                    idcurso = model.Idcurso,
                    nomcurso = model.Nomcurso,
                    idhorario = model.Idhorario,
                    idprof = model.Idprof,

                });
            return View(models);
        }
    }
}
