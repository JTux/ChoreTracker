using ChoreTracker.Services;
using ChoreTracker.Services.DataContract.Group;
using ChoreTracker.Web.DataContract.Group;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChoreTracker.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/G")]
    public class GroupController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get()
        {
            var service = GetGroupService();
            var groups = service.GetMyGroups();
            return Ok(groups);
        }

        [Route("{id:int}")]
        public IHttpActionResult GetGroupById(int id)
        {
            var service = GetGroupService();
            var groups = service.GetGroupInfo(id);

            if (groups.GroupId == 0) return BadRequest();

            return Ok(groups);
        }

        [Route("Create")]
        public IHttpActionResult CreateGroup(GroupCreateDTO dto)
        {
            var service = GetGroupService();

            var rao = new GroupCreateRAO
            {
                GroupName = dto.GroupName
            };

            if (service.CreateGroup(rao))
                return Ok();
            else
                return BadRequest();
        }

        [Route("Join")]
        public IHttpActionResult JoinGroup(GroupJoinDTO dto)
        {
            var service = GetGroupService();

            var rao = new GroupJoinRAO
            {
                GroupInviteKey = dto.GroupInviteKey
            };

            if (service.JoinGroup(rao))
                return Ok();
            else
                return BadRequest();
        }

        [Route("Accept")]
        public IHttpActionResult AcceptApplicant(GroupAcceptanceDTO dto)
        {
            var service = GetGroupService();

            var rao = new GroupAcceptanceRAO
            {
                Accepted = true,
                GroupId = dto.GroupId,
                GroupMemberId = dto.GroupMemberId
            };

            if (service.Acceptance(rao))
                return Ok();
            else
                return BadRequest();
        }

        [Route("Leave")]
        public IHttpActionResult LeaveGroup(GroupLeaveDTO dto)
        {
            var service = GetGroupService();

            var rao = new GroupLeaveRAO
            {
                GroupId = dto.GroupId,
                GroupInviteKey = dto.GroupInviteKey
            };

            if (service.LeaveGroup(rao))
                return Ok();
            else
                return BadRequest();
        }

        [Route("NewKey/{id:int}")]
        public IHttpActionResult GenerateNewKey(int id)
        {
            var service = GetGroupService();

            if (service.EditGroupInviteKey(id))
                return Ok();
            else
                return BadRequest();
        }


        private GroupService GetGroupService() => new GroupService(Guid.Parse(User.Identity.GetUserId()));
    }
}
