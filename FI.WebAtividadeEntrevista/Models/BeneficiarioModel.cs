using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models
{
    public class BeneficiarioModel
    {
        public long Id { get; set; }

        public long IdCliente { get; set; }

        [Required(ErrorMessage = "CPF obrigatório")]
        public string CPF { get; set; }
        
        public string NOME { get; set; }

    }
}