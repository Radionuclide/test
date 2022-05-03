using System;
using System.Collections.Generic;
using System.Text;

namespace XmlExtract
{
    /// <summary>
    /// Gueltige TKS-Standorte. BO, DO, DU, SI, SA oder Anderer
    /// Entspricht dem StandortType aus dem XSD, erweitert um "Anderer"
    /// </summary>
    public enum StandortType
    {
        BO, //Bochum
        DO, //Dortmund 
        DU, //Duisburg
        SI, //Siegerland
        SA, //Galmed (Spanien)
        Anderer,
    }
}
