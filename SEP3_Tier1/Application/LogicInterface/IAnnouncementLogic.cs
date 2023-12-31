﻿using Domain.DTOs;
using Domain.Models;

namespace Application.LogicInterface;

public interface IAnnouncementLogic
{
    Task<Announcement> CreateAsync(CreateAnnouncementDto dto);
    Task<Announcement> GetByIdAsync(int id);
    Task<IEnumerable<Announcement>> GetAsync(SearchAnnouncementDto searchAnnouncementDto);
    Task UpdateAsync(UpdateAnnouncementDto dto);
    Task DeleteAsync(int id);
}