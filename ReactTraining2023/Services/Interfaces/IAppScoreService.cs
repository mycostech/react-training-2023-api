using System;
using ReactTraining2023.Data.Models;

namespace ReactTraining2023.Services.Interfaces
{
	public interface IAppScoreService
	{
		Task<List<AppScore>> GetAllScoreByProjectName(string projectName);

        Task<AppScore?> AddScore(AppScore newAppScore);

        Task<AppScore?> UpdateScore(AppScore appScore);

        Task<bool> DeleteScoreById(int appScoreId);
    }
}

