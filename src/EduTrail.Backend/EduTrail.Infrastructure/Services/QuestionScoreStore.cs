using EduTrail.Application.QuestionCatalog;
using EduTrail.Domain.Entities;
using EduTrail.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EduTrail.Infrastructure.Services
{
    // EF-backed best-score store. One row per (user, slug), enforced by a unique index;
    // RecordBestScoreAsync only raises a stored score, never lowers it.
    public class QuestionScoreStore : IQuestionScoreStore
    {
        private readonly AppDbContext _db;

        public QuestionScoreStore(AppDbContext db)
        {
            _db = db;
        }

        public async Task<float?> GetBestScoreAsync(Guid userId, string slug, CancellationToken cancellationToken = default)
        {
            if (userId == Guid.Empty || string.IsNullOrWhiteSpace(slug)) return null;

            var row = await _db.QuestionBestScores
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.UserId == userId && s.QuestionSlug == slug, cancellationToken);

            return row?.Score;
        }

        public async Task<float> RecordBestScoreAsync(Guid userId, string slug, float score, CancellationToken cancellationToken = default)
        {
            // No signed-in user or no slug: nothing to persist; just echo the attempt score.
            if (userId == Guid.Empty || string.IsNullOrWhiteSpace(slug)) return score;

            var row = await _db.QuestionBestScores
                .FirstOrDefaultAsync(s => s.UserId == userId && s.QuestionSlug == slug, cancellationToken);

            if (row is null)
            {
                row = new QuestionBestScore { UserId = userId, QuestionSlug = slug, Score = score };
                _db.QuestionBestScores.Add(row);
                await _db.SaveChangesAsync(cancellationToken);
                return score;
            }

            // Only keep the maximum: a weaker later attempt leaves the stored score untouched.
            if (score > row.Score)
            {
                row.Score = score;
                await _db.SaveChangesAsync(cancellationToken);
            }

            return row.Score;
        }
    }
}
