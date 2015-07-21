﻿using System.Linq;
using Orchard.Data;
using Orchard.Recipes.Models;

namespace Orchard.Recipes.Services {
    public class RecipeResultAccessor : IRecipeResultAccessor {
        private readonly IRepository<RecipeStepResultRecord> _recipeStepResultRecordRepository;

        public RecipeResultAccessor(IRepository<RecipeStepResultRecord> recipeStepResultRecordRepository) {
            _recipeStepResultRecordRepository = recipeStepResultRecordRepository;
        }

        public RecipeResult GetResult(string executionId) {
            var query =
                from record in _recipeStepResultRecordRepository.Table
                where record.ExecutionId == executionId
                select record;

            var records = query.ToArray();
            
            var result = new RecipeResult() {
                ExecutionId = executionId,
                Steps =
                    from record in records
                    select new RecipeStepResult {
                        StepName = record.StepName,
                        IsCompleted = record.IsCompleted,
                        IsSuccessful = record.IsSuccessful,
                        ErrorMessage = record.ErrorMessage
                    }
            };

            return result;
        }
    }
}