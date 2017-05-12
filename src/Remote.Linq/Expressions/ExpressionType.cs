﻿// Copyright (c) Christof Senn. All rights reserved. See license.txt in the project root for license information.

namespace Remote.Linq.Expressions
{
    using Aqua;
    using System;

    [Serializable]
    public enum ExpressionType
    {
        Binary,
        Conditional,
        Constant,
        Lambda,
        ListInit,
        MemberAccess,
        MemberInit,
        Call,
        New,
        NewArray,
        Parameter,
        TypeIs,
        Unary,
        Block,
    }
}
