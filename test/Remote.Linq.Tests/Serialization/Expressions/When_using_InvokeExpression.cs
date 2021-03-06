﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Remote.Linq.Tests.Serialization.Expressions
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Xunit;
    using RemoteLambdaExpression = Remote.Linq.Expressions.LambdaExpression;

    public abstract class When_using_InvokeExpression
    {
#if !NETCOREAPP1_0
        public class BinaryFormatter : When_using_InvokeExpression
        {
            public BinaryFormatter()
                : base(BinarySerializationHelper.Serialize)
            {
            }
        }
#endif

#if NET
        public class NetDataContractSerializer : When_using_InvokeExpression
        {
            public NetDataContractSerializer()
                : base(NetDataContractSerializationHelper.Serialize)
            {
            }
        }
#endif

        public class DataContractSerializer : When_using_InvokeExpression
        {
            public DataContractSerializer()
                : base(DataContractSerializationHelper.SerializeExpression)
            {
            }
        }

        public class JsonSerializer : When_using_InvokeExpression
        {
            public JsonSerializer()
                : base(JsonSerializationHelper.Serialize)
            {
            }
        }

        public class XmlSerializer : When_using_InvokeExpression
        {
            public XmlSerializer()
                : base(XmlSerializationHelper.SerializeExpression)
            {
            }
        }

        private Expression<Func<decimal, bool>> _originalExpression;

        private RemoteLambdaExpression _remoteExpression;

        private RemoteLambdaExpression _serializedRemoteExpression;

        protected When_using_InvokeExpression(Func<RemoteLambdaExpression, RemoteLambdaExpression> serialize)
        {
            Expression<Func<decimal, bool>> exp = x => x <= 0m;

            var parameter = Expression.Parameter(typeof(decimal), "x");

            var expression = Expression.Lambda<Func<decimal, bool>>(
                Expression.Invoke(exp, parameter), parameter);

            _originalExpression = expression;

            _remoteExpression = expression.ToRemoteLinqExpression();

            _serializedRemoteExpression = serialize(_remoteExpression);
        }

        [Fact]
        public void Expression_block_result_should_be_equal()
        {
            var argument = 0.00001m;

            bool result1 = _originalExpression.Compile()(argument);

            bool result2 = _remoteExpression.ToLinqExpression<decimal, bool>().Compile()(argument);

            bool result3 = _serializedRemoteExpression.ToLinqExpression<decimal, bool>().Compile()(argument);

            false
                .ShouldMatch(result1)
                .ShouldMatch(result2)
                .ShouldMatch(result3);
        }
    }
}