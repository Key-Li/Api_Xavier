using Api_Xavier;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Api_Xavier.Protos;


namespace Api_Xavier.Services
{
    public class ProfesorManagerService : ProfesorManager.ProfesorManagerBase
    {

        private readonly ILogger<ProfesorManagerService> _logger;
        private List<Profesor> profesores;

        public ProfesorManagerService(ILogger<ProfesorManagerService> logger)
        {
            _logger = logger;
            profesores = lista();
        }

        List<Profesor> lista()
        {
            List<Profesor> temporal = new List<Profesor>();

            using (SqlConnection cn = new SqlConnection(@"server=DESKTOP-382FNK8\SQLEXPRESS; database=XAVIER_INSTITUTE; Trusted_Connection=True;"
                    + "MultipleActiveResultSets=True; TrustServerCertificate=False; Encrypt=False"))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("select * from tb_profesores", cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Profesor()
                    {
                        Idprof = dr["idprof"].ToString(),
                        Nomprof = dr.GetString(1),
                        Apeprof = dr.GetString(2),
                        Espeprof = dr.GetString(3),
                        Dniprof = dr.GetString(4),
                        Celprof = dr.GetString(5),
                    }) ;

                }
                dr.Close();
                
            }
            return temporal;
        }

        public override Task<Profesores> GetAll(EmptyP request, ServerCallContext context)
        {
            Profesores result = new Profesores();
            result.Items.AddRange(profesores);
            return Task.FromResult(result);
        }
    }
}
