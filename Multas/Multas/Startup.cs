using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Multas.Models;
using Owin;

namespace Multas
{
    public partial class Startup
    {
        //este método é executado apenas uma única vez, no inicio do funcionamento da aplicação 
        public void Configuration(IAppBuilder app)
        {

            ConfigureAuth(app);
            //inicia a configuração da aplicação 
            iniciaAplicacao();
        }


        /// <summary>
        /// cria, caso não existam, as Roles de suporte à aplicação: Agente, Funcionario, Condutor
        /// cria, nesse caso, também, um utilizador...
        /// </summary>
        private void iniciaAplicacao()
        {

            // identifica a base de dados de serviço à aplicação
            ApplicationDbContext db = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            // criar a Role 'Agente'
            if (!roleManager.RoleExists("Agente"))
            {
                // não existe a 'role'
                // então, criar essa role
                var role = new IdentityRole();
                role.Name = "Agente";
                roleManager.Create(role);
            }
            // criar a Role 'GestorMultas'
            if (!roleManager.RoleExists("GestorMultas"))
            {
                // não existe a 'role'
                // então, criar essa role
                var role = new IdentityRole();
                role.Name = "GestorMultas";
                roleManager.Create(role);
            }

            // criar a Role 'RecursosHumanos'
            if (!roleManager.RoleExists("RecursosHumanos"))
            {
                // não existe a 'role'
                // então, criar essa role
                var role = new IdentityRole();
                role.Name = "RecursosHumanos";
                roleManager.Create(role);
            }



            // criar um utilizador 'Agente'
            var user = new ApplicationUser();
            user.UserName = "agente@multas.pt";
            user.Email = "agente@multas.pt";
            //user.Nome = "Luís Freitas";
            string userPWD = "123_Asd";
            var chkUser = userManager.Create(user, userPWD);

            //Adicionar o Utilizador à respetiva Role-Agente-
            if (chkUser.Succeeded)
            {
                var result1 = userManager.AddToRole(user.Id, "Agente");
            }
        }

    }
}
