using FluentAssertions;
using iba.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using iba.Plugins;
using iba.Utility;
using System.Reflection;
using System.IO;
using iba.TKS_XML_Plugin;
using AM_OSPC_plugin;
using S7_writer_plugin;
using iba;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using FluentAssertions.Execution;

namespace ibaDatCoordinatorTests
{
    public class TaskDataComparsion
    {
        static TaskDataComparsion()
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(CommunicationObject).TypeHandle);
        }

        #region Task data
        public static IEnumerable<object[]> GetTaskDataSubclasses()
        {
            return typeof(TaskData).Assembly.GetTypes()
                .Where(c => c.IsSubclassOf(typeof(TaskData)) && !c.IsAbstract)
                // CustomTaskData requires additional enviroment and it is not possible to test it without creating mutliple objects
                .Where(c => c != typeof(CustomTaskData) && c != typeof(CustomTaskDataUNC))
                .Select(c => new object[] { c });
        }


        [Theory]
        [MemberData(nameof(GetTaskDataSubclasses))]
        public void TaskDataObjectsCreatedByDefaultCtorShouldBeSame(Type taskDataSubtype)
        {
            TaskData instance1 = Activator.CreateInstance(taskDataSubtype) as TaskData;
            TaskData instance2 = Activator.CreateInstance(taskDataSubtype) as TaskData;
            instance1.IsSameInternal(instance2).Should().BeTrue(because: $"two instances of {taskDataSubtype} are constructed with the default ctor and should be IsSame");
        }



        [Theory]
        [MemberData(nameof(GetTaskDataSubclasses))]
        public void TaskDataObjectsWithDifferentPublicFieldsBeDifferent(Type t)
        {
            var props = t.GetProperties()
                .Where(p => p.DeclaringType == t)
                .Where(p => !(p.GetCustomAttributes(typeof(XmlIgnoreAttribute), false).Length > 0))
                .Where(p => p.CanWrite)
                .Where(p => IsProperyIncluded(p));
            using (new AssertionScope())
                foreach (var p in props)
                {
                    if (p.PropertyType != typeof(string) && p.PropertyType != typeof(long) && p.PropertyType != typeof(int) && p.PropertyType != typeof(bool) && p.PropertyType != typeof(double) && p.PropertyType.BaseType != typeof(Enum))
                        continue;
                    var o1 = Activator.CreateInstance(t) as TaskData;
                    var o2 = Activator.CreateInstance(t) as TaskData;
                
                    object p1, p2;
                    do
                    {
                        p1 = RandObj(p.PropertyType);
                        p2 = RandObj(p.PropertyType);
                    }
                    while (p1.Equals(p2));


                    p.SetValue(o1, p1);
                    p.SetValue(o2, p2);

                    o1.IsSameInternal(o2).Should().BeFalse(because: $"two instances of {t} with property {p} should NOT be IsSame");

                    o1.Clone().IsSameInternal(o1).Should().BeTrue(because: $"object {t} and its clone should be IsSame");
                }
        }

        public bool IsProperyIncluded(PropertyInfo p)
        {
            // TODO test passwords using BASE64
            return
                (p.DeclaringType != typeof(KafkaWriterTaskData)  || p.Name != "SASLPassEncrypted") &&
                (p.DeclaringType != typeof(KafkaWriterTaskData)  || p.Name != "schemaPassEncrypted") &&
                (p.DeclaringType != typeof(ReportData)           || p.Name != "Extension") &&
                (p.DeclaringType != typeof(SplitterTaskData)     || p.Name != "EncryptedDatFilePassword") &&
                (p.DeclaringType != typeof(IfTaskData)           || p.Name != "EncryptedDatFilePassword") &&
                (p.DeclaringType != typeof(UpdateDataTaskData)   || p.Name != "DbPasswordCrypted") &&
                (p.DeclaringType != typeof(UploadTaskData)       || p.Name != "PasswordCrypted") &&
                (p.DeclaringType != typeof(UploadTaskData)       || p.Name != "PrivateKeyPassphraseCrypted") &&
                (p.DeclaringType != typeof(HDCreateEventTaskData)|| p.Name != "EncryptedDatFilePassword") &&
                (p.DeclaringType != typeof(HDCreateEventTaskData)|| p.Name != "EncryptedHDPassword") &&
                (p.DeclaringType != typeof(TaskWithTargetDirData)|| p.Name != "PasswordCrypted");
        }

        static readonly Random rand = new Random();
        public object RandObj(Type t)
        {
            if (t.Name == "String")
            {
                var l = rand.Next(5, 10);
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                return new string(Enumerable.Repeat(chars, l)
                    .Select(s => s[rand.Next(s.Length)]).ToArray());
            }
            else if (t.Name == "Double")
            {
                return rand.NextDouble();
            }
            else if (t.Name == "Int" || t.Name == "Int64" || t.Name == "Int32")
            {
                return rand.Next();
            }
            else if (t.Name == "Boolean")
            {
                return rand.Next(10) % 2 == 0;
            }
            else if (t.IsEnum)
            {
                return rand.Next(0, t.GetEnumValues().Length);
            }
            else if (t.IsClass)
            {
                throw new NotImplementedException();
                var props = t.GetProperties();
                var c = Activator.CreateInstance(t);
                foreach (var p in props)
                {
                    p.SetValue(c, RandObj(p.PropertyType));
                }
            }

            throw new NotImplementedException();
        }
        #endregion


        #region Plugin task
        /*public static IEnumerable<object[]> GetIPluginTaskDataIsSame()
        {
            yield return new object[] { typeof(PluginXMLTask) };
            yield return new object[] { typeof(OSPCTaskData) };
            yield return new object[] { typeof(S7TaskData) };
        }

        [Theory]
        [MemberData(nameof(GetIPluginTaskDataIsSame))]
        public void TaskGetIPluginTaskDataIsSameCreatedByDefaultCtorShouldBeSame(Type taskGetIPluginTaskDataIsSame)
        {
            IPluginTaskDataIsSame instance1 = Activator.CreateInstance(taskGetIPluginTaskDataIsSame) as IPluginTaskDataIsSame;
            IPluginTaskDataIsSame instance2 = Activator.CreateInstance(taskGetIPluginTaskDataIsSame) as IPluginTaskDataIsSame;
            instance1.IsSame(instance2).Should().BeTrue(because: $"two instances of {taskGetIPluginTaskDataIsSame} are constructed with the default ctor and should be IsSame");
        }*/
        #endregion


    }
}
