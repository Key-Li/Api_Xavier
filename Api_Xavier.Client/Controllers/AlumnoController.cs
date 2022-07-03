using Microsoft.AspNetCore.Mvc;
using Api_Xavier;
using Api_Xavier.Client.Models;
using Grpc.Net.Client;
using Api_Xavier.Protos;

namespace Api_Xavier.Client.Controllers
{
    public class AlumnoController : Controller
    {
        private AlumnoManager.AlumnoManagerClient alumnoManagerClient;
        public async Task<IActionResult> Index()
        {

            var canal = GrpcChannel.ForAddress("https://localhost:7055");
            alumnoManagerClient = new AlumnoManager.AlumnoManagerClient(canal);

            var request = new Empty();
            var response = await alumnoManagerClient.GetAllAsync(request);

            List<AlumnoModel> models = new List<AlumnoModel>();
            foreach (var model in response.Items)
                models.Add(new AlumnoModel()
                {
                    idalumno = model.Idalumno,
                    nomalum = model.Nomalum,
                    apealum = model.Apealum,
                    dnialum = model.Dnialum,
                    fechalum = model.Fechalum,
                    celalum = model.Celalum,
                    usualum = model.Usualum,
                    passalum = model.Passalum,
                });

            return View(models);
        }

        public async Task<IActionResult> Busqueda(string nombre = "")

        {
            var canal = GrpcChannel.ForAddress("https://localhost:7055");
            alumnoManagerClient = new AlumnoManager.AlumnoManagerClient(canal);


            var request = new AlumnoId();

            request.Id = nombre;

            var mensaje = await alumnoManagerClient.ListadoNombreAsync(request);



            List<AlumnoModel> models = new List<AlumnoModel>();
            foreach (var model in mensaje.Items)
                models.Add(new AlumnoModel()
                {
                    idalumno = model.Idalumno,
                    nomalum = model.Nomalum,
                    apealum = model.Apealum,
                    dnialum = model.Dnialum,
                    fechalum = model.Fechalum,
                    celalum = model.Celalum,
                    usualum = model.Usualum,
                    passalum = model.Passalum,

                });
            return View(models);
        }
    }
}
