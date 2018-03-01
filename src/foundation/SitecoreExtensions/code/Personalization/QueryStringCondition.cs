using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
namespace Sitecore.Foundation.SitecoreExtensions.Personalization
{


/// <summary>
/// Condition class for query string.
/// </summary>
/// <typeparam name="T">Parameter type.</typeparam>
public class CookiesCondition<T> : StringOperatorCondition<T> where T : RuleContext
    {
        /// <summary>
        /// Gets or sets query string name.
        /// </summary>
        public string CookieName { get; set; }

        /// <summary>
        /// Gets or sets query string name.
        /// </summary>
        public string CookieValue { get; set; }

        /// <summary>
        /// Main execute method.
        /// </summary>
        /// <param name="ruleContext">Rule context.</param>
        /// <returns>True or false.</returns>
        protected override bool Execute(T ruleContext)
        {
            bool returnValue = false;
            bool foundExactMatch = false;
            bool foundCaseInsensitiveMatch = false;
            bool foundContains = false;
            bool foundStartsWith = false;
            bool foundEndsWith = false;

            Assert.ArgumentNotNull(ruleContext, "ruleContext");

            string myCookieName = this.CookieName ?? string.Empty;
            
            string myCookieValue = this.CookieValue ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(myCookieName))
            {
                if (HttpContext.Current != null)
                {
                    // Populated with QueryString coming into current Page
                    string incomingCookieValue = HttpContext.Current.Request.Cookies[myCookieName]?.Value ?? string.Empty;

                    if (incomingCookieValue == myCookieValue)
                    {
                        // Indicates that QueryString coming into Page is equal to QueryString selected by Content Author
                        foundExactMatch = true;
                        foundCaseInsensitiveMatch = true;
                        foundContains = true;
                        foundStartsWith = true;
                        foundEndsWith = true;

                        return true;
                    }
                    else if (incomingCookieValue.ToLower(CultureInfo.InvariantCulture) == myCookieValue.ToLower(CultureInfo.InvariantCulture))
                    {
                        // Indicates that QueryString coming into Page has case-insensitive match to QueryString selected by Content Author
                        foundCaseInsensitiveMatch = true;

                        // Check other "Found" variables that are not inherently true
                        if (incomingCookieValue.Contains(myCookieValue))
                        {
                            foundContains = true;
                        }

                        if (incomingCookieValue.StartsWith(myCookieValue, true, CultureInfo.InvariantCulture))
                        {
                            foundStartsWith = true;
                        }

                        if (incomingCookieValue.EndsWith(myCookieValue, true, CultureInfo.InvariantCulture))
                        {
                            foundEndsWith = true;
                        }
                    }
                    else if (incomingCookieValue.Contains(myCookieValue))
                    {
                        // Indicates that QueryString coming into Page contains QueryString selected by Content Author
                        foundContains = true;

                        // Check other "Found" variables that are not inherently true
                        if (incomingCookieValue.StartsWith(myCookieValue, true, CultureInfo.InvariantCulture))
                        {
                            foundStartsWith = true;
                        }

                        if (incomingCookieValue.EndsWith(myCookieValue, true, CultureInfo.InvariantCulture))
                        {
                            foundEndsWith = true;
                        }
                    }
                }
            }

            switch (this.GetOperator())
            {
                case StringConditionOperator.Equals: returnValue = foundExactMatch; break;
                case StringConditionOperator.NotEqual: returnValue = !foundExactMatch; break;
                case StringConditionOperator.CaseInsensitivelyEquals: returnValue = foundCaseInsensitiveMatch; break;
                case StringConditionOperator.NotCaseInsensitivelyEquals: returnValue = !foundCaseInsensitiveMatch; break;
                case StringConditionOperator.Contains: returnValue = foundContains; break;
                case StringConditionOperator.StartsWith: returnValue = foundStartsWith; break;
                case StringConditionOperator.EndsWith: returnValue = foundEndsWith; break;
                default: returnValue = false; break;
            }

            return returnValue;
        }
    }

}