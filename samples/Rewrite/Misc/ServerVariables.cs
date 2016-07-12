// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using Microsoft.AspNetCore.Http;

namespace Rewrite.ConditionParser
{
    public static class ServerVariables
    {
        public static ServerVariable ApplyServerVariable(HttpContext context, string variable)
        {
            switch (variable)
            {
                case "HTTP_ACCEPT":
                    return ServerVariable.HTTP_ACCEPT;
                case "HTTP_COOKIE":
                    return ServerVariable.HTTP_COOKIE;
                case "HTTP_FORWARDED":
                    return ServerVariable.HTTP_FORWARDED;
                case "HTTP_HOST":
                    return ServerVariable.HTTP_HOST;
                case "HTTP_PROXY_CONNECTION":
                    return ServerVariable.HTTP_PROXY_CONNECTION;
                case "HTTP_REFERER":
                    return ServerVariable.HTTP_REFERER;
                case "HTTP_USER_AGENT":
                    return ServerVariable.HTTP_USER_AGENT;
                case "AUTH_TYPE":
                    return ServerVariable.AUTH_TYPE;
                case "CONN_REMOTE_ADDR":
                    return ServerVariable.CONN_REMOTE_ADDR;
                case "CONTEXT_PREFIX":
                    return ServerVariable.CONTEXT_PREFIX;
                case "CONTEXT_DOCUMENT_ROOT":
                    return ServerVariable.CONTEXT_DOCUMENT_ROOT;
                case "IPV6":
                    return ServerVariable.IPV6;
                case "PATH_INFO":
                    return ServerVariable.PATH_INFO;
                case "QUERY_STRING":
                    return ServerVariable.QUERY_STRING;
                case "REMOTE_ADDR":
                    return ServerVariable.REMOTE_ADDR;
                case "REMOTE_HOST":
                    return ServerVariable.REMOTE_HOST;
                case "REMOTE_IDENT":
                    return ServerVariable.REMOTE_IDENT;
                case "REMOTE_PORT":
                    return ServerVariable.REMOTE_PORT;
                case "REMOTE_USER":
                    return ServerVariable.REMOTE_USER;
                case "REQUEST_METHOD":
                    return ServerVariable.REQUEST_METHOD;
                case "SCRIPT_FILENAME":
                    return ServerVariable.SCRIPT_FILENAME;
                case "DOCUMENT_ROOT":
                    return ServerVariable.DOCUMENT_ROOT;
                case "SCRIPT_GROUP":
                    return ServerVariable.SCRIPT_GROUP;
                case "SCRIPT_USER":
                    return ServerVariable.SCRIPT_USER;
                case "SERVER_ADDR":
                    return ServerVariable.SERVER_ADDR;
                case "SERVER_ADMIN":
                    return ServerVariable.SERVER_ADMIN;
                case "SERVER_NAME":
                    return ServerVariable.SERVER_NAME;
                case "SERVER_PORT":
                    return ServerVariable.SERVER_PORT;
                case "SERVER_PROTOCOL":
                    return ServerVariable.SERVER_PROTOCOL;
                case "SERVER_SOFTWARE":
                    return ServerVariable.SERVER_SOFTWARE;
                case "TIME_YEAR":
                    return ServerVariable.TIME_YEAR;
                case "TIME_MON":
                    return ServerVariable.TIME_MON;
                case "TIME_DAY":
                    return ServerVariable.TIME_DAY;
                case "TIME_HOUR":
                    return ServerVariable.TIME_HOUR;
                case "TIME_MIN":
                    return ServerVariable.TIME_MIN;
                case "TIME_SEC":
                    return ServerVariable.TIME_SEC;
                case "TIME_WDAY":
                    return ServerVariable.TIME_WDAY;
                case "TIME":
                    return ServerVariable.TIME;
                case "API_VERSION":
                    return ServerVariable.API_VERSION;
                case "HTTPS":
                    return ServerVariable.HTTPS;
                case "IS_SUBREQ":
                    return ServerVariable.IS_SUBREQ;
                case "REQUEST_FILENAME":
                    return ServerVariable.REQUEST_FILENAME;
                case "REQUEST_SCHEME":
                    return ServerVariable.REQUEST_SCHEME;
                case "REQUEST_URI":
                    return ServerVariable.REQUEST_URI;
                case "THE_REQUEST":
                    return ServerVariable.THE_REQUEST;
                default:
                    return ServerVariable.NONE;
            }
        }
    }

    public enum ServerVariable
    {   
        NONE,
        HTTP_ACCEPT,
        HTTP_COOKIE,
        HTTP_FORWARDED,
        HTTP_HOST,
        HTTP_PROXY_CONNECTION,
        HTTP_REFERER,
        HTTP_USER_AGENT,
        AUTH_TYPE,
        CONN_REMOTE_ADDR,
        CONTEXT_PREFIX,
        CONTEXT_DOCUMENT_ROOT,
        IPV6,
        PATH_INFO,
        QUERY_STRING,
        REMOTE_ADDR,
        REMOTE_HOST,
        REMOTE_IDENT,
        REMOTE_PORT,
        REMOTE_USER,
        REQUEST_METHOD,
        SCRIPT_FILENAME,
        DOCUMENT_ROOT,
        SCRIPT_GROUP,
        SCRIPT_USER,
        SERVER_ADDR,
        SERVER_ADMIN,
        SERVER_NAME,
        SERVER_PORT,
        SERVER_PROTOCOL,
        SERVER_SOFTWARE,
        TIME_YEAR,
        TIME_MON,
        TIME_DAY,
        TIME_HOUR,
        TIME_MIN,
        TIME_SEC,
        TIME_WDAY,
        TIME,
        API_VERSION,
        HTTPS,
        IS_SUBREQ,
        REQUEST_FILENAME,
        REQUEST_SCHEME,
        REQUEST_URI,
        THE_REQUEST
    }
}
