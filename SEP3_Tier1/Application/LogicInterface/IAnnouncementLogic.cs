﻿using Domain.DTOs;
using Domain.Models;

namespace Application.LogicInterface;

public interface IAnnouncementLogic
{
    Task<Announcement> CreateAsync(AnnouncementCreationDto creationDto);
    Task<IEnumerable<Announcement>> GetAsync(SearchAnnouncementDto searchAnnouncementDto);
}