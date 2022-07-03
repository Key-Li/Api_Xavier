using Api_Xavier;
using Grpc.Core;
using Microsoft.Data.SqlClient;
using Api_Xavier.Protos;

namespace Api_Xavier.Services
{
    public class CursoManagerService : CursosManager.CursosManagerBase
    {
        private readonly ILogger<CursoManagerService> _logger;
        private List<Curso> cursos;

        public CursoManagerService(ILogger<CursoManagerService> logger)
        {
            _logger = logger;
            cursos = lista();
        }

        List<Curso> lista()
        {
            List<Curso> temporal = new List<Curso>();

            using (SqlConnection cn = new SqlConnection(@"server=DESKTOP-382FNK8\SQLEXPRESS; database=XAVIER_INSTITUTE; Trusted_Connection=True;"
                    + "MultipleActiveResultSets=True; TrustServerCertificate=False; Encrypt=False"))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("select * from tb_cursos", cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Curso()
                    {
                        Idcurso = dr["idcurso"] + "",
                        Nomcurso = dr.GetString(1),
                        Idhorario = dr["idhorario"] +"",
                        Idprof = dr["idprof"] +"",


                    });
                }
                dr.Close();
            }
            return temporal;
        }
        public override Task<Cursos> GetAll(EmptyC request, ServerCallContext context)
        {
            Cursos result = new Cursos();
            result.Items.AddRange(cursos);
            return Task.FromResult(result);
        }

        public override Task<Cursos> ListadoCurso(CursoId request, ServerCallContext context)

        {
            Cursos result = new Cursos();
            result.Items.AddRange(lista().Where(c => c.Idcurso.StartsWith(request.Id)).ToArray());
            return Task.FromResult(result);

        }

    }
}
