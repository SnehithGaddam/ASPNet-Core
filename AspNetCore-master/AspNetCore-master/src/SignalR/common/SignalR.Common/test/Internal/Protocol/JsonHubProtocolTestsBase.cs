// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.SignalR.Protocol;
using Xunit;

namespace Microsoft.AspNetCore.SignalR.Common.Tests.Internal.Protocol
{
    using static HubMessageHelpers;

    public abstract class JsonHubProtocolTestsBase
    {
        protected abstract IHubProtocol JsonHubProtocol { get; }

        protected abstract IHubProtocol GetProtocolWithOptions(bool useCamelCase, bool ignoreNullValues);

        public static readonly IDictionary<string, string> TestHeaders = new Dictionary<string, string>
        {
            { "Foo", "Bar" },
            { "KeyWith\nNew\r\nLines", "Still Works" },
            { "ValueWithNewLines", "Also\nWorks\r\nFine" },
        };

        // It's cleaner to do this as a prefix and use concatenation rather than string interpolation because JSON is already filled with '{'s.
        public static readonly string SerializedHeaders = "\"headers\":{\"Foo\":\"Bar\",\"KeyWith\\nNew\\r\\nLines\":\"Still Works\",\"ValueWithNewLines\":\"Also\\nWorks\\r\\nFine\"}";

        public static IDictionary<string, JsonProtocolTestData> ProtocolTestData => new[]
        {
            new JsonProtocolTestData("InvocationMessage_HasInvocationId", new InvocationMessage("123", "Target", new object[] { 1, "Foo" }), true, true, "{\"type\":1,\"invocationId\":\"123\",\"target\":\"Target\",\"arguments\":[1,\"Foo\"]}"),
            new JsonProtocolTestData("InvocationMessage_HasBoolArgument", new InvocationMessage(null, "Target", new object[] { true }), true, true, "{\"type\":1,\"target\":\"Target\",\"arguments\":[true]}"),
            new JsonProtocolTestData("InvocationMessage_HasNullArgument", new InvocationMessage(null, "Target", new object[] { null }), true, true, "{\"type\":1,\"target\":\"Target\",\"arguments\":[null]}"),
            new JsonProtocolTestData("InvocationMessage_HasStreamArgument", new InvocationMessage(null, "Target", Array.Empty<object>(), new string[] { "__test_id__" }), true, true, "{\"type\":1,\"target\":\"Target\",\"arguments\":[],\"streamIds\":[\"__test_id__\"]}"),
            new JsonProtocolTestData("InvocationMessage_HasStreamAndNormalArgument", new InvocationMessage(null, "Target", new object[] { 42 }, new string[] { "__test_id__" }), true, true, "{\"type\":1,\"target\":\"Target\",\"arguments\":[42],\"streamIds\":[\"__test_id__\"]}"),
            new JsonProtocolTestData("InvocationMessage_HasMultipleStreams", new InvocationMessage(null, "Target", Array.Empty<object>(), new string[] { "__test_id__", "__test_id2__" }), true, true, "{\"type\":1,\"target\":\"Target\",\"arguments\":[],\"streamIds\":[\"__test_id__\",\"__test_id2__\"]}"),
            new JsonProtocolTestData("InvocationMessage_DateTimeOffsetArgument", new InvocationMessage("Method", new object[] { DateTimeOffset.Parse("2016-05-10T13:51:20+12:34") }), true, true, "{\"type\":1,\"target\":\"Method\",\"arguments\":[\"2016-05-10T13:51:20+12:34\"]}"),

            new JsonProtocolTestData("StreamItemMessage_HasIntegerItem", new StreamItemMessage("123", 1), true, true, "{\"type\":2,\"invocationId\":\"123\",\"item\":1}"),
            new JsonProtocolTestData("StreamItemMessage_HasStringItem", new StreamItemMessage("123", "Foo"), true, true, "{\"type\":2,\"invocationId\":\"123\",\"item\":\"Foo\"}"),
            new JsonProtocolTestData("StreamItemMessage_HasBoolItem", new StreamItemMessage("123", true), true, true, "{\"type\":2,\"invocationId\":\"123\",\"item\":true}"),
            new JsonProtocolTestData("StreamItemMessage_HasNullItem", new StreamItemMessage("123", null), true, true, "{\"type\":2,\"invocationId\":\"123\",\"item\":null}"),

            new JsonProtocolTestData("CompletionMessage_HasIntegerResult", CompletionMessage.WithResult("123", 1), true, true, "{\"type\":3,\"invocationId\":\"123\",\"result\":1}"),
            new JsonProtocolTestData("CompletionMessage_HasStringResult", CompletionMessage.WithResult("123", "Foo"), true, true, "{\"type\":3,\"invocationId\":\"123\",\"result\":\"Foo\"}"),
            new JsonProtocolTestData("CompletionMessage_HasBoolResult", CompletionMessage.WithResult("123", true), true, true, "{\"type\":3,\"invocationId\":\"123\",\"result\":true}"),
            new JsonProtocolTestData("CompletionMessage_HasNullResult", CompletionMessage.WithResult("123", null), true, true, "{\"type\":3,\"invocationId\":\"123\",\"result\":null}"),
            new JsonProtocolTestData("CompletionMessage_HasError", CompletionMessage.WithError("123", "Whoops!"), true, true, "{\"type\":3,\"invocationId\":\"123\",\"error\":\"Whoops!\"}"),
            new JsonProtocolTestData("CompletionMessage_HasErrorAndHeaders", AddHeaders(TestHeaders, CompletionMessage.WithError("123", "Whoops!")), true, true, "{\"type\":3," + SerializedHeaders + ",\"invocationId\":\"123\",\"error\":\"Whoops!\"}"),

            new JsonProtocolTestData("StreamInvocationMessage_HasInvocationId", new StreamInvocationMessage("123", "Target", new object[] { 1, "Foo" }), true, true, "{\"type\":4,\"invocationId\":\"123\",\"target\":\"Target\",\"arguments\":[1,\"Foo\"]}"),
            new JsonProtocolTestData("StreamInvocationMessage_HasBoolArgument", new StreamInvocationMessage("123", "Target", new object[] { true }), true, true, "{\"type\":4,\"invocationId\":\"123\",\"target\":\"Target\",\"arguments\":[true]}"),
            new JsonProtocolTestData("StreamInvocationMessage_HasNullArgument", new StreamInvocationMessage("123", "Target", new object[] { null }), true, true, "{\"type\":4,\"invocationId\":\"123\",\"target\":\"Target\",\"arguments\":[null]}"),
            new JsonProtocolTestData("StreamInvocationMessage_HasStreamArgument", new StreamInvocationMessage("123", "Target", Array.Empty<object>(), new string[] { "__test_id__" }), true, true, "{\"type\":4,\"invocationId\":\"123\",\"target\":\"Target\",\"arguments\":[],\"streamIds\":[\"__test_id__\"]}"),

            new JsonProtocolTestData("CancelInvocationMessage_HasInvocationId", new CancelInvocationMessage("123"), true, true, "{\"type\":5,\"invocationId\":\"123\"}"),
            new JsonProtocolTestData("CancelInvocationMessage_HasHeaders", AddHeaders(TestHeaders, new CancelInvocationMessage("123")), true, true, "{\"type\":5," + SerializedHeaders + ",\"invocationId\":\"123\"}"),

            new JsonProtocolTestData("PingMessage", PingMessage.Instance, true, true, "{\"type\":6}"),

            new JsonProtocolTestData("CloseMessage", CloseMessage.Empty, false, true, "{\"type\":7}"),
            new JsonProtocolTestData("CloseMessage_HasError", new CloseMessage("Error!"), false, true, "{\"type\":7,\"error\":\"Error!\"}"),
            new JsonProtocolTestData("CloseMessage_HasErrorEmptyString", new CloseMessage(""), false, true, "{\"type\":7,\"error\":\"\"}"),
            new JsonProtocolTestData("CloseMessage_HasErrorWithCamelCase", new CloseMessage("Error!"), true, true, "{\"type\":7,\"error\":\"Error!\"}"),

        }.ToDictionary(t => t.Name);

        public static IEnumerable<object[]> ProtocolTestDataNames => ProtocolTestData.Keys.Select(name => new object[] { name });

        public static IDictionary<string, JsonProtocolTestData> OutOfOrderJsonTestData => new[]
        {
            new JsonProtocolTestData("InvocationMessage_StringIsoDateArgumentFirst", new InvocationMessage("Method", new object[] { "2016-05-10T13:51:20+12:34" }), false, true, "{ \"arguments\": [\"2016-05-10T13:51:20+12:34\"], \"type\":1, \"target\": \"Method\" }"),
            new JsonProtocolTestData("InvocationMessage_DateTimeOffsetArgumentFirst", new InvocationMessage("Method", new object[] { DateTimeOffset.Parse("2016-05-10T13:51:20+12:34") }), false, true, "{ \"arguments\": [\"2016-05-10T13:51:20+12:34\"], \"type\":1, \"target\": \"Method\" }"),
            new JsonProtocolTestData("InvocationMessage_IntegerArrayArgumentFirst", new InvocationMessage("Method", new object[] { 1, 2 }), false, true, "{ \"arguments\": [1,2], \"type\":1, \"target\": \"Method\" }"),
            new JsonProtocolTestData("StreamInvocationMessage_IntegerArrayArgumentFirst", new StreamInvocationMessage("3", "Method", new object[] { 1, 2 }), false, true, "{ \"type\":4, \"arguments\": [1,2], \"target\": \"Method\", \"invocationId\": \"3\" }"),
            new JsonProtocolTestData("CompletionMessage_ResultFirst", new CompletionMessage("15", null, 10, hasResult: true), false, true, "{ \"type\":3, \"result\": 10, \"invocationId\": \"15\" }"),
            new JsonProtocolTestData("StreamItemMessage_ItemFirst", new StreamItemMessage("1a", "foo"), false, true, "{ \"item\": \"foo\", \"invocationId\": \"1a\", \"type\":2 }")

        }.ToDictionary(t => t.Name);

        public static IEnumerable<object[]> OutOfOrderJsonTestDataNames => OutOfOrderJsonTestData.Keys.Select(name => new object[] { name });

        [Theory]
        [MemberData(nameof(ProtocolTestDataNames))]
        public void WriteMessage(string protocolTestDataName)
        {
            var testData = ProtocolTestData[protocolTestDataName];

            var expectedOutput = Frame(testData.Json);

            var writer = MemoryBufferWriter.Get();
            try
            {
                var protocol = GetProtocolWithOptions(testData.UseCamelCase, testData.IgnoreNullValues);
                protocol.WriteMessage(testData.Message, writer);
                var json = Encoding.UTF8.GetString(writer.ToArray());

                Assert.Equal(expectedOutput, json);
            }
            finally
            {
                MemoryBufferWriter.Return(writer);
            }
        }

        [Theory]
        [MemberData(nameof(ProtocolTestDataNames))]
        public void ParseMessage(string protocolTestDataName)
        {
            var testData = ProtocolTestData[protocolTestDataName];

            var input = Frame(testData.Json);

            var binder = new TestBinder(testData.Message);
            var data = new ReadOnlySequence<byte>(Encoding.UTF8.GetBytes(input));
            var protocol = GetProtocolWithOptions(testData.UseCamelCase, testData.IgnoreNullValues);
            protocol.TryParseMessage(ref data, binder, out var message);

            Assert.Equal(testData.Message, message, TestHubMessageEqualityComparer.Instance);
        }

        [Theory]
        [InlineData("null", "Unexpected JSON Token Type 'Null'. Expected a JSON Object.")]
        [InlineData("\"foo\"", "Unexpected JSON Token Type 'String'. Expected a JSON Object.")]
        [InlineData("[42]", "Unexpected JSON Token Type 'Array'. Expected a JSON Object.")]
        [InlineData("{}", "Missing required property 'type'.")]

        [InlineData("{\"type\":1,\"headers\":{\"Foo\": 42},\"target\":\"test\",arguments:[]}", "Expected header 'Foo' to be of type String.")]
        [InlineData("{\"type\":1,\"headers\":{\"Foo\": true},\"target\":\"test\",arguments:[]}", "Expected header 'Foo' to be of type String.")]
        [InlineData("{\"type\":1,\"headers\":{\"Foo\": null},\"target\":\"test\",arguments:[]}", "Expected header 'Foo' to be of type String.")]
        [InlineData("{\"type\":1,\"headers\":{\"Foo\": []},\"target\":\"test\",arguments:[]}", "Expected header 'Foo' to be of type String.")]

        [InlineData("{\"type\":1}", "Missing required property 'target'.")]
        [InlineData("{\"type\":1,\"invocationId\":42}", "Expected 'invocationId' to be of type String.")]
        [InlineData("{\"type\":1,\"invocationId\":\"42\"}", "Missing required property 'target'.")]
        [InlineData("{\"type\":1,\"invocationId\":\"42\",\"target\":42}", "Expected 'target' to be of type String.")]
        [InlineData("{\"type\":1,\"invocationId\":\"42\",\"target\":\"foo\"}", "Missing required property 'arguments'.")]
        [InlineData("{\"type\":1,\"invocationId\":\"42\",\"target\":\"foo\",\"arguments\":{}}", "Expected 'arguments' to be of type Array.")]

        [InlineData("{\"type\":2}", "Missing required property 'invocationId'.")]
        [InlineData("{\"type\":2,\"invocationId\":42}", "Expected 'invocationId' to be of type String.")]
        [InlineData("{\"type\":2,\"invocationId\":\"42\"}", "Missing required property 'item'.")]

        [InlineData("{\"type\":3}", "Missing required property 'invocationId'.")]
        [InlineData("{\"type\":3,\"invocationId\":42}", "Expected 'invocationId' to be of type String.")]
        [InlineData("{\"type\":3,\"invocationId\":\"42\",\"error\":[]}", "Expected 'error' to be of type String.")]

        [InlineData("{\"type\":4}", "Missing required property 'invocationId'.")]
        [InlineData("{\"type\":4,\"invocationId\":42}", "Expected 'invocationId' to be of type String.")]
        [InlineData("{\"type\":4,\"invocationId\":\"42\",\"target\":42}", "Expected 'target' to be of type String.")]
        [InlineData("{\"type\":4,\"invocationId\":\"42\",\"target\":\"foo\"}", "Missing required property 'arguments'.")]
        [InlineData("{\"type\":4,\"invocationId\":\"42\",\"target\":\"foo\",\"arguments\":{}}", "Expected 'arguments' to be of type Array.")]

        //[InlineData("{\"type\":3,\"invocationId\":\"42\",\"error\":\"foo\",\"result\":true}", "The 'error' and 'result' properties are mutually exclusive.")]
        //[InlineData("{\"type\":3,\"invocationId\":\"42\",\"result\":true", "Unexpected end when reading JSON.")]
        public void InvalidMessages(string input, string expectedMessage)
        {
            input = Frame(input);

            var binder = new TestBinder(Array.Empty<Type>(), typeof(object));
            var data = new ReadOnlySequence<byte>(Encoding.UTF8.GetBytes(input));
            var ex = Assert.Throws<InvalidDataException>(() => JsonHubProtocol.TryParseMessage(ref data, binder, out var _));
            Assert.Equal(expectedMessage, ex.Message);
        }

        [Theory]
        [MemberData(nameof(OutOfOrderJsonTestDataNames))]
        public void ParseOutOfOrderJson(string outOfOrderJsonTestDataName)
        {
            var testData = OutOfOrderJsonTestData[outOfOrderJsonTestDataName];

            var input = Frame(testData.Json);

            var binder = new TestBinder(testData.Message);
            var data = new ReadOnlySequence<byte>(Encoding.UTF8.GetBytes(input));
            var protocol = GetProtocolWithOptions(testData.UseCamelCase, testData.IgnoreNullValues);
            protocol.TryParseMessage(ref data, binder, out var message);

            Assert.Equal(testData.Message, message, TestHubMessageEqualityComparer.Instance);
        }

        [Theory]
        [InlineData("{\"type\":1,\"invocationId\":\"42\",\"target\":\"foo\",\"arguments\":[],\"extraParameter\":\"1\"}")]
        public void ExtraItemsInMessageAreIgnored(string input)
        {
            input = Frame(input);

            var binder = new TestBinder(paramTypes: new[] { typeof(int), typeof(string) }, returnType: typeof(bool));
            var data = new ReadOnlySequence<byte>(Encoding.UTF8.GetBytes(input));
            Assert.True(JsonHubProtocol.TryParseMessage(ref data, binder, out var message));
            Assert.NotNull(message);
        }

        [Theory]
        [InlineData("{\"type\":1,\"invocationId\":\"42\",\"target\":\"foo\",\"arguments\":[]}", "Invocation provides 0 argument(s) but target expects 2.")]
        [InlineData("{\"type\":1,\"arguments\":[], \"invocationId\":\"42\",\"target\":\"foo\"}", "Invocation provides 0 argument(s) but target expects 2.")]
        [InlineData("{\"type\":1,\"invocationId\":\"42\",\"target\":\"foo\",\"arguments\":[ \"abc\", \"xyz\"]}", "Error binding arguments. Make sure that the types of the provided values match the types of the hub method being invoked.")]
        [InlineData("{\"type\":1,\"invocationId\":\"42\",\"arguments\":[ \"abc\", \"xyz\"], \"target\":\"foo\"}", "Error binding arguments. Make sure that the types of the provided values match the types of the hub method being invoked.")]
        [InlineData("{\"type\":4,\"invocationId\":\"42\",\"target\":\"foo\",\"arguments\":[]}", "Invocation provides 0 argument(s) but target expects 2.")]
        [InlineData("{\"type\":4,\"invocationId\":\"42\",\"target\":\"foo\",\"arguments\":[ \"abc\", \"xyz\"]}", "Error binding arguments. Make sure that the types of the provided values match the types of the hub method being invoked.")]
        [InlineData("{\"type\":1,\"invocationId\":\"42\",\"target\":\"foo\",\"arguments\":[1,\"\",{\"1\":1,\"2\":2}]}", "Invocation provides 3 argument(s) but target expects 2.")]
        [InlineData("{\"type\":1,\"arguments\":[1,\"\",{\"1\":1,\"2\":2}]},\"invocationId\":\"42\",\"target\":\"foo\"", "Invocation provides 3 argument(s) but target expects 2.")]
        [InlineData("{\"type\":1,\"invocationId\":\"42\",\"target\":\"foo\",\"arguments\":[1,[1]]}", "Error binding arguments. Make sure that the types of the provided values match the types of the hub method being invoked.")]
        [InlineData("{\"type\":1,\"invocationId\":\"42\",\"target\":\"foo\",\"arguments\":[1,[]]}", "Error binding arguments. Make sure that the types of the provided values match the types of the hub method being invoked.")]
        public void ArgumentBindingErrors(string input, string expectedMessage)
        {
            input = Frame(input);

            var binder = new TestBinder(paramTypes: new[] { typeof(int), typeof(string) }, returnType: typeof(bool));
            var data = new ReadOnlySequence<byte>(Encoding.UTF8.GetBytes(input));
            JsonHubProtocol.TryParseMessage(ref data, binder, out var message);
            var bindingFailure = Assert.IsType<InvocationBindingFailureMessage>(message);
            Assert.Equal(expectedMessage, bindingFailure.BindingFailure.SourceException.Message);
        }

        [Theory]
        [InlineData("{\"type\":1,\"invocationId\":\"42\",\"target\":\"foo\",\"arguments\":[[}]}")]
        public void InvalidNestingWhileBindingTypesFails(string input)
        {
            input = Frame(input);

            var binder = new TestBinder(paramTypes: new[] { typeof(int[]) }, returnType: null);
            var data = new ReadOnlySequence<byte>(Encoding.UTF8.GetBytes(input));
            var ex = Assert.Throws<InvalidDataException>(() => JsonHubProtocol.TryParseMessage(ref data, binder, out var message));
            Assert.Equal("Error reading JSON.", ex.Message);
        }

        [Theory]
        [InlineData("{\"type\":1,\"invocationId\":\"42\",\"target\":\"foo\",\"arguments\":[\"2007-03-01T13:00:00Z\"]}")]
        [InlineData("{\"type\":1,\"invocationId\":\"42\",\"arguments\":[\"2007-03-01T13:00:00Z\"],\"target\":\"foo\"}")]
        public void DateTimeArgumentPreservesUtcKind(string input)
        {
            var binder = new TestBinder(new[] { typeof(DateTime) });
            var data = new ReadOnlySequence<byte>(Encoding.UTF8.GetBytes(Frame(input)));
            JsonHubProtocol.TryParseMessage(ref data, binder, out var message);
            var invocationMessage = Assert.IsType<InvocationMessage>(message);

            Assert.Single(invocationMessage.Arguments);
            var dt = Assert.IsType<DateTime>(invocationMessage.Arguments[0]);
            Assert.Equal(DateTimeKind.Utc, dt.Kind);
        }

        [Theory]
        [InlineData("{\"type\":3,\"invocationId\":\"42\",\"target\":\"foo\",\"arguments\":[],\"result\":\"2007-03-01T13:00:00Z\"}")]
        [InlineData("{\"type\":3,\"target\":\"foo\",\"arguments\":[],\"result\":\"2007-03-01T13:00:00Z\",\"invocationId\":\"42\"}")]
        public void DateTimeReturnValuePreservesUtcKind(string input)
        {
            var binder = new TestBinder(typeof(DateTime));
            var data = new ReadOnlySequence<byte>(Encoding.UTF8.GetBytes(Frame(input)));
            JsonHubProtocol.TryParseMessage(ref data, binder, out var message);
            var invocationMessage = Assert.IsType<CompletionMessage>(message);

            var dt = Assert.IsType<DateTime>(invocationMessage.Result);
            Assert.Equal(DateTimeKind.Utc, dt.Kind);
        }

        [Fact]
        public void ReadToEndOfArgumentArrayOnError()
        {
            var binder = new TestBinder(new[] { typeof(string) });
            var data = new ReadOnlySequence<byte>(Encoding.UTF8.GetBytes(Frame("{\"type\":1,\"invocationId\":\"42\",\"target\":\"foo\",\"arguments\":[[],{\"target\":\"foo2\"}]}")));
            JsonHubProtocol.TryParseMessage(ref data, binder, out var message);
            var bindingFailure = Assert.IsType<InvocationBindingFailureMessage>(message);

            Assert.Equal("foo", bindingFailure.Target);
        }

        public static string Frame(string input)
        {
            var data = Encoding.UTF8.GetBytes(input);
            return Encoding.UTF8.GetString(FormatMessageToArray(data));
        }

        private static byte[] FormatMessageToArray(byte[] message)
        {
            var output = new MemoryStream();
            output.Write(message, 0, message.Length);
            output.WriteByte(TextMessageFormatter.RecordSeparator);
            return output.ToArray();
        }

        public class JsonProtocolTestData
        {
            public string Name { get; }
            public HubMessage Message { get; }
            public string Json { get; }
            public bool UseCamelCase { get; }
            public bool IgnoreNullValues { get; }

            public JsonProtocolTestData(string name, HubMessage message, bool useCamelCase, bool ignoreNullValues, string json)
            {
                Name = name;
                Message = message;
                Json = json;
                UseCamelCase = useCamelCase;
                IgnoreNullValues = ignoreNullValues;
            }

            public override string ToString() => Name;
        }
    }
}
