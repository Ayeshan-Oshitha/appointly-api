using MotorHub.Application.Common.CurrentUser;
using MotorHub.Application.Common.Interfaces.Persistence;
using MotorHub.Application.DTOs.Admin;
using MotorHub.Application.DTOs.Advertisements;
using MotorHub.Domain.Common.Constants;
using MotorHub.Domain.Common.Enum;
using MotorHub.Domain.Infrastructure.Exceptions;
using MapsterMapper;

namespace MotorHub.Application.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IRoleChangeRepository _roleChangeRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IMapper _mapper;
        public AdminService(IAdminRepository adminRepository, IRoleChangeRepository roleChangeRepository, ICurrentUser currentUser, IAdvertisementRepository advertisementRepository, IMapper mapper)
        {
            _adminRepository = adminRepository;
            _roleChangeRepository = roleChangeRepository;
            _currentUser = currentUser;
            _advertisementRepository = advertisementRepository;
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

            // Without this the request is marked Approved for a role the user never asked for -
            // a Seller request could be closed out by granting Admin.
            if (request.RequestedRole != Roles.Admin)
            {
                throw new BadRequestException($"The request asked for the {request.RequestedRole} role, not {Roles.Admin}");
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

            // See PromoteToAdmin: the granted role must be the one that was requested.
            if (request.RequestedRole != Roles.Seller)
            {
                throw new BadRequestException($"The request asked for the {request.RequestedRole} role, not {Roles.Seller}");
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

        public async Task<AdvertisementResponseDto> ApproveAdvertisement(Guid AdvertisementId)
        {
            var existingAd = await _advertisementRepository.GetAdvertisementByIdAsync(AdvertisementId);

            if (existingAd == null)
            {
                throw new NotFoundException("Advertisement not found");
            }

            if (existingAd.Status != AdStatus.Pending)
            {
                throw new BadRequestException("Advertisement is already processed");
            }

             var currentUserId = _currentUser.Id;

            var approved = await _adminRepository.ApproveAdvertisementAsync(AdvertisementId, currentUserId);
            return _mapper.Map<AdvertisementResponseDto>(approved);
        }

        public async Task<AdvertisementResponseDto> RejectAdvertisement(Guid AdvertisementId, string? reason)
        {
            var existingAd = await _advertisementRepository.GetAdvertisementByIdAsync(AdvertisementId);

            if (existingAd == null)
            {
                throw new NotFoundException("Advertisement not found");
            }

            if (existingAd.Status != AdStatus.Pending)
            {
                throw new BadRequestException("Advertisement is already processed");
            }

            var currentUserId = _currentUser.Id;

            var rejected = await _adminRepository.RejectAdvertisementAsync(AdvertisementId, currentUserId, reason);
            return _mapper.Map<AdvertisementResponseDto>(rejected);
        }

        public async Task<AdvertisementResponseDto> UndoAdvertisementReview(Guid AdvertisementId)
        {
            var existingAd = await _advertisementRepository.GetAdvertisementByIdAsync(AdvertisementId);

            if (existingAd == null)
            {
                throw new NotFoundException("Advertisement not found");
            }

            if (existingAd.Status == AdStatus.Pending)
            {
                throw new BadRequestException("Advertisement is already in pending ");
            }

            // Map the repository's result, not the pre-update local, so the response reflects
            // the row after the undo rather than relying on both loads sharing a change tracker.
            var reverted = await _adminRepository.UndoAdvertisementReviewAsync(AdvertisementId);
            return _mapper.Map<AdvertisementResponseDto>(reverted);
        }
    }
}
