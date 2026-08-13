using Appointly.Application.Common.CurrentUser;
using Appointly.Application.Common.Interfaces.Persistence;
using Appointly.Application.DTOs.Admin;
using Appointly.Application.DTOs.Advertisements;
using Appointly.Domain.Common.Constants;
using Appointly.Domain.Common.Enum;
using Appointly.Domain.Infrastructure.Exceptions;
using MapsterMapper;

namespace Appointly.Application.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IRoleChangeRepository _roleChangeRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IAdvertismentRepository _advertismentRepository;
        private readonly IMapper _mapper;
        public AdminService(IAdminRepository adminRepository, IRoleChangeRepository roleChangeRepository, ICurrentUser currentUser, IAdvertismentRepository advertismentRepository, IMapper mapper)
        {
            _adminRepository = adminRepository;
            _roleChangeRepository = roleChangeRepository;
            _currentUser = currentUser;
            _advertismentRepository = advertismentRepository;
            _mapper = mapper;
        }

        public async Task<List<UserResponseDto>> GetAllUsers()
        {
            return await _adminRepository.GetAllUsersAsync();
        }

        public async Task<bool> PromoteToAdmin(Guid userId, Guid changeRoleRequestId)
        {
            var request = await _roleChangeRepository.GetRoleChangeRequestByIdAsync(changeRoleRequestId);

            if (request == null)
            {
                throw new NotFoundException("Not found the request");
            }

            if (request.Status != RoleRequestTypes.Pending)
            {
                throw new BadRequestException("The request is already processed");
            }

            if (request.UserId != userId)
            {
                throw new BadRequestException("The request does not belong to the specified user");
            }

            var currentUserId = _currentUser.Id;

            return await _adminRepository.PromoteToAdminAsync(userId, changeRoleRequestId, currentUserId);
        }

        public async Task<bool> PromoteToSeller(Guid userId, Guid changeRoleRequestId)
        {
            var request = await _roleChangeRepository.GetRoleChangeRequestByIdAsync(changeRoleRequestId);

            if(request == null)
            {
                throw new NotFoundException("Not found the request");
            }

            if (request.Status != RoleRequestTypes.Pending)
            {
                throw new BadRequestException("The request is already processed");
            }

            if(request.UserId != userId)
            {
                throw new BadRequestException("The request does not belong to the specified user");
            }

            var currentUserId = _currentUser.Id;

            return await _adminRepository.PromoteToSellerAsync(userId, changeRoleRequestId, currentUserId);
        }

        public async Task<bool> RejectPromoteRequest(Guid userId, Guid changeRoleRequestId, string? rejectReason)
        {
            var request = await _roleChangeRepository.GetRoleChangeRequestByIdAsync(changeRoleRequestId);

            if (request == null)
            {
                throw new NotFoundException("Not found the request");
            }

            if (request.Status != RoleRequestTypes.Pending)
            {
                throw new BadRequestException("The request is already processed");
            }

            if (request.UserId != userId)
            {
                throw new BadRequestException("The request does not belong to the specified user");
            }

            var currentUserId = _currentUser.Id;

            return await _adminRepository.RejectPromoteRequestAsync(changeRoleRequestId, currentUserId, rejectReason);
        }

        public async Task<AdvertisementResponseDto> ApproveAdvertisment(Guid AdvertismentId)
        {
            var existingAd = await _advertismentRepository.GetAdvertismentByIdAsync(AdvertismentId);

            if (existingAd == null)
            {
                throw new NotFoundException("Advertisement not found");
            }

            if (existingAd.Status != AdStatus.Pending)
            {
                throw new BadRequestException("Advertisement is already processed");
            }

             var currentUserId = _currentUser.Id;

            var approved = await _adminRepository.ApproveAdvertisementAsync(AdvertismentId, currentUserId);
            return _mapper.Map<AdvertisementResponseDto>(approved);
        }

        public async Task<AdvertisementResponseDto> RejectAdvertisment(Guid AdvertismentId, string? reason)
        {
            var existingAd = await _advertismentRepository.GetAdvertismentByIdAsync(AdvertismentId);

            if (existingAd == null)
            {
                throw new NotFoundException("Advertisement not found");
            }

            if (existingAd.Status != AdStatus.Pending)
            {
                throw new BadRequestException("Advertisement is already processed");
            }

            var currentUserId = _currentUser.Id;

            var rejected = await _adminRepository.RejectAdvertisementAsync(AdvertismentId, currentUserId, reason);
            return _mapper.Map<AdvertisementResponseDto>(rejected);
        }

        public Task<AdvertisementResponseDto> BlockAdvertisment(Guid AdvertismentId)
        {
            throw new NotImplementedException();
        }

        public async Task<AdvertisementResponseDto> UndoAdvertismentReview(Guid AdvertismentId)
        {
            var existingAd = await _advertismentRepository.GetAdvertismentByIdAsync(AdvertismentId);

            if (existingAd == null)
            {
                throw new NotFoundException("Advertisement not found");
            }

            if (existingAd.Status == AdStatus.Pending)
            {
                throw new BadRequestException("Advertisement is already in pending ");
            }

            await _adminRepository.UndoAdvertismentReviewAsync(AdvertismentId);
            return _mapper.Map<AdvertisementResponseDto>(existingAd);
        }
    }
}
