using System;
using System.Threading;
using System.Threading.Tasks;
using Data.Contracts;
using Entities.Entity;
using Services.Contracts;
using Services.Dtos;

namespace Services.Services
{
    public class OAuthService : IOAuthService
    {
        private readonly IOAuthClientRepository _oAuthClientRepository;

        private readonly IOAuthRefreshTokenRepository _oAuthRefreshTokenRepository;

        public OAuthService(
            IOAuthClientRepository oAuthClientRepository,
            IOAuthRefreshTokenRepository oAuthRefreshTokenRepository)
        {
            _oAuthClientRepository = oAuthClientRepository;
            _oAuthRefreshTokenRepository = oAuthRefreshTokenRepository;
        }

        public Task<int?> GetOAuthClientIdByClientIdAndSecretCodeAsync(
            string clientId,
            Guid secretCode,
            CancellationToken cancellationToken)
        {
            return _oAuthClientRepository.GetOAuthClientIdByClientIdAndSecretCodeAsync(
                clientId,
                secretCode,
                cancellationToken);
        }

        public Task<OAuthRefreshToken> GetOAuthRefreshTokenByUserIdAndRefreshCodeAndClientIdAsync(
            int userId,
            Guid refreshToken,
            int clientId,
            CancellationToken cancellationToken)
        {
            return _oAuthRefreshTokenRepository.GetOAuthRefreshTokenByUserIdAndRefreshCodeAndClientIdAsync(
                userId,
                refreshToken,
                clientId,
                cancellationToken);
        }

        public Task AddRefreshTokenAsync(AddRefreshTokenDto request, CancellationToken cancellationToken)
        {
            OAuthRefreshToken oAuthRefreshToken = new OAuthRefreshToken
            {
                CreatedBy = request.UserId,
                RefreshCode = request.RefreshCode,
                OAuthClientId = request.OAuthClientId,
                CreatedAt = request.CreatedAt,
                ExpiresAt = request.ExpireAt
            };
            return _oAuthRefreshTokenRepository.AddAsync(oAuthRefreshToken, cancellationToken);
        }

        public async Task DeleteAllUserRefreshCodesAsync(int userId, CancellationToken cancellationToken)
        {
            await _oAuthRefreshTokenRepository.DeleteAllUserRefreshCodesAsync(userId, cancellationToken);
        }

        public async Task RenewRefreshTokenAsync(RenewRefreshTokenDto request, CancellationToken cancellationToken)
        {
            OAuthRefreshToken oAuthRefreshToken = new OAuthRefreshToken {Id = request.OAuthRefreshTokenId};

            await _oAuthRefreshTokenRepository.DeleteAsync(oAuthRefreshToken, cancellationToken);

            var newOAuthRefreshToken = new OAuthRefreshToken
            {
                CreatedBy = request.UserId,
                OAuthClientId = request.OAuthClientId,
                RefreshCode = request.NewRefreshToken,
                CreatedAt = request.CreatedAt,
                ExpiresAt = request.ExpiresAt
            };

            await _oAuthRefreshTokenRepository.AddAsync(newOAuthRefreshToken, cancellationToken);
        }
    }
}