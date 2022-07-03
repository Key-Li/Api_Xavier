using Api_Xavier;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Api_Xavier.Protos;



namespace Api_Xavier.Services
{

    public class AlumnoManagerService : AlumnoManager.AlumnoManagerBase
    {

        private readonly ILogger<AlumnoManagerService> _logger;
        private List<Alumno> alumnos;


        public AlumnoManagerService(ILogger<AlumnoManagerService> logger)
        {

            _logger = logger;
            alumnos = lista();
        }

        List<Alumno> lista()
        {
            List<Alumno> temporal = new List<Alumno>();

            using (SqlConnection cn = new SqlConnection(@"server=DESKTOP-382FNK8\SQLEXPRESS; database=XAVIER_INSTITUTE; Trusted_Connection=True;"
                    + "MultipleActiveResultSets=True; TrustServerCertificate=False; Encrypt=False"))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("select * from tb_alumnos", cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Alumno()
                    {
                        Idalumno = dr["idalumno"] + "",
                        Nomalum = dr.GetString(1),
                        Apealum = dr.GetString(2),
                        Dnialum = dr.GetString(3),
                        Fechalum = dr.GetString(4),
                        Celalum = dr.GetString(5),
                        Usualum = dr.GetString(6),
                        Passalum = dr.GetString(7),

                    });
                }
                dr.Close();
            }
            return temporal;
        }
        public override Task<Alumnos> GetAll(Empty request, ServerCallContext context)
        {
            Alumnos result = new Alumnos();
            result.Items.AddRange(alumnos);
            return Task.FromResult(result);
        }

        public override Task<Alumnos> ListadoNombre(AlumnoId request, ServerCallContext context)

        {
            Alumnos result = new Alumnos();
            result.Items.AddRange(lista().Where(c => c.Idalumno .StartsWith(request.Id)).ToArray());
            return Task.FromResult(result);

        }

        /* public override Task<Client> Get(ClientId request, ServerCallContext context)
            {
                Client result = new Client();
                result = clientes.Where(c => c.Idcli == request.Id).First();
                return Task.FromResult(result);
            }

            public override Task<Clients> ListadoNombre(ClientId request, ServerCallContext context)

            {
                Clients result= new Clients();
                result.Items.AddRange(lista().Where(c => c.Idcli.StartsWith(request.Id)).ToArray());
                return Task.FromResult(result);

            }*/


    }
}
