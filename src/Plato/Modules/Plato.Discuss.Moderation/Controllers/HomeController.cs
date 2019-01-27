﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Localization;
using Plato.Discuss.Models;
using Plato.Entities.Services;
using Plato.Entities.Stores;
using Plato.Internal.Hosting.Abstractions;
using Plato.Internal.Layout.Alerts;
using Plato.Internal.Models.Users;
using Plato.Internal.Security.Abstractions;
using Plato.Internal.Stores.Abstractions.Users;
using Plato.Moderation.Models;
using Plato.Moderation.Stores;
using YamlDotNet.Serialization;

namespace Plato.Discuss.Moderation.Controllers
{
    public class HomeController : Controller
    {

        private readonly IAuthorizationService _authorizationService;

        private readonly IEntityManager<Topic> _entityManager;
        private readonly IContextFacade _contextFacade;
        private readonly IEntityStore<Topic> _entityStore;
        private readonly IPlatoUserStore<User> _userStore;
        private readonly IModeratorStore<Moderator> _moderatorStore;
        private readonly IAlerter _alerter;

        public IHtmlLocalizer T { get; }

        public IStringLocalizer S { get; }

        
        public HomeController(
            IHtmlLocalizer<AdminController> htmlLocalizer,
            IStringLocalizer<AdminController> stringLocalizer,
            IPlatoUserStore<User> userStore, 
            IModeratorStore<Moderator> moderatorStore,
            IEntityStore<Topic> entityStore,
            IContextFacade contextFacade,
            IAuthorizationService authorizationService,
            IEntityManager<Topic> entityManager, IAlerter alerter)
        {
            _userStore = userStore;
            _moderatorStore = moderatorStore;
            _entityStore = entityStore;
            _contextFacade = contextFacade;
            _authorizationService = authorizationService;
            _entityManager = entityManager;
            _alerter = alerter;

            T = htmlLocalizer;
            S = stringLocalizer;

        }
        
        public async Task<IActionResult> HideTopic(string id)
        {

            // Ensure we have a valid id
            var ok = int.TryParse(id, out int entityId);
            if (!ok)
            {
                return NotFound();
            }

            var topic = await _entityStore.GetByIdAsync(entityId);

            // Ensure the topic exists
            if (topic == null)
            {
                return NotFound();
            }
            
            // Ensure we have permission
            if (!await _authorizationService.AuthorizeAsync(User, topic.CategoryId, ModeratorPermissions.HideTopics))
            {
                return Unauthorized();
            }

            var user = await _contextFacade.GetAuthenticatedUserAsync();

            // Update topic
            topic.ModifiedUserId = user?.Id ?? 0;
            topic.ModifiedDate = DateTimeOffset.UtcNow;
            topic.IsPrivate = true;
            
            // Save changes and return results
            var result = await _entityManager.UpdateAsync(topic);
            
            if (result.Succeeded)
            {
                _alerter.Success(T["Topic hidden successfully"]);
            }
            else
            {
                _alerter.Danger(T["Could not hide the topic"]);
            }
            
            // Redirect back to topic
            return Redirect(_contextFacade.GetRouteUrl(new RouteValueDictionary()
            {
                ["Area"] = "Plato.Discuss",
                ["Controller"] = "Home",
                ["Action"] = "Topic",
                ["Id"] = topic.Id,
                ["Alias"] = topic.Alias
            }));

        }

        public async Task<IActionResult> ShowTopic(string id)
        {

            // Ensure we have a valid id
            var ok = int.TryParse(id, out int entityId);
            if (!ok)
            {
                return NotFound();
            }

            var topic = await _entityStore.GetByIdAsync(entityId);

            // Ensure the topic exists
            if (topic == null)
            {
                return NotFound();
            }

            // Ensure we have permission
            if (!await _authorizationService.AuthorizeAsync(User, topic.CategoryId, ModeratorPermissions.ShowTopics))
            {
                return Unauthorized();
            }

            // Get authenticated user
            var user = await _contextFacade.GetAuthenticatedUserAsync();

            // Update topic
            topic.ModifiedUserId = user?.Id ?? 0;
            topic.ModifiedDate = DateTimeOffset.UtcNow;
            topic.IsPrivate = false;

            // Save changes and return results
            var result = await _entityManager.UpdateAsync(topic);

            if (result.Succeeded)
            {
                _alerter.Success(T["Topic made visible"]);
            }
            else
            {
                _alerter.Danger(T["Could not update the topic"]);
            }

            // Redirect back to topic
            return Redirect(_contextFacade.GetRouteUrl(new RouteValueDictionary()
            {
                ["Area"] = "Plato.Discuss",
                ["Controller"] = "Home",
                ["Action"] = "Topic",
                ["Id"] = topic.Id,
                ["Alias"] = topic.Alias
            }));

        }
        
        public async Task<IActionResult> CloseTopic(string id)
        {

            // Ensure we have a valid id
            var ok = int.TryParse(id, out int entityId);
            if (!ok)
            {
                return NotFound();
            }

            var topic = await _entityStore.GetByIdAsync(entityId);

            // Ensure the topic exists
            if (topic == null)
            {
                return NotFound();
            }

            // Ensure we have permission
            if (!await _authorizationService.AuthorizeAsync(User, topic.CategoryId, ModeratorPermissions.CloseTopics))
            {
                return Unauthorized();
            }

            var user = await _contextFacade.GetAuthenticatedUserAsync();

            // Update topic
            topic.ModifiedUserId = user?.Id ?? 0;
            topic.ModifiedDate = DateTimeOffset.UtcNow;
            topic.IsClosed = true;

            // Save changes and return results
            var result = await _entityManager.UpdateAsync(topic);

            if (result.Succeeded)
            {
                _alerter.Success(T["Topic closed successfully"]);
            }
            else
            {
                _alerter.Danger(T["Could not close the topic"]);
            }

            // Redirect back to topic
            return Redirect(_contextFacade.GetRouteUrl(new RouteValueDictionary()
            {
                ["Area"] = "Plato.Discuss",
                ["Controller"] = "Home",
                ["Action"] = "Topic",
                ["Id"] = topic.Id,
                ["Alias"] = topic.Alias
            }));

        }

        public async Task<IActionResult> OpenTopic(string id)
        {

            // Ensure we have a valid id
            var ok = int.TryParse(id, out int entityId);
            if (!ok)
            {
                return NotFound();
            }

            var topic = await _entityStore.GetByIdAsync(entityId);

            // Ensure the topic exists
            if (topic == null)
            {
                return NotFound();
            }

            // Ensure we have permission
            if (!await _authorizationService.AuthorizeAsync(User, topic.CategoryId, ModeratorPermissions.OpenTopics))
            {
                return Unauthorized();
            }

            // Get authenticated user
            var user = await _contextFacade.GetAuthenticatedUserAsync();

            // Update topic
            topic.ModifiedUserId = user?.Id ?? 0;
            topic.ModifiedDate = DateTimeOffset.UtcNow;
            topic.IsClosed = false;

            // Save changes and return results
            var result = await _entityManager.UpdateAsync(topic);

            if (result.Succeeded)
            {
                _alerter.Success(T["Topic opened successfully"]);
            }
            else
            {
                _alerter.Danger(T["Could not open the topic"]);
            }

            // Redirect back to topic
            return Redirect(_contextFacade.GetRouteUrl(new RouteValueDictionary()
            {
                ["Area"] = "Plato.Discuss",
                ["Controller"] = "Home",
                ["Action"] = "Topic",
                ["Id"] = topic.Id,
                ["Alias"] = topic.Alias
            }));

        }

        public async Task<IActionResult> TopicToSpam(string id)
        {

            // Ensure we have a valid id
            var ok = int.TryParse(id, out int entityId);
            if (!ok)
            {
                return NotFound();
            }

            var topic = await _entityStore.GetByIdAsync(entityId);

            // Ensure the topic exists
            if (topic == null)
            {
                return NotFound();
            }

            // Ensure we have permission
            if (!await _authorizationService.AuthorizeAsync(User, topic.CategoryId, ModeratorPermissions.TopicToSpam))
            {
                return Unauthorized();
            }

            var user = await _contextFacade.GetAuthenticatedUserAsync();

            // Update topic
            topic.ModifiedUserId = user?.Id ?? 0;
            topic.ModifiedDate = DateTimeOffset.UtcNow;
            topic.IsSpam = true;

            // Save changes and return results
            var result = await _entityManager.UpdateAsync(topic);

            if (result.Succeeded)
            {
                _alerter.Success(T["Topic marked as SPAM"]);
            }
            else
            {
                _alerter.Danger(T["Could not mark topic as SPAM"]);
            }

            // Redirect back to topic
            return Redirect(_contextFacade.GetRouteUrl(new RouteValueDictionary()
            {
                ["Area"] = "Plato.Discuss",
                ["Controller"] = "Home",
                ["Action"] = "Topic",
                ["Id"] = topic.Id,
                ["Alias"] = topic.Alias
            }));

        }

        public async Task<IActionResult> TopicFromSpam(string id)
        {

            // Ensure we have a valid id
            var ok = int.TryParse(id, out int entityId);
            if (!ok)
            {
                return NotFound();
            }

            var topic = await _entityStore.GetByIdAsync(entityId);

            // Ensure the topic exists
            if (topic == null)
            {
                return NotFound();
            }

            // Ensure we have permission
            if (!await _authorizationService.AuthorizeAsync(User, topic.CategoryId, ModeratorPermissions.TopicFromSpam))
            {
                return Unauthorized();
            }

            var user = await _contextFacade.GetAuthenticatedUserAsync();

            // Update topic
            topic.ModifiedUserId = user?.Id ?? 0;
            topic.ModifiedDate = DateTimeOffset.UtcNow;
            topic.IsSpam = false;

            // Save changes and return results
            var result = await _entityManager.UpdateAsync(topic);

            if (result.Succeeded)
            {
                _alerter.Success(T["Topic removed from SPAM"]);
            }
            else
            {
                _alerter.Danger(T["Could not remove topic from SPAM"]);
            }

            // Redirect back to topic
            return Redirect(_contextFacade.GetRouteUrl(new RouteValueDictionary()
            {
                ["Area"] = "Plato.Discuss",
                ["Controller"] = "Home",
                ["Action"] = "Topic",
                ["Id"] = topic.Id,
                ["Alias"] = topic.Alias
            }));

        }

        public async Task<IActionResult> DeleteTopic(string id)
        {

            // Ensure we have a valid id
            var ok = int.TryParse(id, out int entityId);
            if (!ok)
            {
                return NotFound();
            }

            var topic = await _entityStore.GetByIdAsync(entityId);

            // Ensure the topic exists
            if (topic == null)
            {
                return NotFound();
            }

            // Ensure we have permission
            if (!await _authorizationService.AuthorizeAsync(User, topic.CategoryId, ModeratorPermissions.DeleteTopics))
            {
                return Unauthorized();
            }

            var user = await _contextFacade.GetAuthenticatedUserAsync();

            // Update topic
            topic.ModifiedUserId = user?.Id ?? 0;
            topic.ModifiedDate = DateTimeOffset.UtcNow;
            topic.IsDeleted = true;

            // Save changes and return results
            var result = await _entityManager.UpdateAsync(topic);

            if (result.Succeeded)
            {
                _alerter.Success(T["Topic removed from SPAM"]);
            }
            else
            {
                _alerter.Danger(T["Could not remove topic from SPAM"]);
            }

            // Redirect back to topic
            return Redirect(_contextFacade.GetRouteUrl(new RouteValueDictionary()
            {
                ["Area"] = "Plato.Discuss",
                ["Controller"] = "Home",
                ["Action"] = "Topic",
                ["Id"] = topic.Id,
                ["Alias"] = topic.Alias
            }));

        }

        public async Task<IActionResult> RestoreTopic(string id)
        {

            // Ensure we have a valid id
            var ok = int.TryParse(id, out int entityId);
            if (!ok)
            {
                return NotFound();
            }

            var topic = await _entityStore.GetByIdAsync(entityId);

            // Ensure the topic exists
            if (topic == null)
            {
                return NotFound();
            }

            // Ensure we have permission
            if (!await _authorizationService.AuthorizeAsync(User, topic.CategoryId, ModeratorPermissions.RestoreTopics))
            {
                return Unauthorized();
            }

            var user = await _contextFacade.GetAuthenticatedUserAsync();

            // Update topic
            topic.ModifiedUserId = user?.Id ?? 0;
            topic.ModifiedDate = DateTimeOffset.UtcNow;
            topic.IsDeleted = false;

            // Save changes and return results
            var result = await _entityManager.UpdateAsync(topic);

            if (result.Succeeded)
            {
                _alerter.Success(T["Topic restored successfully"]);
            }
            else
            {
                _alerter.Danger(T["Could not restore topic"]);
            }

            // Redirect back to topic
            return Redirect(_contextFacade.GetRouteUrl(new RouteValueDictionary()
            {
                ["Area"] = "Plato.Discuss",
                ["Controller"] = "Home",
                ["Action"] = "Topic",
                ["Id"] = topic.Id,
                ["Alias"] = topic.Alias
            }));

        }



    }

}
