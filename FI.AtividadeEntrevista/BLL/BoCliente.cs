using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente)
        {
            cliente.CPF = MascaraCPF(cliente.CPF);
            
            if (!CPFValido(cliente.CPF))
            {
                throw new Exception("O CPF fornecido é inválido.");
            }

            DAL.DaoCliente cli = new DAL.DaoCliente();

            // Verifica se o CPF já existe
            if (cli.VerificarExistencia(cliente.CPF))
            {
                throw new Exception("Já existe um cliente com esse CPF.");
            }
            else
            {
                return cli.Incluir(cliente);
            }            
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Cliente cliente)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Alterar(cliente);
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public DML.Cliente Consultar(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Consultar(id);
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Listar()
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Pesquisa(iniciarEm, quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.VerificarExistencia(CPF);
        }

         /// <summary>
        /// Verifica se o CPF é válido
        /// </summary>
        /// <param name="cpf">CPF a ser validado</param>
        /// <returns>Retorna true se o CPF for válido; caso contrário, false</returns>
        public bool CPFValido(string cpf)
        {
            cpf = Regex.Replace(cpf, @"\D", ""); // Remove caracteres não numéricos

            if (cpf.Length != 11 || Regex.IsMatch(cpf, @"(\d)\1{10}"))
            {
                return false; // CPF precisa ter 11 dígitos e não pode ser uma sequência repetida
            }

            int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            // Calcula o primeiro dígito verificador
            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(cpf[i].ToString()) * multiplicadores1[i];
            }
            int resto = soma % 11;
            int primeiroDigitoVerificador = resto < 2 ? 0 : 11 - resto;

            // Calcula o segundo dígito verificador
            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(cpf[i].ToString()) * multiplicadores2[i];
            }
            resto = soma % 11;
            int segundoDigitoVerificador = resto < 2 ? 0 : 11 - resto;

            // Verifica se os dígitos verificadores calculados conferem com os dígitos do CPF
            return cpf.EndsWith($"{primeiroDigitoVerificador}{segundoDigitoVerificador}");
        }

        public string MascaraCPF(string cpf)
        {
            cpf = Regex.Replace(cpf, @"\D", "");
            cpf = Regex.Replace(cpf, @"(\d{3})(\d{3})(\d{3})(\d{2})", "$1.$2.$3-$4");
            return cpf;
        }
    }
}
