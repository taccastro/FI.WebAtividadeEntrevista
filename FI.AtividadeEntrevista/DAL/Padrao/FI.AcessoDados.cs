﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FI.AtividadeEntrevista.DAL
{
    internal class AcessoDados
    {
        private string stringDeConexao
        {
            get
            {
                ConnectionStringSettings conn = System.Configuration.ConfigurationManager.ConnectionStrings["BancoDeDados"];
                if (conn != null)
                    return conn.ConnectionString;
                else
                    return string.Empty;
            }
        }

        internal void Executar(string NomeProcedure, List<SqlParameter> parametros)
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(stringDeConexao);
            comando.Connection = conexao;
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            comando.CommandText = NomeProcedure;
            foreach (var item in parametros)
                comando.Parameters.Add(item);

            conexao.Open();
            try
            {
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conexao.Close();
            }
        }

        //    internal DataSet Consultar(string NomeProcedure, List<SqlParameter> parametros)
        //    {
        //        SqlCommand comando = new SqlCommand();
        //        SqlConnection conexao = new SqlConnection(stringDeConexao);

        //        comando.Connection = conexao;
        //        comando.CommandType = System.Data.CommandType.StoredProcedure;
        //        comando.CommandText = NomeProcedure;
        //        foreach (var item in parametros)
        //            comando.Parameters.Add(item);

        //        SqlDataAdapter adapter = new SqlDataAdapter(comando);
        //        DataSet ds = new DataSet();
        //        conexao.Open();

        //        try
        //        {
        //            adapter.Fill(ds);
        //        }
        //        catch (Exception ex)
        //        {

        //            throw ex;
        //        }
        //        finally
        //        {
        //            conexao.Close();
        //        }

        //        return ds;
        //    }

        internal DataSet Consultar(string NomeProcedure, List<SqlParameter> parametros)
        {
            SqlCommand comando = new SqlCommand();
            SqlConnection conexao = new SqlConnection(stringDeConexao);

            comando.Connection = conexao;
            comando.CommandType = System.Data.CommandType.StoredProcedure;
            comando.CommandText = NomeProcedure;

            // Verificação se os parâmetros estão presentes
            if (parametros != null)
            {
                foreach (var item in parametros)
                {
                    if (item != null) // Verifica se o parâmetro não é nulo
                        comando.Parameters.Add(item);
                }
            }

            SqlDataAdapter adapter = new SqlDataAdapter(comando);
            DataSet ds = new DataSet();
            conexao.Open();

            try
            {
                adapter.Fill(ds);
            }
            catch (Exception ex)
            {
                // Registra o erro ou apenas relança a exceção
                throw new Exception("Erro ao executar a procedure.", ex);
            }
            finally
            {
                conexao.Close();
            }

            return ds;
        }


    }
}
