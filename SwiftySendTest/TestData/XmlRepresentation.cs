using SwiftySendTest.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace SwiftySendTest.TestData
{
    internal class XmlRepresentation 
    {

        #region Flat Objects

        public static string GetXml(Dummy1 dummy) => 
            $"<Dummy1>\r\n  <StringField>{dummy.StringField}</StringField>\r\n</Dummy1>";

        public static string GetXml(Dummy2 dummy)
            => $"<Dummy2>\r\n  <StringProperty>{dummy.StringProperty}</StringProperty>\r\n</Dummy2>";

        public static string GetXml(Dummy3 dummy) => 
            $"<Dummy3>\r\n  <StringProperty>{dummy.StringProperty}</StringProperty>\r\n  <StringField>" +
                $"{dummy.StringField}</StringField>\r\n</Dummy3>";

        public static string GetXml(Dummy4 dummy) =>
            $"<Dummy4>\r\n  <EnumProperty>{dummy.EnumProperty}</EnumProperty>\r\n  <IntField>" +
                $"{dummy.IntField}</IntField>\r\n</Dummy4>";

        public static string GetXml(Dummy5 dummy) =>
            $"<Dummy5>\r\n  <EnumProperty>{dummy.EnumProperty}</EnumProperty>\r\n  <DecimalProperty>{dummy.DecimalProperty}</DecimalProperty>\r\n  " +
            $"<ShortProperty>{dummy.ShortProperty}</ShortProperty>\r\n  <ByteProperty>{dummy.ByteProperty}</ByteProperty>\r\n  <LongField>{dummy.LongField}</LongField>\r\n  " +
            $"<CharProperty>{dummy.CharProperty}</CharProperty>\r\n  <IntField>{dummy.IntField}</IntField>\r\n  <StringField>{dummy.StringField}</StringField>\r\n  " +
            $"<FloatField>{dummy.FloatField}</FloatField>\r\n  <ObjectField>{dummy.ObjectField}</ObjectField>\r\n  <BoolField>{dummy.BoolField.ToBooleanString()}</BoolField>\r\n</Dummy5>";

        public static string GetXml(Dummy6 dummy) =>
            $"<Dummy6>\r\n  <DateTimeProperty>{dummy.DateTimeProperty.ToString()}</DateTimeProperty>\r\n  " +
            $"<DateTimeField>{dummy.DateTimeField}</DateTimeField>\r\n</Dummy6>";



        public static string GetXml(Dummy7 dummy)
        {
            var propertyValue = typeof(Dummy7).GetProperty("IntPrivateProperty", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(dummy);
            var fieldValue = typeof(Dummy7).GetField("DateTimePrivateField", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(dummy);
            return $"<Dummy7>\r\n  <IntPrivateProperty>{propertyValue}</IntPrivateProperty>\r\n  " +
                $"<DateTimePrivateField>{fieldValue}</DateTimePrivateField>\r\n</Dummy7>";
        }



        #endregion


        #region Complex Objects

        public static string GetXml(ComplexDummy1 dummy) =>
            $"<ComplexDummy1>\r\n  <StringProperty>{dummy.StringProperty}</StringProperty>\r\n  " +
            $"<ComplexDummy2Field>\r\n    <BoolField>{dummy.ComplexDummy2Field.BoolField.ToBooleanString()}</BoolField>\r\n " +
            $" </ComplexDummy2Field>\r\n</ComplexDummy1>";


        public static string GetXml(CollectionDummy1 dummy) =>
            $"<CollectionDummy1>\r\n  <ListStringProperty>\r\n    <String>{dummy.ListStringProperty[0]}</String>\r\n    " +
            $"<String>{dummy.ListStringProperty[1]}</String>\r\n    <String>{dummy.ListStringProperty[2]}</String>\r\n  " +
            $"</ListStringProperty>\r\n</CollectionDummy1>";

        public static string GetXml(AbstractCollectionDummy1 dummy) =>
            $"<AbstractCollectionDummy1>\r\n  <ICollectionStringProperty>\r\n    <String>{dummy.ICollectionStringProperty.ToList()[0]}</String>\r\n    " +
            $"<String>{dummy.ICollectionStringProperty.ToList()[1]}</String>\r\n    <String>{dummy.ICollectionStringProperty.ToList()[2]}</String>\r\n  " +
            $"</ICollectionStringProperty>\r\n</AbstractCollectionDummy1>";


        public static string GetXml(EnumerableDummyWithComplexDummy1 dummy) =>
            $"<EnumerableDummyWithComplexDummy1>\r\n  <IEnumerableSimpleDummyProperty>\r\n    <SimpleDummy2>\r\n      " +
            $"<BoolField>{dummy.IEnumerableSimpleDummyProperty.ToList()[0].BoolField.ToBooleanString()}</BoolField>\r\n    " +
            $"</SimpleDummy2>\r\n    <SimpleDummy2>\r\n      <BoolField>{dummy.IEnumerableSimpleDummyProperty.ToList()[1].BoolField.ToBooleanString()}</BoolField>\r\n    " +
            $"</SimpleDummy2>\r\n    <SimpleDummy2>\r\n      " +
            $"<BoolField>{dummy.IEnumerableSimpleDummyProperty.ToList()[2].BoolField.ToBooleanString()}</BoolField>\r\n    </SimpleDummy2>\r\n  " +
            $"</IEnumerableSimpleDummyProperty>\r\n</EnumerableDummyWithComplexDummy1>";


        #endregion
    }
}
