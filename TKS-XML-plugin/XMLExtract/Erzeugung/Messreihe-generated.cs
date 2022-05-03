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
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.6.0.20097")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.thyssen.com/xml/schema/qbic")]
    public partial class MaterialHeaderType
    {
        
        private object itemField;
        
        private System.Nullable<StandortType> standortField;
        
        private System.Nullable<MaterialArtType> materialArtField;
        
        [System.Xml.Serialization.XmlElementAttribute("LokalerIdent", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("TKSIdent", typeof(ulong))]
        public object Item { get; set; }
        
        /// <summary>
        /// BO, DO, DU, SI, SA
        /// </summary>
        public StandortType Standort
        {
            get
            {
                if (this.standortField.HasValue)
                {
                    return this.standortField.Value;
                }
                else
                {
                    return default(StandortType);
                }
            }
            set
            {
                this.standortField = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StandortSpecified
        {
            get
            {
                return this.standortField.HasValue;
            }
            set
            {
                if (value==false)
                {
                    this.standortField = null;
                }
            }
        }
        
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
    /// Gueltige TKS-Standorte.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.6.0.20097")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.thyssen.com/xml/schema/qbic")]
    public enum StandortType
    {
        
        BO,
        
        DO,
        
        DU,
        
        SI,
        
        SA,
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
        
        private System.Nullable<EinheitEnum> einheitField;
        
        /// <summary>
        /// z.B. TemperaturZinkbad
        /// </summary>
        public string Bezeichner { get; set; }
        public string Wert { get; set; }
        
        public EinheitEnum Einheit
        {
            get
            {
                if (this.einheitField.HasValue)
                {
                    return this.einheitField.Value;
                }
                else
                {
                    return default(EinheitEnum);
                }
            }
            set
            {
                this.einheitField = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EinheitSpecified
        {
            get
            {
                return this.einheitField.HasValue;
            }
            set
            {
                if (value==false)
                {
                    this.einheitField = null;
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.6.0.20097")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.thyssen.com/xml/schema/qbic")]
    public enum EinheitEnum
    {
        
        mm,
        
        mikrom,
        
        gradC,
        
        [System.Xml.Serialization.XmlEnumAttribute("m/min")]
        mmin,
        
        kW,
        
        A,
        
        V,
        
        [System.Xml.Serialization.XmlEnumAttribute("g/qm")]
        gqm,
        
        Prozent,
        
        KN,
        
        mbar,
        
        grad,
        
        [System.Xml.Serialization.XmlEnumAttribute("Keine/1")]
        Keine1,
        
        min,
        
        m,
        
        Test,
        
        [System.Xml.Serialization.XmlEnumAttribute("As/qm")]
        Asqm,
        
        [System.Xml.Serialization.XmlEnumAttribute("m/qs")]
        mqs,
        
        [System.Xml.Serialization.XmlEnumAttribute("U/min")]
        Umin,
        
        MPa,
        
        KNm,
        
        Hz,
        
        [System.Xml.Serialization.XmlEnumAttribute("gradC/s")]
        gradCs,
        
        [System.Xml.Serialization.XmlEnumAttribute("m3/s")]
        m3s,
        
        [System.Xml.Serialization.XmlEnumAttribute("m3/h")]
        m3h,
        
        [System.Xml.Serialization.XmlEnumAttribute("t/h")]
        th,
        
        [System.Xml.Serialization.XmlEnumAttribute("mikroS/cm")]
        mikroScm,
        
        [System.Xml.Serialization.XmlEnumAttribute("N/m2")]
        Nm2,
        
        [System.Xml.Serialization.XmlEnumAttribute("m/s")]
        ms,
        
        [System.Xml.Serialization.XmlEnumAttribute("A/qm")]
        Aqm,
        
        cm1,
        
        s,
        
        mikroS,
        
        m3,
        
        [System.Xml.Serialization.XmlEnumAttribute("ml/min")]
        mlmin,
        
        lgbar,
        
        [System.Xml.Serialization.XmlEnumAttribute("g/l")]
        gl,
        
        IUnit,
        
        pH,
        
        ppm,
        
        kg,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.6.0.20097")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.thyssen.com/xml/schema/qbic")]
    public partial class EinzelwertComplexType
    {
        
        /// <summary>
        /// z.B. BEFB02
        /// </summary>
        public string Aggregat { get; set; }
        public System.DateTime Messzeitpunkt { get; set; }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MesszeitpunktSpecified { get; set; }
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
        
        /// <summary>
        /// Anz. der Werte, wird berechnet wenn fehlend.
        /// </summary>
        public int AnzahlX { get; set; }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AnzahlXSpecified { get; set; }
        /// <summary>
        /// Schrittlaenge zwischen zwei Messpunkten, in der spez. x-Richtung und Dimension (Sekunde oder Meter)
        /// </summary>
        public double SegmentgroesseX { get; set; }
        /// <summary>
        /// Darf weggelassen werden, dann wird 0 angenommen.
        /// </summary>
        public double SegmentOffsetX { get; set; }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SegmentOffsetXSpecified { get; set; }
        /// <summary>
        /// Fehlt, wenn y-z-unabhaengig oder 0
        /// </summary>
        public double SegmentOffsetY { get; set; }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SegmentOffsetYSpecified { get; set; }
        /// <summary>
        /// Fehlt wenn z-unabhaengig oder 0
        /// </summary>
        public double SegmentOffsetZ { get; set; }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SegmentOffsetZSpecified { get; set; }
        public string WerteX { get; set; }
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool MedianSpecified { get; set; }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2Code", "3.6.0.20097")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.thyssen.com/xml/schema/qbic")]
    public partial class SpurType
    {
        
        private object itemField;
        
        private System.Nullable<WerteTypeEnum> typField;
        
        private BezugDimensionEnum dimensionXField;
        
        private System.Nullable<BezugDimensionEnum> dimensionYField;
        
        private System.Nullable<BezugDimensionEnum> dimensionZField;
        
        [System.Xml.Serialization.XmlElementAttribute("Einheit", typeof(EinheitEnum))]
        [System.Xml.Serialization.XmlElementAttribute("EinheitLokal", typeof(string))]
        public object Item { get; set; }
        /// <summary>
        /// Name der Messreihe im statischen Modell
        /// </summary>
        public string Bezeichner { get; set; }
        /// <summary>
        /// Darf weggelassen werden; dann werden die Werte berechnet.
        /// </summary>
        public StatistikType Statistik { get; set; }
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
        [System.Xml.Serialization.XmlElementAttribute("Raster1D")]
        public List<Raster1DType> Raster1D { get; set; }
        
        /// <summary>
        /// SpurType class constructor
        /// </summary>
        public SpurType()
        {
            this.Raster1D = new List<Raster1DType>();
            this.Statistik = new StatistikType();
            this.Bezugswert = 0;
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
        
        /// <summary>
        /// Fehlt, wenn Messung unabhaengig von 2. und 3. Dimension.
        /// </summary>
        public BezugDimensionEnum DimensionY
        {
            get
            {
                if (this.dimensionYField.HasValue)
                {
                    return this.dimensionYField.Value;
                }
                else
                {
                    return default(BezugDimensionEnum);
                }
            }
            set
            {
                this.dimensionYField = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DimensionYSpecified
        {
            get
            {
                return this.dimensionYField.HasValue;
            }
            set
            {
                if (value==false)
                {
                    this.dimensionYField = null;
                }
            }
        }
        
        /// <summary>
        /// Fehlt, wenn Messung unabhaengig von  3. Dimension.
        /// </summary>
        public BezugDimensionEnum DimensionZ
        {
            get
            {
                if (this.dimensionZField.HasValue)
                {
                    return this.dimensionZField.Value;
                }
                else
                {
                    return default(BezugDimensionEnum);
                }
            }
            set
            {
                this.dimensionZField = value;
            }
        }
        
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DimensionZSpecified
        {
            get
            {
                return this.dimensionZField.HasValue;
            }
            set
            {
                if (value==false)
                {
                    this.dimensionZField = null;
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
        
        private BandlaufrichtungEnum bandlaufrichtungField;
        
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
        public System.DateTime Messzeitpunkt { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("Spur")]
        public List<SpurType> Spur { get; set; }
        /// <summary>
        /// Voraussichtlich wird keine weitere Messung zu diesem Arbeitsgangdurchsatz folgen.
        /// </summary>
        public bool LetzteMsgAmDurchsatz { get; set; }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LetzteMsgAmDurchsatzSpecified { get; set; }
        
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
