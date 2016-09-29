//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FaculdadeSI.Models
{
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
    
    public class Perfil
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Perfil()
        {
            this.Avaliacaos = new HashSet<Avaliacao>();
            this.Usuarios = new HashSet<Usuario>();
        }
    
        public int IdPerfil { get; set; }
        public string DescricaoPerfil { get; set; }
        public Nullable<bool> PerfilStatus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Avaliacao> Avaliacaos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Usuario> Usuarios { get; set; }

        private ReviewEntities db = new ReviewEntities();



        public Perfil GetDetailsPerfil(int id)
        {
            //Perfil perfil = new Perfil();
            if (id == null)
            {
                throw new KeyNotFoundException();
            }
            Perfil perfil = db.Perfils.Find(id);
           
            if (perfil == null)
            {
                throw new KeyNotFoundException();
            }

            return perfil;
        }
    }
}