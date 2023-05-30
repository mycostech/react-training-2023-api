using System;
using Microsoft.EntityFrameworkCore;
using ReactTraining2023.Data.Models;
using ReactTraining2023.Services.Interfaces;

namespace ReactTraining2023.Services
{
	public class AppScoreService : IAppScoreService
	{
        private readonly MycosReact2023TrainingContext _dbContext;

		public AppScoreService(MycosReact2023TrainingContext dbContext)
		{
            _dbContext = dbContext;
        }

        public async Task<AppScore?> AddScore(AppScore newAppScore)
        {
            if (newAppScore != null && newAppScore.Id == 0)
            {
                _dbContext.AppScores.Add(newAppScore);
                await _dbContext.SaveChangesAsync();

                var inserted = await _dbContext.AppScores.FirstOrDefaultAsync(x => x.Id == newAppScore.Id);

                return inserted;
            }

            return null;
        }

        public async Task<bool> DeleteScoreById(int appScoreId)
        {
            var existAppScore = await _dbContext.AppScores.FirstOrDefaultAsync(x => x.Id == appScoreId);

            if (existAppScore != null)
            {
                _dbContext.AppScores.Remove(existAppScore);
                var result = await _dbContext.SaveChangesAsync();

                return (result != 0) ? true : false;
            }

            return false;
        }

        public async Task<List<AppScore>> GetAllScoreByProjectName(string projectName)
        {
            var appScores = await _dbContext.AppScores.Where(x => x.ProjectName == projectName).ToListAsync();

            return appScores;
        }

        public async Task<AppScore?> UpdateScore(AppScore appScore)
        {
            var existAppScore = await _dbContext.AppScores.FirstOrDefaultAsync(x => x.Id == appScore.Id);

            if (existAppScore != null)
            {
                existAppScore.Name = appScore.Name;
                existAppScore.ProjectName = appScore.ProjectName;
                existAppScore.Score = appScore.Score;
                existAppScore.Ip = appScore.Ip;
                existAppScore.TotalTime = appScore.TotalTime;

                await _dbContext.SaveChangesAsync();

                return existAppScore;
            }

            return null;
        }
    }
}

