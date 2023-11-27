﻿using Application.LogicInterface;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("announcements")]
public class AnnouncementController : ControllerBase
{
    private readonly IAnnouncementLogic logic;

    public AnnouncementController(IAnnouncementLogic logic)
    {
        this.logic = logic;
    }

    [HttpPost]
    public async Task<ActionResult<Announcement>> CreateAnnouncement(CreateAnnouncementDto dto)
    {
        try
        {
            Announcement announcement = await logic.CreateAsync(dto);
            return Created($"/announcements/{announcement.Id}", announcement);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(404, e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult> GetAsync([FromQuery] SearchAnnouncementDto parameters)
    {
        try
        {
            var announcements = await logic.GetAsync(parameters);
            return Ok(announcements);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateAsync([FromBody] UpdateAnnouncementDto dto)
    {
        try
        {
            await logic.UpdateAsync(dto);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            await logic.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}