namespace CNIWebApp.Models
{
    public class ApiCni : InterfaceCNI
    {
        public string Num_cni { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }

        public string Type { get; set; }

        public List<ApiCni> ListInfo()
        {
            var listeCNI = new List<ApiCni>();

            listeCNI.Add(new ApiCni
            {
                Nom = this.Nom,
                Prenom = this.Prenom,
                Num_cni = this.Num_cni,
                Type = "Carte Nationale Standard"
            });

            return listeCNI;
        }

        public bool IsValid() => true;

        public string FormatSerialNumber() => "DIP-" + this.Num_cni;
    }
}
