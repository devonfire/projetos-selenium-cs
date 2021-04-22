using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using System.Windows.Forms;

namespace GeradorDePessoaFicticia
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            rtbResultado.Visible = false;
            lblResultado.Visible = false;
            dataGridView.Visible = false;
        }

        ChromeOptions options;
        ChromeDriver driver;

        string url = "https://www.4devs.com.br/";
        string path = System.AppDomain.CurrentDomain.BaseDirectory.ToString();



        private void Form1_Load(object sender, EventArgs e)
        { }

        private void btnGerar_Click(object sender, EventArgs e)
        {
            if (cbSexo.Text == string.Empty)
            {
                MessageBox.Show("Selecione um tipo de sexo", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (nIdade.Value == 0 || nIdade.Value < 0)
            {
                MessageBox.Show("Informe a idade", "Oops!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (nIdade.Value <= 17)
            {
                MessageBox.Show("Pessoa deve ser maior de idade", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {


                options = new ChromeOptions();
                options.AddArgument("user-data-dir=" + path + "ChromeDriver\\Cache");
                driver = new ChromeDriver(path, options);


                driver.Navigate().GoToUrl(url);
                driver.Manage().Window.Maximize();

                IWebElement ferramentaGeradorPessoas = driver.FindElement(By.XPath("//div//span[contains(text(),'Gerador de Documentos de Pessoas')]"));
                ferramentaGeradorPessoas.Click();

                switch (cbSexo.Text)
                {
                    case "Masculino":
                        driver.FindElement(By.Id("sexo_homem")).Click();
                        break;
                    case "Feminino":
                        driver.FindElement(By.Id("sexo_mulher")).Click();
                        break;
                    default:
                        driver.FindElement(By.Id("sexo_indiferente")).Click();
                        break;
                }
                /**/
                SelectElement oSelectOrdem = new SelectElement(driver.FindElement(By.Id("idade")));
                oSelectOrdem.SelectByText(nIdade.Value.ToString());

                driver.FindElement(By.Id("bt_gerar_pessoa")).Click();

                Thread.Sleep(5000);

                //Clica na aba Json
                driver.FindElement(By.Id("btn_json_tab")).Submit();
                var json = driver.FindElement(By.Id("dados_json")).Text;

                rtbResultado.Text = json;
                rtbResultado.Visible = true;
                lblResultado.Visible = true;

                driver.FindElement(By.Id("btn_form_tab")).Click();

                /**/
                //Recupera do input do nome
                var nomeCliente = driver.FindElement(By.Id("nome")).FindElement(By.TagName("span")).Text;

                //Recupera do input do cpf
                var cpf = driver.FindElement(By.Id("cpf")).FindElement(By.TagName("span")).Text;

                //Recupera do input do email
                var email = driver.FindElement(By.Id("email")).FindElement(By.TagName("span")).Text;
                dataGridView.Visible = true;



                dataGridView.Columns.Add("Pessoa", "Nome");
                dataGridView.Columns.Add("Pessoa", "CPF");
                dataGridView.Columns.Add("Pessoa", "Email");

                dataGridView.Rows.Add();

                dataGridView.Rows[0].Cells[0].Value = nomeCliente;
                dataGridView.Rows[0].Cells[1].Value = cpf;
                dataGridView.Rows[0].Cells[2].Value = email;

            }
            catch (Exception ex)
            {

                MessageBox.Show($"Erro do tipo: {ex}", "Algo Deu Errado!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            finally
            {
                driver.Quit();
                MessageBox.Show("Processo Finalizado!👍");
            }
        }

    }
}
