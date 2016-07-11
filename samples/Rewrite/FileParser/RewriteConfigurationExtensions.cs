// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using Rewrite.Structure2;
using System;
using System.Collections.Generic;
using System.IO;

namespace Rewrite.FileParser
{
    public static class RewriteConfiguration
    {

        public static List<Rule> AddRewriteFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(nameof(path));
            }
            Stream stream = File.Open(path, FileMode.Open);
            return RewriteConfigurationFileParser.Parse(stream);
        }
    }
}
