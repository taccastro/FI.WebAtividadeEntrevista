using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models
{
    public class BeneficiarioModel
    {
        public long Id { get; set; }
        /// <summary>
        /// Nome do Beneficiário    
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// CPF do Beneficiário
        /// </summary>
        [Required]
        public string CPF { get; set; }

        /// <summary>
        /// Id do Cliente   
        /// </summary>
        public long? ClienteId { get; set; }
    }
}