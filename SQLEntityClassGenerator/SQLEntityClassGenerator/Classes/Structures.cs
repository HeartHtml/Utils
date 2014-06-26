using System;
using System.Collections.Generic;
using System.Linq;
using M3.Entities.ExtensionMethods;

namespace SQLEntityClassGenerator.Classes
{
    public class Structures
    {
        public class SQLEntityGeneratorOptions
        {
            public string FileName { get; set; }
            public string ConnectionString { get; set; }
            public string TableName { get; set; }
            public string NameSpace { get; set; }
            public List<Property> SelectedColumns { get; set; }

            public SQLEntityGeneratorOptions()
            {
                FileName = string.Empty;
                ConnectionString = string.Empty;
                TableName = string.Empty;
                NameSpace = string.Empty;
                SelectedColumns = new List<Property>();
            }
        }

        public class SQLEntityGeneratorValidation
        {
            public bool RequiresConnectionString { get; set; }
            public bool RequiresTableName { get; set; }
            public bool RequiresNameSpace { get; set; }
            public bool RequiresCustomPrefix { get; set; }

            public SQLEntityGeneratorValidation()
            {
                RequiresConnectionString = true;
                RequiresTableName = true;
                RequiresNameSpace = true;
                RequiresCustomPrefix = true;
            }
        }

        public class Preferences
        {
            public bool DisableMessageBoxesPref { get; set; }

            public string DefaultSaveLocationPref { get; set; }

            public string LibraryLocationPref { get; set; }

            public bool AutoIncludeFilesPref { get; set; }

            public bool ModifyProjectFilePref { get; set; }

            public bool DisableToStringPromptPref { get; set; }

            public bool DisableCustomPropertiesPromptPref { get; set; }
        }

        public class CustomProperty
        {
            public string PropertyName { get; set; }

            public string CustomName { get; set; }

            public string EnumerationType { get; set; }

            public bool IsEnumeration { get; set; }

            public override string ToString()
            {
                return IsEnumeration
                           ? string.Format("Property: {0}, Custom Name: {1}, Enum: {2}",
                                           PropertyName,
                                           CustomName,
                                           EnumerationType)
                           : string.Format("Property: {0}, Custom Name: {1}",
                                           PropertyName,
                                           CustomName);
            }
        }

        public static class CustomProperties
        {
            public static List<CustomProperty> SelectedCustomProperties { get; private set; }

            public static void InitializeCustomProperties()
            {
                SelectedCustomProperties = new List<CustomProperty>();
            }

            public static void ClearCustomProperties()
            {
                InitializeCustomProperties();
            }

            public static void AddCustomProperties(IEnumerable<CustomProperty> customProperties)
            {
                foreach (CustomProperty customProperty in customProperties)
                {
                    AddCustomProperty(customProperty);
                }
            }

            public static void AddCustomProperty(CustomProperty customProperty)
            {
                if (!SelectedCustomProperties.SafeAny(p => p.PropertyName.Equals(customProperty.PropertyName)))
                {
                    SelectedCustomProperties.Add(customProperty);
                }
            }

            public static CustomProperty GetCustomProperty(Property property)
            {
                return SelectedCustomProperties.FirstOrDefault(p => p.PropertyName.SafeEquals(property.Name));
            }
        }

        public struct Property
        {
            public string Name { get; set; }
            public string CustomName { get; set; }
            public string Type { get; set; }
            public object DefaultValue { get; set; }
            public bool Nullable { get; set; }
            public int? CharacterLength { get; set; }
            public int? NumericPrecision { get; set; }
            public int? NumericScale { get; set; }
            public string SqlDataType { get; set; }
            public string Comparer { get; set; }
            public bool IsPrimaryKey { get; set; }
            public bool IsIdentity { get; set; }
            public bool IsComputed { get; set; }
            public bool IsEnum { get; set; }
            public bool HasCustomName { get; set; }

            public bool IsDateTime
            {
                get { return Type.Equals("DateTime", StringComparison.InvariantCultureIgnoreCase); }
            }
        }

        public struct Table
        {
            public string Name { get; set; }
            public bool IsView { get; set; }

            public static string DisplayMember { get { return "Name"; } }
            public static string ValueMember { get { return "IsView"; } }
        }

        public enum EnumApplicationID
        {
            Custom = 0,
            GPSApplication = 1,
        }

        public enum EnumLayer
        {
            All = 0,
            StoredProcedure = 1,
            Search = 2,
            Entities = 3,
            Data = 4,
            Business = 5,
            Tests = 6
        }
    }
}
