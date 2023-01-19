/*
 * Criado por SharpDevelop.
 * Usuário: macoratti
 * Data: 07/30/2008
 * Hora: 11:28
 * 
 * Para alterar este modelo use Ferramentas | Opções | Codificação | Editar Cabeçalhos Padrão.
 */

using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace AcessoDadosC_
{
	/// <summary>
	/// AcessoBD.
	/// </summary>
	/// 
   /// <summary>
    /// ADO.NET - acesso a dados usando o provedor gerenciado para SQL Server
    /// </summary>
    public class Database : IDisposable
    {
        // conexão com o banco de dados
        private SqlConnection con;
        
        /// <summary>
        /// Executa uma stored procedure.
        /// </summary>
        /// <param name="procName">Nome da stored procedure.</param>
        /// <returns>Valor retornada pela Stored procedure.</returns>
        public int RunProc(string procName)
        {
            SqlCommand cmd = CreateCommand(procName, null);
            cmd.ExecuteNonQuery();
            this.Close();
            return (int)cmd.Parameters["ReturnValue"].Value;
        }

        /// <summary>
        /// Executa uma stored procedure.
        /// </summary>
        /// <param name="procName">Nome da stored procedure.</param>
        /// <param name="prams">parametros da Stored.</param>
        /// <returns>valor retornado pela Stored procedure.</returns>
        public int RunProc(string procName, SqlParameter[] prams)
        {
            SqlCommand cmd = CreateCommand(procName, prams);
            cmd.ExecuteNonQuery();
            this.Close();
            return (int)cmd.Parameters["ReturnValue"].Value;
        }

        /// <summary>
        /// Executa uma stored procedure.
        /// </summary>
        /// <param name="procName">Nome da stored procedure.</param>
        /// <param name="dataReader">Resultado da procedure.</param>
        public void RunProc(string procName, out SqlDataReader dataReader)
        {
            SqlCommand cmd = CreateCommand(procName, null);
            dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Executa uma stored procedure.
        /// </summary>
        /// <param name="procName">Nome da stored procedure.</param>
        /// <param name="prams">Parametros da Stored procedure.</param>
        /// <param name="dataReader">Resultado da procedure.</param>
        public void RunProc(string procName, SqlParameter[] prams, out SqlDataReader dataReader)
        {
            SqlCommand cmd = CreateCommand(procName, prams);
            dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Cria um objeto command usado para chamar uma stored procedure
        /// </summary>
        /// <param name="procName">Nome da stored procedure.</param>
        /// <param name="prams">Parametros da stored procedure.</param>
        /// <returns>Command object.</returns>
        private SqlCommand CreateCommand(string procName, SqlParameter[] prams)
        {
            // abre a conexao
            Open();

            
            SqlCommand cmd = new SqlCommand(procName, con);
            cmd.CommandType = CommandType.StoredProcedure;

            // adiciona os parametros
            if (prams != null)
            {
                foreach (SqlParameter parameter in prams)
                    cmd.Parameters.Add(parameter);
            }
            
            // retorna os parametros
            cmd.Parameters.Add(
                new SqlParameter("ReturnValue", SqlDbType.Int, 4,
                ParameterDirection.ReturnValue, false, 0, 0,
                string.Empty, DataRowVersion.Default, null));

            return cmd;
        }

        /// <summary>
        /// Abra a conexao
        /// </summary>
        private void Open()
        {
            // Abra a conexao
            if (con == null)
            {
                //con = new SqlConnection(ConfigurationSettings.AppSettings["ConnectionString"]);
                con = new SqlConnection(ConfigurationSettings.AppSettings["ConnectionString"]);
                con.Open();
            }                
        }

        /// <summary>
        /// Fecha a conexao
        /// </summary>
        public void Close()
        {
            if (con != null)
                con.Close();
        }

        /// <summary>
        /// Libera recursos.
        /// </summary>
        public void Dispose()
        {
            // verifica se conexao esta fechada
            if (con != null)
            {
                con.Dispose();
                con = null;
            }                
        }

        /// <summary>
        /// Cria um parametro de entrada
        /// </summary>
        /// <param name="ParamName">Nome do parametro.</param>
        /// <param name="DbType">Tipo do Parametro.</param>
        /// <param name="Size">Tamanho do Parametro.</param>
        /// <param name="Value">Valor do Parametero.</param>
        /// <returns>Novo parametro.</returns>
        public SqlParameter MakeInParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }        

        /// <summary>
        ///  Cria um parametro de saida
        /// </summary>
        /// <param name="ParamName">Nome do parametro.</param>
        /// <param name="DbType">Tipo do Parametro.</param>
        /// <param name="Size">Tamanho do Parametro.</param>
        /// <returns>Novo parametro.</returns>
        public SqlParameter MakeOutParam(string ParamName, SqlDbType DbType, int Size)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }        

        /// <summary>
        /// Cria um parametro para stored procedure
        /// </summary>
        /// <param name="ParamName">Nome do parametro.</param>
        /// <param name="DbType">Tipo do Parametro.</param>
        /// <param name="Size">Tamanho do Parametro.</param>
        /// <param name="Direction">Direção do Parametro.</param>
        /// <param name="Value">Valor do Parametero.</param>
        /// <returns>Novo parametro.</returns>
        public SqlParameter MakeParam(string ParamName, SqlDbType DbType, Int32 Size,
         ParameterDirection Direction, object Value)
        {
            SqlParameter param;

            if(Size > 0)
                param = new SqlParameter(ParamName, DbType, Size);
            else
                param = new SqlParameter(ParamName, DbType);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
                param.Value = Value;

            return param;
        }
        
        /// <summary>
        ///  Obtem um Datatable
        /// </summary>
        /// <param name="Sql">A instrucao SQL</param>
        /// <param name="ConnectString">A string de conexao</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string Sql,string ConnectString)
        {
            SqlDataAdapter da = new SqlDataAdapter(Sql,ConnectString);
            DataTable dt = new DataTable();

            int rows = da.Fill(dt);
                        
            return dt;
        }
    }

}
