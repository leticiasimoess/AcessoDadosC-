/*
 * Criado por SharpDevelop.
 * Usuário: macoratti
 * Data: 07/30/2008
 * Hora: 11:27
 * 
 * Para alterar este modelo use Ferramentas | Opções | Codificação | Editar Cabeçalhos Padrão.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;

namespace AcessoDadosC_
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			//define a string de conexao com provedor caminho e nome do banco de dados
			string strProvider = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\\dados\\Cadastro.mdb";
			//define a instrução SQL
			string strSql = "SELECT * FROM Clientes";

			//cria a conexão com o banco de dados
			OleDbConnection con = new OleDbConnection(strProvider);
			//cria o objeto command para executar a instruçao sql
			OleDbCommand cmd = new OleDbCommand(strSql, con);

			//abre a conexao
			con.Open();

			//define o tipo do comando 
			cmd.CommandType = CommandType.Text;
			//cria um dataadapter
			OleDbDataAdapter da = new OleDbDataAdapter(cmd);

			//cria um objeto datatable
			DataTable clientes = new DataTable();

			//preenche o datatable via dataadapter
			da.Fill(clientes);

			//atribui o datatable ao datagridview para exibir o resultado
			dataGridView1.DataSource = clientes;
			
			con.Close();
		
		}
		
	
		void Button2Click(object sender, EventArgs e)
		{
			//define a string de conexao com provedor caminho e nome do banco de dados
			string strProvider = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c:\\dados\\Cadastro.mdb";
			//define a instrução SQL
			string strSql = "SELECT * FROM Clientes";

			//cria a conexão com o banco de dados
			OleDbConnection con = new OleDbConnection(strProvider);
			//cria o objeto command para executar a instruçao sql
			OleDbCommand cmd = new OleDbCommand(strSql, con);

			//abre a conexao
			con.Open();

			//define o tipo do comando 
			cmd.CommandType = CommandType.Text;
			
			//obtem um datareader
			OleDbDataReader dr = cmd.ExecuteReader();
			
			int nColunas = dr.FieldCount;
			
			for (int i=0; i < nColunas; i++)
			{
				dataGridView1.Columns.Add(dr.GetName(i).ToString() ,dr.GetName(i).ToString());
			}
			
            //define um array de strings com nCOlunas
            string[] linhaDados = new string[nColunas];
				
            //percorre o DataRead
			while (dr.Read())
			{
				//percorre cada uma das colunas
				for (int a =0 ; a < nColunas; a++)
				{
					//verifica o tipo de dados da coluna
					if (dr.GetFieldType(a).ToString() == "System.Int32")
					{
						linhaDados[a] = dr.GetInt32(a).ToString();
					}
					if (dr.GetFieldType(a).ToString() == "System.String")
					{
						linhaDados[a] = dr.GetString(a).ToString();
					}
					if (dr.GetFieldType(a).ToString() == "System.DateTime")
					{
						linhaDados[a] = dr.GetDateTime(a).ToString();
					}
				}
				//atribui a linha ao datagridview
				dataGridView1.Rows.Add(linhaDados);
			}
				
		}//fim 
	}
}
