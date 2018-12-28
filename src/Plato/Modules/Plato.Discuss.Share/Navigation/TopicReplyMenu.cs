﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Plato.Discuss.Models;
using Plato.Internal.Navigation;

namespace Plato.Discuss.Share.Navigation
{
    public class TopicReplyMenu : INavigationProvider
    {

        private readonly IActionContextAccessor _actionContextAccessor;
    
        public IStringLocalizer T { get; set; }

        public TopicReplyMenu(
            IStringLocalizer localizer,
            IActionContextAccessor actionContextAccessor)
        {
            T = localizer;
            _actionContextAccessor = actionContextAccessor;
        }

        public void BuildNavigation(string name, NavigationBuilder builder)
        {

            if (!String.Equals(name, "topicreply", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
        
            // Get model from navigation builder
            var reply = builder.ActionContext.HttpContext.Items[typeof(Reply)] as Reply;
            if (reply == null)
            {
                return;
            }

            // Get model from navigation builder
            var topic = builder.ActionContext.HttpContext.Items[typeof(Topic)] as Topic;
            if (topic == null)
            {
                return;
            }

            // Options
            builder
                .Add(T["Options"], int.MaxValue, options => options
                        .IconCss("fa fa-ellipsis-h")
                        .Attributes(new Dictionary<string, object>()
                        {
                            {"data-provide", "tooltip"},
                            {"title", T["Options"]}
                        })
                        .Add(T["Share"], share => share
                            .Action("Index", "Home", "Plato.Discuss.Share", new RouteValueDictionary()
                            {
                                ["entityId"] = reply.EntityId.ToString(),
                                ["alias"] = topic.Alias,
                                ["entityReplyId"] = reply.Id.ToString()
                            })
                            .Attributes(new Dictionary<string, object>()
                            {
                                {"data-toggle", "dialog"},
                                {"data-dialog-modal-css", "modal"},
                                {"data-dialog-css", "modal-dialog"}
                            })
                            //.Permission(Permissions.ManageRoles)
                            .LocalNav()
                        ), new List<string>() {"topic-options", "text-muted", "dropdown-toggle-no-caret", "text-hidden"}
                );

        }

    }

}
