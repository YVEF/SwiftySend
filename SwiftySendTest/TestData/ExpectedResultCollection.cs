using System;

namespace SwiftySendTest.TestData
{
    internal class ExpectedResultCollection 
    {
        public static string GetResult(Dummy1 dummy) => 
            $"<Dummy1>\r\n  <StringField>{dummy.StringField}</StringField>\r\n</Dummy1>";

        public static string GetResult(Dummy2 dummy)
            => $"<Dummy2>\r\n  <StringProperty>{dummy.StringProperty}</StringProperty>\r\n</Dummy2>";

        public static string GetResult(Dummy3 dummy) => 
            $"<Dummy3>\r\n  <StringProperty>{dummy.StringProperty}</StringProperty>\r\n  <StringField>" +
                $"{dummy.StringField}</StringField>\r\n</Dummy3>";

        public static string GetResult(Dummy4 dummy) =>
            $"<Dummy4>\r\n  <EnumProperty>{dummy.EnumProperty}</EnumProperty>\r\n  <IntField>" +
                $"{dummy.IntField}</IntField>\r\n</Dummy4>";

        public static string GetResult(Dummy5 dummy) =>
            $"<Dummy5>\r\n  <EnumProperty>{dummy.EnumProperty}</EnumProperty>\r\n  <DecimalProperty>{dummy.DecimalProperty}</DecimalProperty>\r\n  " +
            $"<ShortProperty>{dummy.ShortProperty}</ShortProperty>\r\n  <ByteProperty>{dummy.ByteProperty}</ByteProperty>\r\n  <LongField>{dummy.LongField}</LongField>\r\n  " +
            $"<CharProperty>{dummy.CharProperty}</CharProperty>\r\n  <IntField>{dummy.IntField}</IntField>\r\n  <StringField>{dummy.StringField}</StringField>\r\n  " +
            $"<FloatField>{dummy.FloatField}</FloatField>\r\n  <ObjectField>{dummy.ObjectField}</ObjectField>\r\n  <BoolField>{dummy.BoolField.ToString().ToLower()}</BoolField>\r\n</Dummy5>";
    }
}
