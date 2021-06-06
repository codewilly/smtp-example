using Domain.Interfaces;
using System;
using System.Reflection;

namespace Domain.EmailTemplates
{
    public abstract class BaseEmailTemplate : IEmailTemplate
    {
        /// <summary>
        /// Get a template by the class name and replaces the tags by the equivalent values.
        /// </summary>
        /// <returns>string with replaced template values</returns>
        public string Build()
        {
            string templateName = GetType().Name;
            string template = Properties.Resources.ResourceManager.GetString(templateName);

            if (string.IsNullOrEmpty(template))
                throw new Exception($"Email template '{templateName}' was not found!");

            PropertyInfo[] propertiesInfo = GetType().GetProperties();

            foreach (PropertyInfo propertyInfo in propertiesInfo)
            {
                string name = propertyInfo.Name;
                string value = propertyInfo.GetValue(this)?.ToString();

                template = template.Replace($"[{name}]", value);
            }

            return template;
        }
    }
}
