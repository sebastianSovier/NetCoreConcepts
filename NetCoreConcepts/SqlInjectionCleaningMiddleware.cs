using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreConcepts
{
    public class SqlInjectionCleaningMiddleware
    {
        private readonly RequestDelegate _next;

        public SqlInjectionCleaningMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;

            if (request.Body != null && request.ContentLength > 0)
            {
                // Temporarily read the body into memory
                var bodyStream = new StreamReader(request.Body);
                var bodyContent = await bodyStream.ReadToEndAsync();

                // Clean the body content for SQL injection
                var cleanedBody = CleanObject(bodyContent);

                // Convert the cleaned body back to a stream
                var cleanedStream = new MemoryStream(Encoding.UTF8.GetBytes(cleanedBody));

                // Assign the cleaned stream to the request body
                request.Body = cleanedStream;

                // Reset the position of the stream
                request.Body.Position = 0;
            }

            // Continue processing the request
            await _next(context);
        }
        public static string LimpiaInyection(string valor)
        {
            if (valor != string.Empty)
            {
                if (valor != null)
                {
                    if (valor != "null")
                    {
                        string[] array =
                        {
                        "CREATE", "ALTER", "DROP", "TRUNCATE", "INSERT", "UPDATE", "DELETE", "SELECT", "&",
                        "FROM", "WHERE", "REPLACE", "FUNCTION", "TABLE", "COLUMN", "ROW", "DATABASE",
                        "%", "!", "$", "(", ")", "'", ":", "[", "]", "{", "}", "+", "*", "=", "#",
                        "%", "!", "$", "(", ")", "'",
                        "[", "]", "+", "*", "=", "#",
                        "DOCTYPE", "applet", "basefont", "body", "button", "frame", "frameset", "head",
                        "html", "iframe", "img", "input", "label", "object", "script", "style", "table",
                        "textarea", "title", "javascript", "prompt", "read", "write", "cookie", "&#",
                        "vbscript", "language", "confirm", "alert", ">", "<", "/", "'\'", "&lt", "&gt",
                        "msgbox", "USERNAME", "USER_ACCOUNT_URL", " USER_ID", "fromCharCode",
                        "SRC", "HTTP", "ONLOAD", "TYPE", "TEXT", "stylesheet", "CSS", "HREF", "LINK",
                        "BACKGROUND", "NAMESPACE", "XML", "DATASRC", "DATAFLD", "CurrentPage", "parameters",
                        "document", "querySelector", "jquery", "innerHTML", "outerHTML", "parseHTML", "insertAdjacentHTML",
                        "ONCLICK", "ONMOUSEOVER", "prepend", "wrapAll", "writeln", "QUERY", "showMessage",
                    };

                        foreach (string s in array)
                        {
                            bool b = valor.ToUpper().Contains(s);

                            if (b)
                            {
                                valor = valor.Replace(s.ToUpper(), string.Empty);
                            }
                        }

                        return valor.Trim();
                    }
                    else
                    {
                        return valor;
                    }
                }
                else
                {
                    return valor!;
                }
            }
            else
            {
                return valor;
            }
        }


        public static T CleanObject<T>(T obj)
            where T : class
        {
            Type type = obj.GetType();

            foreach (PropertyInfo property in type.GetProperties())
            {
                // Ensure that the property is a string or a type that can be cleaned
                if (property.PropertyType == typeof(string))
                {
                    var currentValue = property.GetValue(obj)?.ToString();

                    if (!string.IsNullOrEmpty(currentValue))
                    {
                        // Perform the cleaning operation
                        var cleanedValue = LimpiaInyection(currentValue);

                        // Set the cleaned value back into the property
                        property.SetValue(obj, cleanedValue);
                    }
                }
            }

            // Return the modified object
            return obj;
        }
    }

}
