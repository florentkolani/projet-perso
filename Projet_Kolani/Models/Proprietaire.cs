namespace Projet_Kolani.Models
{
    public class Proprietaire
    {
       
            public int ProprietaireId { get; set; }
            public string Nom { get; set; }
            public string Prenom { get; set; }

            public ICollection<Engin> Engins { get; set; } = new List<Engin>();


    }
}
