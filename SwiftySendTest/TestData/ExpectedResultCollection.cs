using System;

namespace SwiftySendTest.TestData
{
    public class ExpectedResultCollection
    {
        public static Func<Dummy1, string> Dummy1ExpectedResult = (x) => $"<Dummy1>\r\n  <StringField>{x.StringField}</StringField>\r\n</Dummy1>";
        public static Func<Dummy2, string> Dummy2ExpectedResult = (x) => $"<Dummy2>\r\n  <StringProperty>{x.StringProperty}</StringProperty>\r\n</Dummy2>";
        public static Func<Dummy3, string> Dummy3ExpectedTest = (x) => $"<Dummy3>\r\n  <StringProperty>{x.StringProperty}</StringProperty>\r\n  <StringField>" +
                $"{x.StringField}</StringField>\r\n</Dummy3>";
    }
}
