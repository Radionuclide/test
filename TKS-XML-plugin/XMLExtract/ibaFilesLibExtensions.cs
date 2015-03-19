namespace XmlExtract
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using iba.ibaFilesLiteDotNet;


    internal static class ibaFilesLibExtensions
    {

        internal static string ResolveSignalId(this IbaChannelReader channel, IdFieldLocation idField)
        {
            string value = string.Empty;
            switch (idField)
            {
                case IdFieldLocation.PDA_Comment1:
                    value = channel.Comment1;
                    break;
                case IdFieldLocation.PDA_Comment2:
                    value = channel.Comment2;
                    break;
            }

            if (!String.IsNullOrEmpty(value))
                return value;

            return channel.Name;
        }


    }
}

//namespace System.Runtime.CompilerServices
//{
//    /// <remarks>
//    /// This attribute allows us to define extension methods without 
//    /// requiring .NET Framework 3.5. For more information, see the section,
//    /// <a href="http://msdn.microsoft.com/en-us/magazine/cc163317.aspx#S7">Extension Methods in .NET Framework 2.0 Apps</a>,
//    /// of <a href="http://msdn.microsoft.com/en-us/magazine/cc163317.aspx">Basic Instincts: Extension Methods</a>
//    /// column in <a href="http://msdn.microsoft.com/msdnmag/">MSDN Magazine</a>, 
//    /// issue <a href="http://msdn.microsoft.com/en-us/magazine/cc135410.aspx">Nov 2007</a>.
//    /// </remarks>
//    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly)]
//    sealed partial class ExtensionAttribute : Attribute { }
//}
