using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multas.Models;

namespace Multas.Controllers
{
    public class AgentesController : Controller
    {
        //cria uma variavel que representa a base de dados
        private MultasDB db = new MultasDB();

        // GET: Agentes
        public ActionResult Index()
        {
            //procura a totalidade dos agente na base de dados 
            //Intrução feita em LINQ
            //SELECT * from Agentes ORDER BY name
            var listaAgentes = db.Agentes.OrderBy(a=>a.Nome).ToList();
            return View(listaAgentes);
        }

        // GET: Agentes/Details/5
        /// <summary>
        /// Mostra os dados de um Agente 
        /// </summary>
        /// <param name="id">identifica o Agente</param>
        /// <returns>Devolve a View com os dados</returns>
        /// int? - pode nao ser preenchido
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //resposta por defeito
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //este erro ocorre pois o utilizador anda a "fazer asneiras"
                return RedirectToAction("Index");

            }
            //SELECT * FROM Agentes WHERE ID=id
            Agentes agente = db.Agentes.Find(id);
            //o Agente foi encontrado?
            if (agente == null)
            {
                //O Agente não foi encontrado, porque o utilizador está 'à pesca'
                return RedirectToAction("Index");
            }
            return View(agente);
        }

        // GET: Agentes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agentes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// criação de um novo agente
        /// </summary>
        /// <param name="agente"> recolhe os dados do Nome e da Esquadra do Agente</param>
        /// <param name="fotografia">representa a fotografia que identifica o Agente</param>
        /// <returns>devolve uma View</returns>
        [HttpPost]
        //anotação para proteger o código de ataques 
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,Esquadra")] Agentes agente,
                                          HttpPostedFileBase fotografia)
        {
            //precisamos de processar a fotografia
            //1º será que foi fornecido um ficheiro?
            //2º será do tipo correto (jpg, png)?
            //3º se for do tipo correto, guarda-se 
            //      se não for, atribui-se um 'avatar genérico' ao utilizador

            //var auxiliar 
            string caminho = "";
            bool haFicheiro = false;
            //há ficheiro?
            if (fotografia == null)
            {
                //não ha ficheiro
                //atribui-se-lhe o avatar 
                agente.Fotografia = "noUser.jpg";
            }
            else {
                //há ficheiro
                //é correto?
                if (fotografia.ContentType == "image/jpeg" || fotografia.ContentType == "image/png") {
                    //estamos perante uma fotografia correta 
                    string extensao = Path.GetExtension(fotografia.FileName).ToLower();
                    Guid g;
                    g = Guid.NewGuid();
                    //nome do ficheiro
                    string nome = g.ToString() + extensao;
                    //onde guardar o ficheiro 
                    //MapPath vai a raiz do projeto e avança até a pasta das imagens
                    //Combine - junta a string com o nome do ficheiro
                     caminho = Path.Combine(Server.MapPath("~/imagens"), nome);

                    //atribuir ao agente o nome do ficheiro
                    agente.Fotografia = nome;

                    //assinalar que ha fotografia
                    haFicheiro = true;
                }
            }   
            //verifica se os dados fornecidos estão de acordo com as regras definidas no Modelo
            if (ModelState.IsValid)
            {
                //sempre que se interage com a base de dados é necessário colocar um try/catch
                try
                {
                    //adição de um novo agente
                    db.Agentes.Add(agente);
                    //"commit" - consolida os dados na BD
                    db.SaveChanges();
                    //vou guardar o ficheiro 
                    if(haFicheiro)
                    fotografia.SaveAs(caminho);
                
                    //devolve o controlo para a página do index
                    return RedirectToAction("Index");

                }
                catch (Exception) {
                    ModelState.AddModelError("", "Ocorreu um erro com a escrita de dados do novo Agente");
                }
                
            }
            //caso nao seja válido
            return View(agente);
        }

        // GET: Agentes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Agentes agente = db.Agentes.Find(id);
            if (agente == null)
            {
                return RedirectToAction("Index");
            }
            return View(agente);
        }

        // POST: Agentes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Esquadra,Fotografia")] Agentes agente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agente);
        }

        // GET: Agentes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Agentes agente = db.Agentes.Find(id);
            if (agente == null)
            {
                return RedirectToAction("Index");
            }
            //o agente foi encontrado 
            //vou salvaguardar os dados para posterior validação
            //- guardar o ID do Agente num cookie cifrado - vai para o cliente 
            //- guardar o ID numa variável de sessão - uma variável de sessão nunca vai para o cliente fica sempre no servidor
            //    (se se usar o ASP .NET Core, esta ferramenta já não existe
            //- outras alternativas válidas 
            Session["Agente"] = agente.ID;
            //mostra na view os dados do agente
            return View(agente);
        }

        // POST: Agentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null) {
                return RedirectToAction("Index");
            }
            //o ID não é nulo 
            //Será o ID o que eu espero?
            //vamos validar se o ID está correto
            if (id != (int) Session["Agente"]) {
                return RedirectToAction("Index");
            }



            //procura o agente a remover
            Agentes agente = db.Agentes.Find(id);

            if (agente == null) {
                //não foi encontrado o agente 
                return RedirectToAction("Index");
            }
            try
            {
               
                //dá ordem de remoção do agente
                db.Agentes.Remove(agente);
                //consolida a remoção
                db.SaveChanges();
            }
            catch (Exception)
            {
                //devem aqui ser escritas todas as instruções que são consideradas necessárias 

                //informar que houve um erro 
                ModelState.AddModelError("", "Não é possível remover o Agente. Provavelmente, o  agente, "+ agente.Nome +", tem multas associadas a ele...");

                //redirecionar para a página onde o erro foi gerado
                return View(agente);
            }
            

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
