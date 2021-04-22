using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WhatsBot
{
    public partial class Form1 : Form
    {
        public ChromeOptions options;
        public ChromeDriver driver;
        public string webWhats ="https://web.whatsapp.com/";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            string path = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
            options = new ChromeOptions();
            options.AddArgument("user-data-dir="+path+"ChromeDriver\\Cache");
            driver = new ChromeDriver(path, options);
            AcessarUrl(webWhats);
        }

        public void AcessarUrl(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        public string DefinirContato()
        {
            string nomeContato = txtContato.Text;

            return nomeContato;
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            string nome = DefinirContato().ToString();
            string mensagem = rtxMensagem.Text.ToString();

            // //span[@title=""]
            IWebElement listaDeContatos = driver.FindElement(By.XPath("//div[@class='JnmQF _3QmOg']"));
            List<IWebElement> contatos =listaDeContatos.FindElements(By.XPath("//span[@title]")).ToList();
            
            
            foreach (IWebElement contato in contatos)
            {
                if (contato.Text == nome)
                {
                    contato.Click();
                    break;
                }
              
            }
            Thread.Sleep(6000);
            IWebElement campoMensagem = driver.FindElement(By.XPath("//div[@class='_2A8P4']"));

            campoMensagem.Click();
            Thread.Sleep(4000);
            campoMensagem.SendKeys(mensagem);
            campoMensagem.SendKeys(OpenQA.Selenium.Keys.Enter);


            driver.Quit();
            
        }
    }
}
