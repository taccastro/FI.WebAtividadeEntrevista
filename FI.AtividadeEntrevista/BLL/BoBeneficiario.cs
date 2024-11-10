using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WebAtividadeEntrevista.DTOs;

namespace WebAtividadeEntrevista.DAO
{
    public class BoBeneficiario
    {
        public long Incluir(Beneficiario beneficiario)
        {
            if (!ValidarCPF(beneficiario.CPF))
            {
                throw new Exception("CPF inválido.");
            }

            beneficiario.CPF = MascaraCPF(beneficiario.CPF);
            DaoBeneficiario daoBeneficiario = new DaoBeneficiario();


            if (VerificarExistencia(beneficiario.CPF))
            {
                throw new Exception("Já existe um beneficiário com esse CPF para o mesmo cliente.");
            }

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

        public List<Beneficiario> Pesquisa(int idCliente, int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DaoBeneficiario daoBeneficiario = new DaoBeneficiario();
            return daoBeneficiario.Pesquisa(idCliente, iniciarEm, quantidade, campoOrdenacao, crescente, out qtd);
        }

        public bool VerificarExistencia(string CPF)
        {

            DaoBeneficiario daoBeneficiario = new DaoBeneficiario();
            return daoBeneficiario.VerificarExistencia(CPF);
        }

        public bool ValidarCPF(string cpf)
        {

            cpf = cpf.Replace(".", "").Replace("-", "");


            if (cpf.Length != 11 || cpf.All(c => c == cpf[0]))
            {
                return false;
            }

            int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };


            int soma1 = 0;
            for (int i = 0; i < 9; i++)
            {
                soma1 += int.Parse(cpf[i].ToString()) * multiplicadores1[i];
            }

            int resto1 = soma1 % 11;
            int digito1 = (resto1 < 2) ? 0 : 11 - resto1;


            int soma2 = 0;
            for (int i = 0; i < 10; i++)
            {
                soma2 += int.Parse(cpf[i].ToString()) * multiplicadores2[i];
            }

            int resto2 = soma2 % 11;
            int digito2 = (resto2 < 2) ? 0 : 11 - resto2;


            return cpf[9].ToString() == digito1.ToString() && cpf[10].ToString() == digito2.ToString();
        }


        public string MascaraCPF(string cpf)
        {
            cpf = Regex.Replace(cpf, @"\D", "");
            cpf = Regex.Replace(cpf, @"(\d{3})(\d{3})(\d{3})(\d{2})", "$1.$2.$3-$4");
            return cpf;
        }
    }
}