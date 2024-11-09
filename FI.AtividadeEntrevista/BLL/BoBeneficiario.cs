using System.Collections.Generic;
using System.Text.RegularExpressions;
using WebAtividadeEntrevista.DTOs;

namespace WebAtividadeEntrevista.DAO
{
    public class BoBeneficiario
    {
        public long Incluir(Beneficiario beneficiario)
        {
            beneficiario.CPF = MascaraCPF(beneficiario.CPF);
            DaoBeneficiario daoBeneficiario = new DaoBeneficiario();
            return daoBeneficiario.Incluir(beneficiario);
        }

        public void Alterar(Beneficiario beneficiario)
        {
            DaoBeneficiario daoBeneficiario = new DaoBeneficiario();
            daoBeneficiario.Alterar(beneficiario);
        }

        public Beneficiario Consultar(long id)
        {
            DaoBeneficiario daoBeneficiario = new DaoBeneficiario();
            return daoBeneficiario.Consultar(id);
        }

        public void Excluir(long id)
        {
            DaoBeneficiario daoBeneficiario = new DaoBeneficiario();
            daoBeneficiario.Excluir(id);
        }

        public List<Beneficiario> Listar()
        {
            DaoBeneficiario daoBeneficiario = new DaoBeneficiario();
            return daoBeneficiario.Listar();
        }

        public List<Beneficiario> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DaoBeneficiario daoBeneficiario = new DaoBeneficiario();
            return daoBeneficiario.Pesquisa(
                iniciarEm, quantidade, campoOrdenacao, crescente, out qtd);
        }

        public bool VerificarExistencia(string CPF)
        {
            DaoBeneficiario daoBeneficiario = new DaoBeneficiario();
            return daoBeneficiario.VerificarExistencia(CPF);
        }

        public string MascaraCPF(string cpf)
        {
            cpf = Regex.Replace(cpf, @"\D", "");
            cpf = Regex.Replace(cpf, @"(\d{3})(\d{3})(\d{3})(\d{2})", "$1.$2.$3-$4");
            return cpf;
        }
    }
}