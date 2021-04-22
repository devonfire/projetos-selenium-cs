using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RPA_ConsultorDeCEP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConsulta_Click(object sender, EventArgs e)
        {
            string url = "https://buscacepinter.correios.com.br/app/endereco/index.php";
            string cep = txtCEP.Text;
            string resultado = "";
            Regex padrao;

            #region Setup
            string path = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
      
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("user-data-dir=" + path + "ChromeDriver\\Cache");

            ChromeDriver driver = new ChromeDriver(path, options);
            driver.Navigate().GoToUrl(url);
            #endregion

            #region Elements
            IWebElement campoDeBuscaDeCEP = driver.FindElement(By.XPath("//input[@id='endereco']"));
            IWebElement botaoBuscar = driver.FindElement(By.XPath("//button[@id='btn_pesquisar']"));
            List<IWebElement> tabelaResultante = driver.FindElements(By.XPath("//table[@id='resultado-DNEC']")).ToList();
            #endregion

            #region Actions
            InserirCEP();
            ClicarNoBotaoBuscar();
            EsperarResultadoDaBusca();
            MostrarResultadosObtidos();
            #endregion

            void InserirCEP()
            {
                campoDeBuscaDeCEP.Click();
                campoDeBuscaDeCEP.SendKeys(cep.Replace("-", ""));
            }
            void ClicarNoBotaoBuscar() => botaoBuscar.Click();
            void EsperarResultadoDaBusca()
            {
                IWebElement telaResultanteDaConsulta = (new WebDriverWait(driver, TimeSpan.FromSeconds(50)).Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='resultado']"))));       
            }
            void MostrarResultadosObtidos()
            {

                padrao = new Regex("(Logradouro/Nome\\.?|Bairro/Distrito\\.?|Localidade/UF\\.?|CEP\\.?)");
                
                foreach (IWebElement item in tabelaResultante)
                {

                    resultado = resultado + "Resultado da Busca: \n\n" + padrao.Replace(item.Text,"");//
                }
                lblResultado.Text=resultado;
                
            }

         //driver.Quit();
            

        }
    }
}
