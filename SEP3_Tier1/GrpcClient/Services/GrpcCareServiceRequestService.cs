﻿using Application.DaoInterface;
using Domain.DTOs;
using Domain.Models;
using Domain.Models.Enums;
using Grpc.Core;
using GrpcClient.Mappers;
using Microsoft.VisualBasic.CompilerServices;

namespace GrpcClient.Services;

public class GrpcCareServiceRequestService : ICareServiceRequestDao
{
    private readonly ServiceService.ServiceServiceClient careRequestClient;
    private readonly IUserDao userService;
    private readonly IAnnouncementDao announcementService;
    private ServiceMapper mapper;

    public GrpcCareServiceRequestService(ServiceService.ServiceServiceClient careRequestClient, IUserDao userDao, IAnnouncementDao announcementDao)
    {
        this.careRequestClient = careRequestClient;
        this.userService = userDao;
        this.announcementService = announcementDao;
        mapper = new ServiceMapper(userService, announcementService);
    }

    public async Task OfferAsync(CreateOfferCareDto dto)
    {
        try
        {
            await careRequestClient
                .RequestStartServiceAsync(new ServiceRequestProto
                {
                    AnnouncementId = dto.AnnouncementId,
                    InitiatorEmail = dto.InitiatorId,
                    RecipientEmail = dto.RecipientId,
                    Status = ServiceRequestProto.Types.Status.Sent
                });
        }
        catch (RpcException e)
        {
            throw new Exception(e.Status.Detail);
        }
    }
    
    public async Task AcceptAsync(int requestId)
    {
        try
        {
            await careRequestClient
                .AcceptStartServiceAsync(new FindRequestServiceProto()
                {
                    RequestId = requestId
                });
        }
        catch (RpcException e)
        {
            throw new Exception(e.Status.Detail);
        }
    }
    
    public async Task DenyAsync(int requestId)
    {
        try
        {
            await careRequestClient
                .DenyStartServiceAsync(new FindRequestServiceProto()
                {
                    RequestId = requestId
                });
        }
        catch (RpcException e)
        {
            throw new Exception(e.Status.Detail);
        }
    }
    
    public async Task EndAsync(int serviceId)
    {
        try
        {
            await careRequestClient
                .EndServiceAsync(new FindServiceProto()
                {
                    ServiceId = serviceId
                });
        }
        catch (RpcException e)
        {
            throw new Exception(e.Status.Detail);
        }
    }
    
    public async Task<IEnumerable<ServiceRequest>> GetRequestsAsync(int announcementId)
    {
        try
        {
            var requestServicesProto = await careRequestClient
                .SearchRequestServicesAsync(new FindAnnouncementProto()
                {
                    Id = announcementId
                });
            return await mapper.MapToEntityList(requestServicesProto);
        }
        catch (RpcException e)
        {
            throw new Exception(e.Status.Detail);
        }
    }
    
    public async Task<Service> GetServiceAsync(int serviceId)
    {
        try
        {
            var proto = await careRequestClient
                .FindServiceAsync(new FindServiceProto()
                {
                    ServiceId = serviceId
                });
            return await mapper.MapToEntity(proto);
        }
        catch (RpcException e)
        {
            throw new Exception(e.Status.Detail);
        }
    }
    
    public async Task<IEnumerable<Service>> GetServicesAsync(SearchServicesDto dto)
    {
        try
        {
            var serviceProto = new SearchServiceProto()
            {
                CaretakerEmail = dto.caretakerEmail,
                PetOwnerEmail = dto.petOwnerEmail,
                Status = (ServiceStatus)(-1)
            };

            if (dto.status != null)
                serviceProto.Status = (ServiceStatus)(int)dto.status;
            
            ServicesProto services = await careRequestClient
                .SearchServicesAsync(serviceProto);
            
            return await mapper.MapToEntityList(services);
        }
        catch (RpcException e)
        {
            throw new Exception(e.Status.Detail);
        }
    }
    
    public async Task AddFeedbackAsync(Feedback feedback)
    {
        try
        {
            await careRequestClient
                .AddFeedbackAsync(new FeedbackProto()
                {
                    Feedback = feedback.FeedbackText,
                    Rating = feedback.Rating,
                    ServiceId = feedback.ServiceId,
                });
        }
        catch (RpcException e)
        {
            throw new Exception(e.Status.Detail);
        }
    }
    
    public async Task DeleteFeedbackAsync(int serviceId, string email)
    {
        try
        {
            await careRequestClient
                .DeleteFeedbackAsync(new FindFeedbackProto()
                {
                    ServiceId = serviceId,
                });
        }
        catch (RpcException e)
        {
            throw new Exception(e.Status.Detail);
        }
    }
    
    public async Task<IEnumerable<Feedback>> GetFeedbacks(string email)
    {
        try
        {
            var protos = await careRequestClient
                .SearchFeedbacksAsync(new FindUserProto()
                {
                    Email = email
                });
            return await mapper.MapToEntityList(protos);
        }
        catch (RpcException e)
        {
            throw new Exception(e.Status.Detail);
        }
    }
}