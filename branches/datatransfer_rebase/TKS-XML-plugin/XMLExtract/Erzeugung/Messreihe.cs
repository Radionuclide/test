// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code. Version 3.6.0.0
//    <NameSpace>XmlExtract</NameSpace><Collection>List</Collection><codeType>CSharp</codeType><EnableDataBinding>False</EnableDataBinding><EnableLazyLoading>False</EnableLazyLoading><TrackingChangesEnable>False</TrackingChangesEnable><GenTrackingClasses>False</GenTrackingClasses><HidePrivateFieldInIDE>False</HidePrivateFieldInIDE><EnableSummaryComment>True</EnableSummaryComment><VirtualProp>False</VirtualProp><PascalCase>False</PascalCase><BaseClassName>EntityBase</BaseClassName><IncludeSerializeMethod>False</IncludeSerializeMethod><UseBaseClass>False</UseBaseClass><GenBaseClass>False</GenBaseClass><GenerateCloneMethod>False</GenerateCloneMethod><GenerateDataContracts>False</GenerateDataContracts><CodeBaseTag>Net35</CodeBaseTag><SerializeMethodName>Serialize</SerializeMethodName><DeserializeMethodName>Deserialize</DeserializeMethodName><SaveToFileMethodName>SaveToFile</SaveToFileMethodName><LoadFromFileMethodName>LoadFromFile</LoadFromFileMethodName><GenerateXMLAttributes>True</GenerateXMLAttributes><OrderXMLAttrib>False</OrderXMLAttrib><EnableEncoding>False</EnableEncoding><AutomaticProperties>True</AutomaticProperties><GenerateShouldSerialize>False</GenerateShouldSerialize><DisableDebug>True</DisableDebug><PropNameSpecified>Default</PropNameSpecified><Encoder>UTF8</Encoder><CustomUsings>System.Xml</CustomUsings><ExcludeIncludedTypes>False</ExcludeIncludedTypes><InitializeFields>All</InitializeFields><GenerateAllTypes>True</GenerateAllTypes>
//  </auto-generated>
// ------------------------------------------------------------------------------
namespace XmlExtract
{
    using System;
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System.Collections;
    using System.Xml.Schema;
    using System.ComponentModel;
    using System.Xml;
    using System.Collections.Generic;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.6.0.20097")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.thyssen.com/xml/schema/qbic")]
    [System.Xml.Serialization.XmlRootAttribute("Erzeugung", Namespace="http://www.thyssen.com/xml/schema/qbic", IsNullable=false)]
    public partial class ErzeugungType
    {
        [System.Xml.Serialization.XmlAttributeAttribute("schemaLocation", Namespace = System.Xml.Schema.XmlSchema.InstanceNamespace)]
        public string xsiSchemaLocation = "http://www.thyssen.com/xml/schema/qbic http://www-eai/schema/qbic/Messung/REL-2_6_1/Messreihe.xsd";

        public MaterialHeaderType MaterialHeader { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("Messung")]
        public List<MessungType> Messung { get; set; }
        public EinzelwertComplexType Einzelwerte { get; set; }
        
        /// <summary>
        /// ErzeugungType class constructor
        /// </summary>
        public ErzeugungType()
        {
            this.Einzelwerte = new EinzelwertComplexType();
            this.Messung = new List<MessungType>();
            this.MaterialHeader = new MaterialHeaderType();
        }

        public bool ShouldSerializeEinzelwerte()
        {
            return (Einzelwerte.Einzelwert.Count > 0);
        }

    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.6.0.20097")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.thyssen.com/xml/schema/qbic")]
    public partial class MaterialHeaderType
    {
        
        private System.Nullable<MaterialArtType> materialArtField;
        

        public string LokalerIdent { get; set; }

        public string TKSIdent { get; set; }
        
        /// <summary>
        /// BO, DO, DU, SI oder Anderer
        /// </summary>
        public string Standort { get; set; }
        
        /// <summary>
        /// Nur DU:  BR WB KB VZ BB PK
        /// </summary>
        public MaterialArtType MaterialArt
        {
            get
            {
                if (this.materialArtField.HasValue)
                {
                    return this.materialArtField.Value;
                }
                else
                {
                    return default(MaterialArtType);
                }
            }
            set
            {
                this.materialArtField = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MaterialArtSpecified
        {
            get
            {
                return this.materialArtField.HasValue;
            }
            set
            {
                if (value==false)
                {
                    this.materialArtField = null;
                }
            }
        }
    }
    
    /// <summary>
    /// Zusaetzliche Kennzeichnung von Duisburger Material.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.6.0.20097")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.thyssen.com/xml/schema/qbic")]
    public enum MaterialArtType
    {
        
        /// <summary>
        /// Bramme
        /// </summary>
        BR,
        
        /// <summary>
        /// Warmband
        /// </summary>
        WB,
        
        /// <summary>
        /// Kaltband
        /// </summary>
        KB,
        
        /// <summary>
        /// verzinktes Material
        /// </summary>
        VZ,
        
        /// <summary>
        /// bandbeschichtetes Material
        /// </summary>
        BB,
        
        /// <summary>
        /// Paket (Material zum Versand)
        /// </summary>
        PK,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.6.0.20097")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.thyssen.com/xml/schema/qbic")]
    public partial class EinzelwertType
    {
        
        /// <summary>
        /// z.B. TemperaturZinkbad
        /// </summary>
        public string Bezeichner { get; set; }
        public string Wert { get; set; }

        public string Einheit { get; set; }

        public string EinheitLokal { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.6.0.20097")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.thyssen.com/xml/schema/qbic")]
    public enum EinheitEnum
    {
        
        mm,
        
        mikrom,
        
        gradC,
        
        /// <summary>
        /// m/min
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("m/min")]
        mmin,
        
        kW,
        
        A,
        
        V,

        /// <summary>
        /// g/qm
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("g/qm")]
        gqm,
        
        Prozent,
        
        KN,
        
        mbar,
        
        grad,

        /// <summary>
        /// Keine/1
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("Keine/1")]
        Keine1,
        
        min,
        
        m,
        
        Test,
        
        /// <summary>
        /// As/qm
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("As/qm")]
        Asqm,
        
        /// <summary>
        /// m/qs
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("m/qs")]
        mqs,
        
        /// <summary>
        /// U/min
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("U/min")]
        Umin,
        
        MPa,
        
        KNm,
        
        Hz,
        
        /// <summary>
        /// gradC/s
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("gradC/s")]
        gradCs,
        
        /// <summary>
        /// m3/s
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("m3/s")]
        m3s,
        
        /// <summary>
        /// m3/h
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("m3/h")]
        m3h,
        
        /// <summary>
        /// t/h
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("t/h")]
        th,
        
        /// <summary>
        /// mikroS/cm
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("mikroS/cm")]
        mikroScm,
        
        /// <summary>
        /// N/m2
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("N/m2")]
        Nm2,
        
        /// <summary>
        /// m/s
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("m/s")]
        ms,
        
        /// <summary>
        /// A/qm
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("A/qm")]
        Aqm,
        
        /// <summary>
        /// cm-1
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("cm-1")]
        cm1,
        
        s,
        
        mikroS,
        
        m3,
        
        /// <summary>
        /// ml/min
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("ml/min")]
        mlmin,
        
        lgbar,
        
        /// <summary>
        /// g/l
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("g/l")]
        gl,
        
        /// <summary>
        /// I-Unit
        /// </summary>
        [System.Xml.Serialization.XmlEnumAttribute("I-Unit")]
        IUnit,
        
        pH,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.6.0.20097")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.thyssen.com/xml/schema/qbic")]
    public partial class EinzelwertComplexType
    {
        
        public string Aggregat { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("Einzelwert")]
        public List<EinzelwertType> Einzelwert { get; set; }
        
        /// <summary>
        /// EinzelwertComplexType class constructor
        /// </summary>
        public EinzelwertComplexType()
        {
            this.Einzelwert = new List<EinzelwertType>();
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.6.0.20097")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.thyssen.com/xml/schema/qbic")]
    public partial class Raster1DType
    {
        public Raster1DType()
        {
            this.WerteList = new List<float>();
        }        
        /// <summary>
        /// Anz. der Werte, wird berechnet wenn fehlend.
        /// </summary>
        public int AnzahlX { get; set; }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AnzahlXSpecified { get; set; }
        /// <summary>
        /// Schrittlaenge zwischen zwei Messpunkten, in der spez. x-Richtung und Dimension (Sekunde oder Meter)
        /// </summary>
        public float SegmentgroesseX { get; set; }
        /// <summary>
        /// Darf weggelassen werden, dann wird 0 angenommen.
        /// </summary>
        public float SegmentOffsetX { get; set; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string WerteX
        {
            get
            {
                var stringItems = WerteList.ConvertAll(w => XmlConvert.ToString(w));
                return String.Join(" ", stringItems.ToArray());
            }
            set
            {
                WerteList.Clear();
                foreach (var item in value.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    WerteList.Add(XmlConvert.ToSingle(item));
                }
            }
        }

        [XmlIgnore]
        public List<float> WerteList { get; set; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.6.0.20097")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.thyssen.com/xml/schema/qbic")]
    public partial class StatistikType
    {
        
        public double Min { get; set; }
        public double Max { get; set; }
        public double Avg { get; set; }
        public double StdDev { get; set; }
        public double Median { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.6.0.20097")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.thyssen.com/xml/schema/qbic")]
    public partial class SpurType
    {
                
        private System.Nullable<WerteTypeEnum> typField;
        
        public string Einheit { get; set; }

        public string EinheitLokal { get; set; }

        /// <summary>
        /// Name der Messreihe im statischen Modell
        /// </summary>
        public string Bezeichner { get; set; }
        /// <summary>
        /// Darf weggelassen werden; dann werden die Werte berechnet.
        /// </summary>
        public StatistikType Statistik { get; set; }
        /// added by iba
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeStatistik()
        {
            if (this.Statistik == null)
                return false;

            return (Statistik.Avg + Statistik.Max + Statistik.Min + Statistik.StdDev + Statistik.Median) != 0;
        }
        /// <summary>
        /// Falls Werte relativ sind, kann hier der Bezugswert angegeben werden. Falls die Werte absolut sind, ist der Bezugswert 0.
        /// </summary>
        [System.ComponentModel.DefaultValueAttribute(0)]
        public double Bezugswert { get; set; }
        /// <summary>
        /// Darf weggelassen werden; dann wird true angenommen.
        /// </summary>
        public bool isAbsolut { get; set; }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isAbsolutSpecified { get; set; }
        /// <summary>
        /// Richtung der x-Achse (oder Dimension Zeit, Windungen)
        /// </summary>
        public BezugDimensionEnum DimensionX { get; set; }
        public Raster1DType Raster1D { get; set; }
        
        /// <summary>
        /// SpurType class constructor
        /// </summary>
        public SpurType()
        {
            this.Raster1D = new Raster1DType();
            this.Statistik = new StatistikType();
            this.DimensionX = BezugDimensionEnum.Laenge;
            this.Bezugswert = 0.0;
        }
        
        /// <summary>
        /// Neben den Datenspuren koennen z.B. stat. Daten oder Validitaetsflags vorliegen. Falls fehlend, wird der Typ Daten angenommen.
        /// </summary>
        public WerteTypeEnum Typ
        {
            get
            {
                if (this.typField.HasValue)
                {
                    return this.typField.Value;
                }
                else
                {
                    return default(WerteTypeEnum);
                }
            }
            set
            {
                this.typField = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TypSpecified
        {
            get
            {
                return this.typField.HasValue;
            }
            set
            {
                if (value==false)
                {
                    this.typField = null;
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.6.0.20097")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.thyssen.com/xml/schema/qbic")]
    public enum WerteTypeEnum
    {
        
        Daten,
        
        Validitaet,
        
        Minimum,
        
        Maximum,
        
        StdDev,
    
        Test,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.6.0.20097")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.thyssen.com/xml/schema/qbic")]
    public enum BezugDimensionEnum
    {
        
        Zeit,
        
        Laenge,
        
        Breite,
        
        Dicke,
        
        Windungen,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.6.0.20097")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.thyssen.com/xml/schema/qbic")]
    public partial class MessungType
    {

        public string IDMessgeraet { get; set; }
        /// <summary>
        /// z.B. BEFB02
        /// </summary>
        public string Aggregat { get; set; }
        public BandlaufrichtungEnum Bandlaufrichtung { get; set; }
        public bool Endprodukt { get; set; }
        /// <summary>
        /// z.B. DICKE__AL
        /// </summary>
        public string Gruppe { get; set; }
        
        [XmlIgnore]
        public System.DateTime Messzeitpunkt { get; set; }

        [XmlElement("Messzeitpunkt")]
        public string MesszeitpunktString
        {
            get { return this.Messzeitpunkt.ToString("yyyy-MM-dd'T'HH:mm:sszzz"); }
            set { this.Messzeitpunkt = DateTime.ParseExact(value, "yyyy-MM-dd'T'HH:mm:sszzz", System.Globalization.CultureInfo.CurrentCulture); }
        }
        [System.Xml.Serialization.XmlElementAttribute("Spur")]
        public List<SpurType> Spur { get; set; }
        /// <summary>
        /// Voraussichtlich wird keine weitere Messung zu diesem Arbeitsgangdurchsatz folgen.
        /// </summary>
        public bool LetzteMsgAmDurchsatz { get; set; }
        
        /// <summary>
        /// MessungType class constructor
        /// </summary>
        public MessungType()
        {
            this.Spur = new List<SpurType>();
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.6.0.20097")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.thyssen.com/xml/schema/qbic")]
    public enum BandlaufrichtungEnum
    {
        
        InWalzRichtung,
        
        GegenWalzRichtung,
    }
}
