using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GFramework.Utils
{
    public static class TemplateHelper
    {
        private const string TEMPLATE_PATH = "GTemplates/";

        public static string GetTemplate(string templateName, Dictionary<string, string> arguments)
        {
            var filePath = $"{TEMPLATE_PATH}{templateName}";
            var textAsset = Resources.Load<TextAsset>(filePath);
            var template = textAsset.text;
            Resources.UnloadAsset(textAsset);
            return arguments.Aggregate(template, (current, argument) => current.Replace($"$[{argument.Key}]", argument.Value));
        }

    }
}