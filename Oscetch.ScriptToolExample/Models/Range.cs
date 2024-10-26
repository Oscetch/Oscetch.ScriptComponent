// Borrowed from dotnet/roslyn-sdk which has this line in the top of the file on github
// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.
// They seem to have switched to a MIT license but the text is still there.. regardless props to dotnet for this class
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Text;

namespace Oscetch.ScriptToolExample.Models
{
    public class Range(ClassifiedSpan classifiedSpan, string text)
    {
        public ClassifiedSpan ClassifiedSpan { get; private set; } = classifiedSpan;
        public string Text { get; private set; } = text;

        public Range(string classification, TextSpan span, SourceText text) :
            this(classification, span, text.GetSubText(span).ToString())
        {
        }

        public Range(string classification, TextSpan span, string text) :
            this(new ClassifiedSpan(classification, span), text)
        {
        }

        public string ClassificationType => ClassifiedSpan.ClassificationType;

        public TextSpan TextSpan => ClassifiedSpan.TextSpan;
    }
}
