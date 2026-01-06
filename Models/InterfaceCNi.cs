using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace CNIWebApp.Models
{
    public interface InterfaceCNI
    {
        // 1. Retourne les informations simplifiées pour l'API
        List<ApiCni> ListInfo();

        // 2. Calcule la validité (différent selon le type de carte)
        bool IsValid();

        // 3. Formate le numéro de série selon les standards du pays
        string FormatSerialNumber();
    }
}
