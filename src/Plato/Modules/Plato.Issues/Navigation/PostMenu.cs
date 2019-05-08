﻿using System;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Routing;
using Plato.Internal.Navigation.Abstractions;
using System.Collections.Generic;
using Plato.Issues.Models;

namespace Plato.Issues.Navigation
{
    public class PostMenu : INavigationProvider
    {
        
        public IStringLocalizer T { get; set; }
        
        public PostMenu(IStringLocalizer localizer)
        {
            T = localizer;
        }

        public void BuildNavigation(string name, INavigationBuilder builder)
        {

            if (!String.Equals(name, "post", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // Get action name
            var actionName = string.Empty;
            if (builder.ActionContext.RouteData.Values.ContainsKey("action"))
            {
                actionName = builder.ActionContext.RouteData.Values["action"].ToString();
            }

            // No need to display the menu when editing entities
            if (actionName.IndexOf("Edit", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return;
            }

            builder
                .Add(T["New"], 1, create => create
                    .IconCss("fal fa-plus")
                    .Attributes(new Dictionary<string, object>()
                    {
                        ["data-display"] = "static"
                    })
                    .Add(T["Issue"], 7, article => article
                        .Action("Create", "Home", "Plato.Issues", new RouteValueDictionary())
                        .Permission(Permissions.PostIssues)
                        .LocalNav()
                    ), new List<string>() {"nav-item", "text-hidden", "text-muted" });
        }
    }

}
