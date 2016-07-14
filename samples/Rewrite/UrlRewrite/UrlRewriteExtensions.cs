// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;

namespace Rewrite.Structure2
{

    public static class UrlRewriteExtensions
    {
        public static IApplicationBuilder UseRewriter(this IApplicationBuilder app, Action<UrlRewriteBuilder> action)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            var rules = new UrlRewriteBuilder();
            action(rules);
            var options = new UrlRewriteOptions
            {
                Rules = rules.Build(),
            };
            if (rules == null)
            {
                throw new ArgumentNullException(nameof(rules));
            }

            return app.Use(next => new UrlRewriteMiddleware(next, options).Invoke);
        }
    }
}
