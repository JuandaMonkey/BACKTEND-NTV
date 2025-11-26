using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BACKEND_NTV.DTOs.Cortometraje
{
    public class CortometrajeDTO
    {
        [JsonPropertyName("idcortometraje")]
        public int IdCortometraje { get; set; }

        [JsonPropertyName("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [JsonPropertyName("autor")]
        public string Autor { get; set; } = string.Empty;

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }

        [JsonPropertyName("urlvideo")]
        public string? UrlVideo { get; set; }

        [JsonPropertyName("urlportada")]
        public string? UrlPortada { get; set; }

        [JsonPropertyName("duracionminutos")]
        public int DuracionMinutos { get; set; }

        [JsonPropertyName("duracionsegundos")]
        public int DuracionSegundos { get; set; }

        [JsonPropertyName("aniolanzamiento")]
        public int? AnioLanzamiento { get; set; }

        [JsonPropertyName("estado")]
        public bool Estado { get; set; }

        [JsonPropertyName("categorias")]
        public IEnumerable<string> Categorias { get; set; } = new List<string>();
    }
}
