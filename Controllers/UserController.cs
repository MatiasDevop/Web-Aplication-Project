using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVClog.Models;
using MVClog.Models.ViewModels;
using MVClog.Membership;

namespace MVClog.Controllers
{
    public class UserController : Controller
    {
        private Context db = new Context();

        //
        // GET: /User/

        public ActionResult Index()
        {
            return View(db.Users.Include(r=>r.Roles).ToList());
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            //guardar todos lo roles en una variable solo para mostrar en el get
            var allRoles = db.Roles;


            //mostrar los tres atributos del viewmodel
            var viewModel = new List<AssignedRoleData>();


            //iterar cada rol y cargarlo en un viewmodel
            foreach (var role in allRoles)
            {

                //viewModel captura los datos del Rol
                viewModel.Add(new AssignedRoleData
                {
                    RoleID = role.RoleID,
                    Name = role.RoleName,
                    Assigned = false,
                });
            }

            //cargar todo los viewModel en un viewbag pequeño contenedor
            ViewBag.Roles = viewModel;

            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(User user, string[] selectedRoles)
        {

            //buscar un usuario en mi base de datos
            User myUser = db.Users.Where(u => u.UserName.ToUpper() == user.UserName.ToUpper()).FirstOrDefault();



            if (myUser == null)
            {
                if (!String.IsNullOrEmpty(user.UserName) && !String.IsNullOrEmpty(user.Password) && !String.IsNullOrEmpty(user.Name) && !String.IsNullOrEmpty(user.LastName))
                {

                    //asignar variables no insertadas por el usuario
                    user.Password = Crypto.HashPassword(user.Password);
                    user.CreateDate = DateTime.Now;
                    user.IsApproved = true;
                    db.Users.Add(user);
                    db.SaveChanges();

                    //crear variable para actualizar roles dentro del bd
                    //ingresa a la tabla muchos a muchos userRoles
                    var userUpdate = db.Users.Include("Roles").Where(i => i.UserID == user.UserID).Single();

                    try
                    {
                        UpdateUserRoles(selectedRoles, userUpdate);

                        //sirve para modifica o actualizar la base de datos
                        db.Entry(userUpdate).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (DataException)
                    {

                        ModelState.AddModelError("", "Error");
                    }


                    return RedirectToAction("Index");
                }
            }


            var allRoles = db.Roles;
            var viewModel = new List<AssignedRoleData>();
            foreach (var role in allRoles)
            {
                viewModel.Add(new AssignedRoleData
                {
                    RoleID = role.RoleID,
                    Name = role.RoleName,
                    Assigned = false,
                });
            }
            ViewBag.Roles = viewModel;

            return View(user);
        }


        private string UpdateUserRoles(string[] selectedRoles, User userToUpdate)
        {
            string newPass = null;


            if (selectedRoles == null)
            {
                userToUpdate.Roles = new List<Role>();


                return newPass;
            }

            //ingresar a la tabla userroles
            var selectedRolesHS = new HashSet<string>(selectedRoles);

            //codigo de los roles , que roles tiene el usuario
            var userRoles = new HashSet<int>(userToUpdate.Roles.Select(c => c.RoleID));

            //iterar en los roles
            foreach (var role in db.Roles)
            {
                //verificamos si el rol lo tiene el usuario
                if (selectedRolesHS.Contains(role.RoleID.ToString()))
                {
                    if (!userRoles.Contains(role.RoleID))
                    {
                        //agregamos el rol del usuario
                        userToUpdate.Roles.Add(role);
                    }
                }
                else
                {
                    if (userRoles.Contains(role.RoleID))
                    {
                        //eliminamos el rol que ha sido eliminado o quitado a ese usuario
                        userToUpdate.Roles.Remove(role);
                    }
                }
            }


            return newPass;

        }





        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}