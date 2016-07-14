// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Http;
using System;
using System.Text.RegularExpressions;

namespace Rewrite.ConditionParser
{
    public class ConditionExpression : Expression
    {
        public Operand Operand { get; set; }
        public bool Invert { get; set; }
       
        public bool VisitConditionExpression(HttpContext context, Match results, Match previous, string concatTestString)
        {
            // 
            if (Operand is RegexOperand)
            {
                return (Operand as RegexOperand).VisitRegexOperand(previous, concatTestString);
            }
            else if (Operand is StringOperand)
            {
                return (Operand as StringOperand).VisitStringOperand(concatTestString);   
            }
            else if (Operand is IntegerOperand)
            {
                return (Operand as IntegerOperand).VisitIntegerOperand(concatTestString);
            }
            else if (Operand is PropertyOperand)
            {
                return (Operand as PropertyOperand).VisitPropertyOperand(concatTestString);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }

}
